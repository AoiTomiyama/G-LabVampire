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
    [SerializeField, Header("攻撃頻度")]
    private float _attackSpeed;
    [SerializeField, Header("攻撃範囲（半径）")]
    private float _attackRange;
    [SerializeField, Header("貫通力"),Range(1, 10)]
    private int _piercing;

    private List<EnemyBehaviour> _damagedList = new();
    /// <summary>
    /// 攻撃力を定数分増加する。
    /// </summary>
    /// <param name="attackPower">増加させる量</param>
    public void Upgrade(int attackPower)
    {
        _attackPower += attackPower;
    }
    /// <summary>
    /// 攻撃力を割合で増加する。
    /// </summary>
    /// <param name="attackMultiplier">増加させる割合</param>
    public void Upgrade(float attackMultiplier = 1f)
    {
        _attackPower = Mathf.FloorToInt(_attackPower * attackMultiplier);
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
