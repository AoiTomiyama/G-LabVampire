using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponID _weaponID;
    [Header("Stats")] 
    [SerializeField] int _weaponLevel;
    [SerializeField] string _weaponName;
    [SerializeField] float Attack;
    [Tooltip("�p�b�V�u(0)���ʏ�(1)���̍U���^�C�v")]
    [SerializeField] bool AttackType;
    [SerializeField] Animation _anim;
    [SerializeField] float _range;
    [SerializeField] int _count;

    private void Update()
    {
        switch (_weaponID) 
        {
            case WeaponID.Thunder:
                
                break;
            case WeaponID.Sikigami:
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

    enum WeaponID { Thunder, Sikigami, JapaneseSword, Naginata, Barrier, Fuda}
}
