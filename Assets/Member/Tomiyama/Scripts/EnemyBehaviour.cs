using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;
/// <summary>
/// �G�̏������Ǌ�����N���X�B
/// </summary>
public class EnemyBehaviour : MonoBehaviour, IPausable
{
    [SerializeField, Header("�G�̎��")]
    private EnemyData _enemyData;
    [SerializeField, Header("�_���[�W�\��������I�u�W�F�N�g")]
    private GameObject _damageText;
    [SerializeField, Header("�{�X�G�l�~�[���ǂ����i�L�����A�J�����͈͊O�ɍs���Ă����ł��Ȃ�")]
    private bool _hasBossFlag;

    [Header("�G�����S���Ɏ��s")]
    public UnityEvent OnEnemyDeath;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;

    /// <summary>�ǐՑΏہB��{�v���C���[�B</summary>
    private Transform _target;
    /// <summary>���݂̎c��̗́B</summary>
    private int _health;
    /// <summary>�U���C���^�[�o�����v������^�C�}�[�B</summary>
    private float _timer;
    /// <summary>�Ώۂ��ڐG�����𔻒肳����B</summary>
    private PlayerBehaviour _player;
    /// <summary>�|�[�Y�̏��</summary>
    private bool _isPaused = false;


    /// <summary>�_���[�W�������Transform</summary>
    public Transform DamageShowPos { set; private get; }
    /// <summary>���E�q�b�g���̖��G����</summary>
    public float InvincibleTime { get; set; }
    /// <summary>�{�X�G�l�~�[���ǂ���</summary>
    public bool HasBossFlag => _hasBossFlag;

    /// <summary>�I�u�W�F�N�g�v�[��</summary>
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
    /// �G�Ƀ_���[�W��^����B
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
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
            Debug.LogWarning($"�v���t�@�u���A�T�C������Ă��܂���I �G���[�ӏ�: {nameof(EnemyBehaviour)}.{nameof(_damageText)}");
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
    /// �G�̎��S�����B
    /// </summary>
    private void Death()
    {
        //�����Ōo���l���h���b�v���邩���肷��B
        if (Random.Range(0, 101) < _enemyData.ExpProbability && _enemyData.ExpSize != 0)
        {
            Instantiate(ExpGenerator.Instance.ExpPrefabs[(int)_enemyData.ExpSize], transform.position, Quaternion.identity, ExpGenerator.Instance.transform);
        }
        //�L���������Z�B
        PlayerBehaviour.PlayerKillCount++;
        OnEnemyDeath?.Invoke();

        ReturnToPool();
    }
    /// <summary>
    /// �I�u�W�F�N�g���v�[���ɕԋp
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
