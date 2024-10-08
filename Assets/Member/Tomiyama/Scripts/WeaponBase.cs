using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 武器の基底クラス
/// これ自体は使用せず継承して具体的な機能を足していくことを想定。
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("敵を貫通するかどうか")]
    private bool _isPierceEnemy;
    /// <summary>プレイヤーのステータスを取得。</summary>
    private PlayerBehaviour _playerBehaviour;
    /// <summary>武器ジェネレータのパラメータを取得。</summary>
    private WeaponGenerator _weaponGenerator;
    public WeaponGenerator WeaponGenerator { set =>  _weaponGenerator = value; }
    private Dictionary<EnemyBehaviour, float> _damagedDic = new();

    private void Start()
    {
        _playerBehaviour = FindObjectOfType<PlayerBehaviour>();
    }

    private void FixedUpdate()
    {
        switch (_weaponGenerator.WeaponType)
        {
            case WeaponType.Shikigami:
                transform.position += transform.up * _weaponGenerator.BulletSpeed;
                break;
            case WeaponType.Shield:
                foreach (var key in _damagedDic.Keys.ToList())
                {
                    if (_damagedDic[key] >= _weaponGenerator.AttackInterval)
                    {
                        key.RemoveHealth(_playerBehaviour.PlayerAttack * _weaponGenerator.AttackPower + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange));
                        if (!key.gameObject.activeSelf)
                        {
                            _damagedDic.Remove(key);
                        }
                        else
                        {
                            _damagedDic[key] = 0;
                        }
                    }
                    else
                    {
                        _damagedDic[key] += Time.deltaTime;
                    }
                }
                break;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            if (!_damagedDic.ContainsKey(enemyBehaviour))
            {
                enemyBehaviour.RemoveHealth(_playerBehaviour.PlayerAttack * _weaponGenerator.AttackPower + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange));
                _damagedDic.Add(enemyBehaviour, 0f);

                if (!_isPierceEnemy) Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            _damagedDic.Remove(enemyBehaviour);
        }
    }
}
