using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyGenerator : MonoBehaviour
{
    [Header("ObjectPool Settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform _poolPlace;
    [SerializeField] private int _startCapacity = 50;
    [SerializeField] private int _maxCapacity = 100;
    [SerializeField] private Transform _damageGeneratePos;

    [SerializeField, Header("継続生成の情報")]
    private EnemyKeepGenerate[] _enemyKeepGenerates;
    [SerializeField, Header("波状攻撃の情報")]
    private EnemyWave[] _enemyWaves; 

    private Dictionary<int, ObjectPool<EnemyBehaviour>> _enemyPools;

    private BoxCollider2D _boxCollider2d;
    void Start()
    {
        _poolPlace = _poolPlace != null ? _poolPlace : transform;

        _enemyPools = new Dictionary<int, ObjectPool<EnemyBehaviour>>();
        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            var prefab = _enemyPrefabs[i];
            var key = prefab.GetInstanceID();
            _enemyPools.Add(key, new ObjectPool<EnemyBehaviour>(() =>
            {
                EnemyBehaviour newEnemy = Instantiate(prefab, _poolPlace).GetComponent<EnemyBehaviour>();
                newEnemy.DamageShowPos = _damageGeneratePos;
                newEnemy.EnemyPool = _enemyPools[key];
                newEnemy.transform.SetAsFirstSibling();
                return newEnemy;
            },
            enemy => enemy.gameObject.SetActive(true),
            enemy => enemy.gameObject.SetActive(false),
            enemy => Destroy(enemy.gameObject),
            false, _startCapacity, _maxCapacity));

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
                    Debug.LogWarning($"生成に失敗しました。開始時間が終了時間を超えています！Element index is {Array.IndexOf(_enemyKeepGenerates, generator)}");
                }
                else
                {
                    StartCoroutine(GenerateDuration(generator._enemies, start, end, generator._rate));
                }
            }
        }
    }
    private IEnumerator GenerateDuration(EnemyProbability[] enemies, int startSec, int endSec, int rate)
    {
        float timer = startSec;
        yield return new WaitForSeconds(startSec);
        while (timer < endSec)
        {
            Generate(enemies);
            yield return new WaitForSeconds(1f / rate);
            timer += 1f / rate;
        }
    }
    private IEnumerator GenerateOneShot(EnemyProbability[] enemies, int count, int waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        for (int i = 0; i < count; i++)
        {
            Generate(enemies);
        }
    }
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
            if (Random.Range(1, totalProbability) <= enemies[i]._probability || i == enemies.Length - 1)
            {
                int key = enemies[i]._enemy.GetInstanceID();
                var enemy = _enemyPools[key].Get();
                enemy.transform.position = pos;
                break;
            }
        }
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
