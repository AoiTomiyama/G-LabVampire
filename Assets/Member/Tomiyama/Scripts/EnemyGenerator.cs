using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyGenerator : MonoBehaviour
{
    [Header("ObjectPool Settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private Transform _poolPlace;
    [SerializeField] private int _startCapacity = 50;
    [SerializeField] private int _maxCapacity = 100;

    [SerializeField, Header("îgèÛçUåÇÇÃèÓïÒ")]
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
                StartCoroutine(Generate(wave._enemies, wave._count, sec));
            }
        }
    }

    private IEnumerator Generate(GameObject[] enemies, int count, int waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        for (int i = 0; i < count; i++)
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
    }
    [System.Serializable]
    private class EnemyWave
    {
        [Header("äJénÇ©ÇÁê∂ê¨Ç‹Ç≈ÇÃïbêî"), Range(0, 14)]
        public int _minutes;
        [Range(0, 59)]
        public int _seconds;
        [Header("ê∂ê¨å¬êî"), Range(1, 100)]
        public int _count;
        [Header("ê∂ê¨Ç∑ÇÈìGÇÃéÌóﬁ")]
        public GameObject[] _enemies;
    }
}
