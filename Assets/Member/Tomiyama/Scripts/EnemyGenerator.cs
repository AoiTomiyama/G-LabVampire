using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyGenerator : MonoBehaviour, IPausable
{
    [Header("ObjectPool Settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform _poolPlace;
    [SerializeField] private Transform _damageGeneratePos;
    [SerializeField] private int _startCapacity = 50;
    [SerializeField] private int _maxCapacity = 100;

    [SerializeField, Header("継続生成の情報")]
    private EnemyKeepGenerate[] _enemyKeepGenerates;
    [SerializeField, Header("波状攻撃の情報")]
    private EnemyWave[] _enemyWaves;

    private Dictionary<int, ObjectPool<EnemyBehaviour>> _enemyPools;
    private BoxCollider2D _boxCollider2d;
    /// <summary>ポーズの状態</summary>
    private bool _isPaused = false;

    private void Start()
    {
        _poolPlace = _poolPlace != null ? _poolPlace : transform;

        //複数のプールを管理する辞書を作成。
        _enemyPools = new Dictionary<int, ObjectPool<EnemyBehaviour>>();

        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            var prefab = _enemyPrefabs[i];

            //オブジェクトのインスタンスIDをキーとすることで固有のキーを作成。
            var key = prefab.GetInstanceID();

            _enemyPools.Add(key, new ObjectPool<EnemyBehaviour>(() =>
            {
                //プールにオブジェクトを新しく生成。
                EnemyBehaviour newEnemy = Instantiate(prefab, _poolPlace).GetComponent<EnemyBehaviour>();
                newEnemy.DamageShowPos = _damageGeneratePos;
                newEnemy.EnemyPool = _enemyPools[key];
                newEnemy.transform.SetAsFirstSibling();

                //プール内に参照を保存。
                return newEnemy;
            },
            //プール内のオブジェクトをアクティブ化。
            enemy => { if (enemy != null) enemy.gameObject.SetActive(true); },
            //プール内のオブジェクトを非アクティブ化。
            enemy => { if (enemy != null) enemy.gameObject.SetActive(false); },
            //プール内のオブジェクトを破棄。
            enemy =>
            {
                Destroy(enemy.gameObject);
                Debug.LogWarning("不正なアクセスが検知されたため、オブジェクトを破棄しました。");
            },
            true, _startCapacity, _maxCapacity));

            //シーンの初めに大量に生成してプール内を満たす。
            for (int j = 0; j < _enemyPools.Count; j++)
            {
                var list = new List<EnemyBehaviour>();
                for (int k = 0; k < _startCapacity; k++)
                {
                    list.Add(_enemyPools[key].Get());
                }
                list.ForEach(p => _enemyPools[key].Release(p));
            }
            Debug.Log("Object Pool Setup Complete");
        }

        _boxCollider2d = GetComponent<BoxCollider2D>();
        if (_enemyWaves.Length > 0)
        {
            foreach (EnemyWave wave in _enemyWaves)
            {
                int sec = wave._minutes * 60 + wave._seconds;
                StartCoroutine(GenerateOneShot(wave._enemies, wave._count, sec));
            }
        }
        if (_enemyKeepGenerates.Length > 0)
        {
            foreach (var generator in _enemyKeepGenerates)
            {
                int start = generator._startMinutes * 60 + generator._startSeconds;
                int end = generator._endMinutes * 60 + generator._endSeconds;

                if (start >= end)
                {
                    //生成時間が負の値になる場合に警告を表示する。
                    Debug.LogWarning($"生成に失敗しました。開始時間が終了時間を超えています！Element index is {Array.IndexOf(_enemyKeepGenerates, generator)}");
                }
                else
                {
                    StartCoroutine(GenerateDuration(generator._enemies, start, end, generator._rate));
                }
            }
        }
    }
    /// <summary>
    /// 指定された時間の間、敵を継続的に生成する。
    /// </summary>
    /// <param name="enemies">生成する敵種</param>
    /// <param name="startSec">開始する時間</param>
    /// <param name="endSec">終了する時間</param>
    /// <param name="rate">生成するレート</param>
    private IEnumerator GenerateDuration(EnemyProbability[] enemies, int startSec, int endSec, int rate)
    {
        var elapsed = 0f;
        while (elapsed < startSec)
        {
            yield return new WaitWhile(() => _isPaused);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (elapsed < endSec)
        {
            Generate(enemies);
            var elapsed2 = 0f;
            while (elapsed2 < 1f / rate)
            {
                yield return new WaitWhile(() => _isPaused);
                elapsed2 += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            elapsed += 1f / rate;
        }
    }
    /// <summary>
    /// 指定されたタイミングに、敵を瞬間的に生成する。
    /// </summary>
    /// <param name="enemies">生成する敵種</param>
    /// <param name="count">敵の個数</param>
    /// <param name="waitSecond">生成するタイミング</param>
    /// <returns></returns>
    private IEnumerator GenerateOneShot(EnemyProbability[] enemies, int count, int waitSecond)
    {
        var waitTime = 0f;
        while (waitTime < waitSecond)
        {
            yield return new WaitWhile(() => _isPaused);
            waitTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < count; i++)
        {
            Generate(enemies);
        }
    }
    /// <summary>
    /// 敵を生成する。
    /// </summary>
    /// <param name="enemies">生成する敵種</param>
    private void Generate(EnemyProbability[] enemies)
    {
        int dir = Random.Range(0, 4);
        float randomX = Random.Range(-_boxCollider2d.size.x / 2, _boxCollider2d.size.x / 2);
        float randomY = Random.Range(-_boxCollider2d.size.y / 2, _boxCollider2d.size.y / 2);
        Vector2 pos = transform.position;
        switch (dir)
        {
            case 0:
                pos += new Vector2(randomX, _boxCollider2d.size.y / 2);
                break;
            case 1:
                pos += new Vector2(randomX, -_boxCollider2d.size.y / 2);
                break;
            case 2:
                pos += new Vector2(_boxCollider2d.size.x / 2, randomY);
                break;
            case 3:
                pos += new Vector2(-_boxCollider2d.size.x / 2, randomY);
                break;
        }

        int totalProbability = enemies.Sum(e => e._probability);
        for (int i = 0; i < enemies.Length; i++)
        {
            //配列の要素が最後か、乱数が既定の確率以下の場合、プールから生成。
            if (i == enemies.Length - 1 || Random.Range(1, totalProbability) <= enemies[i]._probability)
            {
                int key = enemies[i]._enemy.GetInstanceID();
                if (_enemyPools.ContainsKey(key))
                {
                    var enemy = _enemyPools[key].Get();
                    enemy.transform.position = pos;
                }
                else
                {
                    Instantiate(enemies[i]._enemy, pos, Quaternion.identity, _poolPlace);
                }
                break;
            }
        }
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    [Serializable]
    private class EnemyProbability
    {
        [Header("敵のPrefabを入れる")]
        public GameObject _enemy;
        [Header("生成確率（テーブル内で数値が大きいほど出やすい。）"), Range(0, 100)]
        public int _probability;
    }

    [Serializable]
    private class EnemyWave
    {
        [Header("開始から生成までの秒数"), Range(0, 9)]
        public int _minutes;
        [Range(0, 59)]
        public int _seconds;
        [Header("生成個数"), Range(1, 100)]
        public int _count;
        [Header("生成する敵の種類")]
        public EnemyProbability[] _enemies;
    }
    [Serializable]
    private class EnemyKeepGenerate
    {
        [Header("生成開始時間"), Range(0, 9)]
        public int _startMinutes;
        [Range(0, 59)]
        public int _startSeconds;
        [Header("生成終了時間"), Range(0, 9)]
        public int _endMinutes;
        [Range(0, 59)]
        public int _endSeconds;
        [Header("生成レート（一秒間に生成する量）"), Range(1, 50)]
        public int _rate;
        [Header("生成する敵の種類")]
        public EnemyProbability[] _enemies;
    }
}
