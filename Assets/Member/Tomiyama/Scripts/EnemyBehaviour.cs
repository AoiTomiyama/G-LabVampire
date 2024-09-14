using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Header("“G‚ÌŽí—Þ")]
    private EnemyData _enemyData;

    private Transform _target;
    private Rigidbody2D _rb;
    private int _health;

    private void Start()
    {
        _health = _enemyData.MaxHealth;
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            _rb.velocity = (_target.position - transform.position).normalized * _enemyData.MoveSpeed;
        }
    }

    public void RemoveHealth(int attackPower)
    {
        if (_health + _enemyData.Armor - attackPower <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _health -= attackPower - _enemyData.Armor;
        }
    }
}
