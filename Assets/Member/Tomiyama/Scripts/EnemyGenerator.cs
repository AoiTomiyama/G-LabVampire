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

    [SerializeField, Header("�p�������̏��")]
    private EnemyKeepGenerate[] _enemyKeepGenerates;
    [SerializeField, Header("�g��U���̏��")]
    private EnemyWave[] _enemyWaves;

    private Dictionary<int, ObjectPool<EnemyBehaviour>> _enemyPools;
    private BoxCollider2D _boxCollider2d;
    /// <summary>�|�[�Y�̏��</summary>
    private bool _isPaused = false;

    private void Start()
    {
        _poolPlace = _poolPlace != null ? _poolPlace : transform;

        //�����̃v�[�����Ǘ����鎫�����쐬�B
        _enemyPools = new Dictionary<int, ObjectPool<EnemyBehaviour>>();

        for (int i = 0; i < _enemyPrefabs.Length; i++)
        {
            var prefab = _enemyPrefabs[i];

            //�I�u�W�F�N�g�̃C���X�^���XID���L�[�Ƃ��邱�ƂŌŗL�̃L�[���쐬�B
            var key = prefab.GetInstanceID();

            _enemyPools.Add(key, new ObjectPool<EnemyBehaviour>(() =>
            {
                //�v�[���ɃI�u�W�F�N�g��V���������B
                EnemyBehaviour newEnemy = Instantiate(prefab, _poolPlace).GetComponent<EnemyBehaviour>();
                newEnemy.DamageShowPos = _damageGeneratePos;
                newEnemy.EnemyPool = _enemyPools[key];
                newEnemy.transform.SetAsFirstSibling();

                //�v�[�����ɎQ�Ƃ�ۑ��B
                return newEnemy;
            },
            //�v�[�����̃I�u�W�F�N�g���A�N�e�B�u���B
            enemy => { if (enemy != null) enemy.gameObject.SetActive(true); },
            //�v�[�����̃I�u�W�F�N�g���A�N�e�B�u���B
            enemy => { if (enemy != null) enemy.gameObject.SetActive(false); },
            //�v�[�����̃I�u�W�F�N�g��j���B
            enemy =>
            {
                Destroy(enemy.gameObject);
                Debug.LogWarning("�s���ȃA�N�Z�X�����m���ꂽ���߁A�I�u�W�F�N�g��j�����܂����B");
            },
            true, _startCapacity, _maxCapacity));

            //�V�[���̏��߂ɑ�ʂɐ������ăv�[�����𖞂����B
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
                    //�������Ԃ����̒l�ɂȂ�ꍇ�Ɍx����\������B
                    Debug.LogWarning($"�����Ɏ��s���܂����B�J�n���Ԃ��I�����Ԃ𒴂��Ă��܂��IElement index is {Array.IndexOf(_enemyKeepGenerates, generator)}");
                }
                else
                {
                    StartCoroutine(GenerateDuration(generator._enemies, start, end, generator._rate));
                }
            }
        }
    }
    /// <summary>
    /// �w�肳�ꂽ���Ԃ̊ԁA�G���p���I�ɐ�������B
    /// </summary>
    /// <param name="enemies">��������G��</param>
    /// <param name="startSec">�J�n���鎞��</param>
    /// <param name="endSec">�I�����鎞��</param>
    /// <param name="rate">�������郌�[�g</param>
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
    /// �w�肳�ꂽ�^�C�~���O�ɁA�G���u�ԓI�ɐ�������B
    /// </summary>
    /// <param name="enemies">��������G��</param>
    /// <param name="count">�G�̌�</param>
    /// <param name="waitSecond">��������^�C�~���O</param>
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
    /// �G�𐶐�����B
    /// </summary>
    /// <param name="enemies">��������G��</param>
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
            //�z��̗v�f���Ōォ�A����������̊m���ȉ��̏ꍇ�A�v�[�����琶���B
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
        [Header("�G��Prefab������")]
        public GameObject _enemy;
        [Header("�����m���i�e�[�u�����Ő��l���傫���قǏo�₷���B�j"), Range(0, 100)]
        public int _probability;
    }

    [Serializable]
    private class EnemyWave
    {
        [Header("�J�n���琶���܂ł̕b��"), Range(0, 9)]
        public int _minutes;
        [Range(0, 59)]
        public int _seconds;
        [Header("������"), Range(1, 100)]
        public int _count;
        [Header("��������G�̎��")]
        public EnemyProbability[] _enemies;
    }
    [Serializable]
    private class EnemyKeepGenerate
    {
        [Header("�����J�n����"), Range(0, 9)]
        public int _startMinutes;
        [Range(0, 59)]
        public int _startSeconds;
        [Header("�����I������"), Range(0, 9)]
        public int _endMinutes;
        [Range(0, 59)]
        public int _endSeconds;
        [Header("�������[�g�i��b�Ԃɐ�������ʁj"), Range(1, 50)]
        public int _rate;
        [Header("��������G�̎��")]
        public EnemyProbability[] _enemies;
    }
}
