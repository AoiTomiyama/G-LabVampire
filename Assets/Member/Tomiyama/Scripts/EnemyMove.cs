using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    private float _moveSpeed;

    private Transform _target;
    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        if (_target != null)
        {
            _rb.velocity = (_target.position - transform.position).normalized * _moveSpeed;
        }
    }
}
