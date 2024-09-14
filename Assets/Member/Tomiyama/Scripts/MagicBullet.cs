using UnityEngine;

public class MagicBullet : WeaponBase
{
    [SerializeField, Header("’e‘¬")]
    private float _bulletSpeed;
    private void FixedUpdate()
    {
        transform.position += transform.up * _bulletSpeed;
    }
}
