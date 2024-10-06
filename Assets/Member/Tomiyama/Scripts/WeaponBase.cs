using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器の基底クラス
/// これ自体は使用せず継承して具体的な機能を足していくことを想定。
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("貫通力"), Range(1, 20)]
    private int _piercing;
    /// <summary>プレイヤーのステータスを取得。</summary>
    private PlayerBehaviour _playerBehaviour;
    /// <summary>武器ジェネレータのパラメータを取得。</summary>
    private WeaponGenerator _weaponGenerator;
    public WeaponGenerator WeaponGenerator { set =>  _weaponGenerator = value; }

    private void Start()
    {
        _playerBehaviour = FindObjectOfType<PlayerBehaviour>();
    }

    private List<EnemyBehaviour> _damagedList = new();
    private void FixedUpdate()
    {
        switch (_weaponGenerator.WeaponType)
        {
            case WeaponType.Shikigami:
                transform.position += transform.up * _weaponGenerator.BulletSpeed;
                break;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour) && _piercing > 0)
        {
            if (!_damagedList.Contains(enemyBehaviour))
            {
                enemyBehaviour.RemoveHealth(_playerBehaviour.PlayerAttack * _weaponGenerator.AttackPower + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange));
                _damagedList.Add(enemyBehaviour);
                _piercing--;
            }
            if (_piercing <= 0) Destroy(gameObject);
        }
    }
}
