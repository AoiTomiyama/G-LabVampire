using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Header("“G‚ÌŽí—Þ")]
    private EnemyData _enemyData;

    private Transform _target;
    private Rigidbody2D _rb;
    private int _health;
    private float _timer;
    private bool _isAttacking;

    private void Start()
    {
        _health = _enemyData.MaxHealth;
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        if (_isAttacking)
        {
            if (_timer < _enemyData.AttackSpeed)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _isAttacking = false;
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

    public void RemoveHealth(int damage)
    {
        if (_health + _enemyData.Armor - damage <= 0)
        {
            int r = Random.Range(0, 101);
            if (r < _enemyData.ExpProbability && _enemyData.ExpSize != 0)
            {
                Instantiate(ExpGenerator.Instance.ExpPrefabs[_enemyData.ExpSize], transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else
        {
            _health -= damage - _enemyData.Armor;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour) && !_isAttacking)
        {
            playerBehaviour.RemoveHealth(_enemyData.Damage);
            _isAttacking = true;
        }
    }
}
