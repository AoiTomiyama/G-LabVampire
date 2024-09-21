using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// �G�̏������Ǌ�����N���X�B
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Header("�G�̎��")]
    private EnemyData _enemyData;

    private Rigidbody2D _rb;
    /// <summary>�ǐՑΏہB��{�v���C���[�B</summary>
    private Transform _target;
    /// <summary>���݂̎c��̗́B</summary>
    private int _health;
    /// <summary>�U���C���^�[�o�����v������^�C�}�[�B</summary>
    private float _timer;
    /// <summary>�Ώۂ��ڐG�����𔻒肳����B</summary>
    private PlayerBehaviour _player;

    /// <summary>�I�u�W�F�N�g�v�[��</summary>
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
    /// �G�Ƀ_���[�W��^����B
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
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
    /// �G�̎��S�����B
    /// </summary>
    private void Death()
    {
        //�����Ōo���l���h���b�v���邩���肷��B
        if (Random.Range(0, 101) < _enemyData.ExpProbability && _enemyData.ExpSize != 0)
        {
            Instantiate(ExpGenerator.Instance.ExpPrefabs[(int)_enemyData.ExpSize], transform.position, Quaternion.identity);
        }
        //�L���������Z�B
        PlayerBehaviour.PlayerKillCount++;

        //�I�u�W�F�N�g���v�[���ɕԋp
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
