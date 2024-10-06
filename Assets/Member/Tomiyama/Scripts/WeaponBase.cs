using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����̊��N���X
/// ���ꎩ�͎̂g�p�����p�����ċ�̓I�ȋ@�\�𑫂��Ă������Ƃ�z��B
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("�ђʗ�"), Range(1, 20)]
    private int _piercing;
    /// <summary>�v���C���[�̃X�e�[�^�X���擾�B</summary>
    private PlayerBehaviour _playerBehaviour;
    /// <summary>����W�F�l���[�^�̃p�����[�^���擾�B</summary>
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
