using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;
/// <summary>
/// 敵の処理を管轄するクラス。
/// </summary>
public class EnemyBehaviour : MonoBehaviour, IPausable
{
    [SerializeField, Header("敵の種類")]
    private EnemyData _enemyData;
    [SerializeField, Header("ダメージ表示させるオブジェクト")]
    private GameObject _damageText;
    [SerializeField, Header("ボスエネミーかどうか（有効時、カメラ範囲外に行っても消滅しない")]
    private bool _hasBossFlag;

    [Header("敵が死亡時に実行")]
    public UnityEvent OnEnemyDeath;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;

    /// <summary>追跡対象。基本プレイヤー。</summary>
    private Transform _target;
    /// <summary>現在の残り体力。</summary>
    private int _health;
    /// <summary>攻撃インターバルを計測するタイマー。</summary>
    private float _timer;
    /// <summary>対象が接触中かを判定させる。</summary>
    private PlayerBehaviour _player;
    /// <summary>ポーズの状態</summary>
    private bool _isPaused = false;


    /// <summary>ダメージ生成先のTransform</summary>
    public Transform DamageShowPos { set; private get; }
    /// <summary>結界ヒット時の無敵時間</summary>
    public float InvincibleTime { get; set; }
    /// <summary>ボスエネミーかどうか</summary>
    public bool HasBossFlag => _hasBossFlag;

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
        AttackAtPlayer();
    }

    private void AttackAtPlayer()
    {
        if (_player == null || _isPaused) return;
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

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_target == null || (PauseManager.Instance != null && PauseManager.Instance.IsPaused))
        {
            _rb.velocity = Vector3.zero;
            return;
        }
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

    /// <summary>
    /// 敵にダメージを与える。
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void RemoveHealth(int damage)
    {
        AudioManager.Instance.PlaySE(AudioManager.SE.EnemyDamage);
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
        OnEnemyDeath?.Invoke();

        ReturnToPool();
    }
    /// <summary>
    /// オブジェクトをプールに返却
    /// </summary>
    public void ReturnToPool()
    {
        if (_enemyPool != null)
        {
            _enemyPool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
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
    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }
}
