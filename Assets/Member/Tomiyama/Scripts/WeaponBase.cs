using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����̊��N���X
/// ���ꎩ�͎̂g�p�����p�����ċ�̓I�ȋ@�\�𑫂��Ă������Ƃ�z��B
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("�G���ђʂ��邩�ǂ���")]
    private bool _isPierceEnemy;
    /// <summary>�v���C���[�̃X�e�[�^�X���擾�B</summary>
    private PlayerBehaviour _playerBehaviour;
    /// <summary>����W�F�l���[�^�̃p�����[�^���擾�B</summary>
    private WeaponGenerator _weaponGenerator;
    public WeaponGenerator WeaponGenerator { set =>  _weaponGenerator = value; }
    /// <summary>�G�ɃR���C�_�[���p���Ă���̂ŁA���d�q�b�g�����Ȃ����߂̑΍�</summary>
    private List<EnemyBehaviour> _damagedList = new();

    private void Start()
    {
        _playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        transform.localScale *= _weaponGenerator.BulletSize;
    }

    private void FixedUpdate()
    {
        switch (_weaponGenerator.WeaponType)
        {
            case WeaponType.Shikigami:
                ShikigamiBehaviour();
                break;
            case WeaponType.Shield:
                ShieldBehaviour();
                break;
        }
    }

    private void ShieldBehaviour()
    {
        int damage = Mathf.RoundToInt(_playerBehaviour.PlayerAttack * _weaponGenerator.AttackPower + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange));
        var hits = Physics2D.CircleCastAll(transform.position, _weaponGenerator.BulletSize, transform.forward)
                           .Select(hit => hit.collider.GetComponent<EnemyBehaviour>())
                           .Where(eb => eb != null);
        foreach (var enemy in hits)
        {
            if (enemy.InvincibleTime >= _weaponGenerator.AttackInterval)
            {
                enemy.InvincibleTime = 0;
                enemy.RemoveHealth(damage);
            }
            else
            {
                enemy.InvincibleTime += Time.deltaTime;
            }
        }
    }

    private void ShikigamiBehaviour()
    {
        transform.position += transform.up * _weaponGenerator.BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_weaponGenerator.WeaponType != WeaponType.Shield && collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            if (!_damagedList.Contains(enemyBehaviour))
            {
                int damage = Mathf.RoundToInt(_playerBehaviour.PlayerAttack * _weaponGenerator.AttackPower + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange));
                enemyBehaviour.RemoveHealth(damage);
                _damagedList.Add(enemyBehaviour);
                if (!_isPierceEnemy) Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_weaponGenerator.WeaponType != WeaponType.Shield && collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            if (_damagedList.Contains(enemyBehaviour))
            {
                _damagedList.Remove(enemyBehaviour);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (_weaponGenerator.WeaponType == WeaponType.Shield)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _weaponGenerator.BulletSize);
        }
    }
}
