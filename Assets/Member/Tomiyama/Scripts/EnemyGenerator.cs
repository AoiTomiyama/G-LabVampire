using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Header("”góUŒ‚‚Ìî•ñ")]
    private EnemyWave[] _enemyWaves;

    private BoxCollider2D _boxCollider2d;
    private float _timer;
    void Start()
    {
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
            Instantiate(enemies[Random.Range(0, enemies.Length)], pos, Quaternion.identity);
        }
    }

    [System.Serializable]
    private class EnemyWave
    {
        [Header("ŠJn‚©‚ç¶¬‚Ü‚Å‚Ì•b”"), Range(0, 14)]
        public int _minutes;
        [Range(0, 59)]
        public int _seconds;
        [Header("¶¬ŒÂ”"), Range(1, 100)]
        public int _count;
        [Header("¶¬‚·‚é“G‚Ìí—Ş")]
        public GameObject[] _enemies;
    }
}
