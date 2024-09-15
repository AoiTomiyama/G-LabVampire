using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Header("“G‚ÌŽí—Þ")]
    private EnemyData _enemyData;

    private Transform _target;
    private Rigidbody2D _rb;
    private int _health;
    private float _timer;
    private PlayerBehaviour _player;

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

    public void RemoveHealth(int damage)
    {
        if (_health + _enemyData.Armor - damage <= 0)
        {
            int r = Random.Range(0, 101);
            if (r < _enemyData.ExpProbability && _enemyData.ExpSize != 0)
            {
                Instantiate(ExpGenerator.Instance.ExpPrefabs[(int)_enemyData.ExpSize], transform.position, Quaternion.identity);
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
