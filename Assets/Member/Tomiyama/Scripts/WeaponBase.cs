using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器の基底クラス
/// これ自体は使用せず継承して具体的な機能を足していくことを想定。
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("攻撃力")]
    private int _attackPower;
    [SerializeField, Header("ダメージの振れ幅")]
    private int _damageRange;
    [SerializeField, Header("貫通力"),Range(1, 10)]
    private int _piercing;
    [SerializeField, Header("武器タイプ")]
    private WeaponType _weaponType;
    [SerializeField, Header("弾速")]
    private float _bulletSpeed;

    private List<EnemyBehaviour> _damagedList = new();
    private void FixedUpdate()
    {
        switch (_weaponType)
        {
            case WeaponType.Shikigami:
                transform.position += transform.up * _bulletSpeed;
                break;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour) && _piercing > 0)
        {
            if (!_damagedList.Contains(enemyBehaviour))
            {
                enemyBehaviour.RemoveHealth(_attackPower + Random.Range(-_damageRange, _damageRange));
                _damagedList.Add(enemyBehaviour);
                _piercing--;
            }
            if (_piercing <= 0) Destroy(gameObject);
        }
    }
}
