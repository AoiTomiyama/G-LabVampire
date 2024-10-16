using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponID _weaponID;
    [Header("Stats")] 
    [SerializeField] int _weaponLevel;
    [SerializeField] float _attack;
    [Tooltip("攻撃頻度")]
    [SerializeField] float _attackSpeed;
    [Tooltip("パッシブ(0)か通常(1)かの攻撃タイプ")]
    [SerializeField] bool AttackType;
    [SerializeField] Animation _anim;
    [SerializeField] float _range;
    [SerializeField] int _count;
    [Header("AddStats")]
    [SerializeField] float _addAttack;
    [SerializeField] float _addAttackSpeed;

    float time;
    [Header("プレファブ")]
    [SerializeField] GameObject _summonObj;

    private void Update()
    {
        time += Time.deltaTime;
        switch (_weaponID) 
        {
            case WeaponID.Thunder:
                break;
            case WeaponID.Sikigami:
                Sikigami();
                break;
            case WeaponID.JapaneseSword:
                break;
            case WeaponID.Naginata:
                break;
            case WeaponID.Barrier:
                break;
            case WeaponID.Fuda:
                break;
        }           
    }

    public void Sikigami()
    {
        if(_attackSpeed < time)
        {
            GameObject targetEnemy = GameObject.FindGameObjectWithTag("Enemy");
            var playerPos = transform.position;
            var go = Instantiate(_summonObj, playerPos, Quaternion.identity);
            go.transform.up = (targetEnemy.transform.position - playerPos).normalized;
            time = 0;
        }
    }

    public void LevelUp()
    {
        _weaponLevel += 1;
        _attack += _addAttack;
        _attackSpeed += _addAttackSpeed;
    }

    enum WeaponID { Thunder, Sikigami, JapaneseSword, Naginata, Barrier, Fuda}
}
