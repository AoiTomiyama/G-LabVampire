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

    [SerializeField, Header("Œp‘±¶¬‚Ìî•ñ")]
    private EnemyKeepGenerate[] _enemyKeepGenerates;
    [SerializeField, Header("”góUŒ‚‚Ìî•ñ")]
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
                newEnemy.EnemyPool = _enemyPools[key];
                newEnemy.transform.SetAsFirstSibling();
                newEnemy.gameObject.SetActive(false);
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
                    Debug.LogWarning($"¶¬‚É¸”s‚µ‚Ü‚µ‚½BŠJnŠÔ‚ªI—¹ŠÔ‚ğ’´‚¦‚Ä‚¢‚Ü‚·IElement index is {Array.IndexOf(_enemyKeepGenerates, generator)}");
                }
                else
                {
                    StartCoroutine(GenerateDuration(generator._enemies, start, end, generator._rate));
                }
            }
        }
    }
    private IEnumerator GenerateDuration(GameObject[] enemies, int startSec, int endSec, int rate)
    {
        float timer = startSec;
        yield return new WaitForSeconds(startSec);
        while (timer < endSec)
        {
            Generate(enemies);
            yield return new WaitForSeconds(1f / rate);
            timer += Time.deltaTime;
        }
    }
    private IEnumerator GenerateOneShot(GameObject[] enemies, int count, int waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        for (int i = 0; i < count; i++)
        {
            Generate(enemies);
        }
    }
    private void Generate(GameObject[] enemies)
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
        int key = enemies[Random.Range(0, enemies.Length)].GetInstanceID();
        var enemy = _enemyPools[key].Get();
        enemy.transform.position = pos;
    }
    [System.Serializable]
    private class EnemyWave
    {
        [Header("ŠJn‚©‚ç¶¬‚Ü‚Å‚Ì•b”"), Range(0, 9)]
        public int _minutes;
        [Range(0, 59)]
        public int _seconds;
        [Header("¶¬ŒÂ”"), Range(1, 100)]
        public int _count;
        [Header("¶¬‚·‚é“G‚Ìí—Ş")]
        public GameObject[] _enemies;
    }
    [System.Serializable]
    private class EnemyKeepGenerate
    {
        [Header("¶¬ŠJnŠÔ"), Range(0, 9)]
        public int _startMinutes;
        [Range(0, 59)]
        public int _startSeconds;
        [Header("¶¬I—¹ŠÔ"), Range(0, 9)]
        public int _endMinutes;
        [Range(0, 59)]
        public int _endSeconds;
        [Header("¶¬ƒŒ[ƒgiˆê•bŠÔ‚É¶¬‚·‚é—Êj"), Range(1, 50)]
        public int _rate;
        [Header("¶¬‚·‚é“G‚Ìí—Ş")]
        public GameObject[] _enemies;
    }
}
