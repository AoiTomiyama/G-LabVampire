using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
/// <summary>
/// 敵の処理を管轄するクラス。
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Header("敵の種類")]
    private EnemyData _enemyData;
    [SerializeField, Header("ダメージ表示させるオブジェクト")]
    private GameObject _damageText;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    public Rigidbody2D Rb { get => _rb; }

    /// <summary>結界ヒット時の無敵時間</summary>
    public float InvincibleTime { get; set; }
    /// <summary>追跡対象。基本プレイヤー。</summary>
    private Transform _target;
    /// <summary>現在の残り体力。</summary>
    private int _health;
    /// <summary>攻撃インターバルを計測するタイマー。</summary>
    private float _timer;
    /// <summary>対象が接触中かを判定させる。</summary>
    private PlayerBehaviour _player;
    /// <summary>ダメージ生成先のTransform</summary>
    public Transform DamageShowPos { set; private get; }

    /// <summary>オブジェクトプール</summary>
    private ObjectPool<EnemyBehaviour> _enemyPool;
    public ObjectPool<EnemyBehaviour> EnemyPool { set => _enemyPool = value; }


    private void OnEnable()
    {
        _health = _enemyData.MaxHealth;
        InvincibleTime = 0;
        _timer = _enemyData.AttackSpeed;
        _sr = GetComponent<SpriteRenderer>();
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
            _sr.flipX = (transform.position - _target.transform.position).x < 0;
            _rb.velocity = _enemyData.MoveSpeed * Time.fixedDeltaTime * (_target.position - transform.position).normalized;
            if (transform.position.y < _target.position.y)
            {
                _sr.sortingOrder = 1;
            }
            else
            {
                _sr.sortingOrder = -1;
            }
        }
    }

    /// <summary>
    /// 敵にダメージを与える。
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void RemoveHealth(int damage)
    {
        if (_damageText != null)
        {
            var go = Instantiate(_damageText, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity, DamageShowPos);
            go.GetComponent<Text>().text = damage.ToString();
        }
        else
        {
            Debug.LogWarning($"プレファブがアサインされていません！ エラー箇所: {nameof(EnemyBehaviour)}.{nameof(_damageText)}");
        }

        if (_health - damage <= 0)
        {
            Death();
        }
        else
        {
            _health -= damage;
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
            Instantiate(ExpGenerator.Instance.ExpPrefabs[(int)_enemyData.ExpSize], transform.position, Quaternion.identity, ExpGenerator.Instance.transform);
        }
        //キル数を加算。
        PlayerBehaviour.PlayerKillCount++;

        ReturnToPool();
    }
    /// <summary>
    /// オブジェクトをプールに返却
    /// </summary>
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
