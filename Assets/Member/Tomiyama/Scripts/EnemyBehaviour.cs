using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// 敵の処理を管轄するクラス。
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Header("敵の種類")]
    private EnemyData _enemyData;

    private Rigidbody2D _rb;
    /// <summary>追跡対象。基本プレイヤー。</summary>
    private Transform _target;
    /// <summary>現在の残り体力。</summary>
    private int _health;
    /// <summary>攻撃インターバルを計測するタイマー。</summary>
    private float _timer;
    /// <summary>対象が接触中かを判定させる。</summary>
    private PlayerBehaviour _player;

    /// <summary>オブジェクトプール</summary>
    private ObjectPool<EnemyBehaviour> _enemyPool;
    public ObjectPool<EnemyBehaviour> EnemyPool { set => _enemyPool = value; }


    private void Start()
    {
        _health = _enemyData.MaxHealth;
        _timer = _enemyData.AttackSpeed;
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        if (_player != null)
        {
            if (_timer < _enemyData.AttackSpeed)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _player.RemoveHealth(_enemyData.Damage);
                _timer = 0;
            }
        }
    }
    private void FixedUpdate()
    {
        if (_target != null)
        {
            _rb.velocity = (_target.position - transform.position).normalized * _enemyData.MoveSpeed;
        }
    }

    /// <summary>
    /// 敵にダメージを与える。
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void RemoveHealth(int damage)
    {
        if (_health + _enemyData.Armor - damage <= 0)
        {
            Death();
        }
        else
        {
            _health -= damage - _enemyData.Armor;
        }
    }
    /// <summary>
    /// 敵の死亡処理。
    /// </summary>
    private void Death()
    {
        //乱数で経験値をドロップするか判定する。
        if (Random.Range(0, 101) < _enemyData.ExpProbability && _enemyData.ExpSize != 0)
        {
            Instantiate(ExpGenerator.Instance.ExpPrefabs[(int)_enemyData.ExpSize], transform.position, Quaternion.identity);
        }
        //キル数を加算。
        PlayerBehaviour.PlayerKillCount++;

        //オブジェクトをプールに返却
        ReturnToPool();
    }
    public void ReturnToPool()
    {
        _enemyPool.Release(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour))
        {
            Debug.Log("Player Enter");
            _player = playerBehaviour;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exit");
            _player = null;
        }
    }
}
