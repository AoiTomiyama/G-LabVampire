using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����̃_���[�W����A�ړ����s���B
/// </summary>
public class WeaponBase : MonoBehaviour, IPausable
{
    [SerializeField, Header("�G���ђʂ��邩�ǂ���")]
    private bool _isPierceEnemy = false;
    [SerializeField, Header("�m�b�N�o�b�N�̈З�")]
    private float _knockback = 0f;
    /// <summary>�v���C���[�̃X�e�[�^�X���擾�B</summary>
    private PlayerBehaviour _playerBehaviour;
    /// <summary>����W�F�l���[�^�̃p�����[�^���擾�B</summary>
    private WeaponGenerator _weaponGenerator;
    public WeaponGenerator WeaponGenerator { set => _weaponGenerator = value; }
    /// <summary>�G�ɃR���C�_�[���p���Ă���̂ŁA���d�q�b�g�����Ȃ����߂̑΍�</summary>
    private readonly List<EnemyBehaviour> _damagedList = new();
    /// <summary>�|�[�Y�̏��</summary>
    private bool _isPaused = false;
    /// <summary>�A�C�e���ɂ�鋭����Ԃ��擾</summary>
    private ItemPowerUpManager _powerUpManager;

    // �Ε����˂̎����ɕK�v�ȃp�����[�^�Q
    /// <summary>�J�n�ʒu</summary>
    private Vector3 _start;
    /// <summary>�o�ߎ���</summary>
    private float _t = 0f;
    /// <summary>���ˊp�x</summary>
    private float _degree;
    public float Degree { set => _degree = value; }

    private void Start()
    {
        _start = transform.position;
        _playerBehaviour = FindAnyObjectByType<PlayerBehaviour>();
        _powerUpManager = FindAnyObjectByType<ItemPowerUpManager>();
        transform.localScale *= _weaponGenerator.BulletSize * _powerUpManager.CurrentRangeAdd;
    }

    private void FixedUpdate()
    {
        if (_isPaused) return;

        switch (_weaponGenerator.WeaponType)
        {
            case WeaponType.Shikigami:
                ShikigamiBehaviour();
                break;
            case WeaponType.Shield:
                ShieldBehaviour();
                break;
            case WeaponType.Fuda:
                FudaBehaviour();
                break;
            case WeaponType.Katana:
                KatanaBehaviour();
                break;
        }
    }
    /// <summary>
    /// �����Ε����˂�����B
    /// </summary>
    private void KatanaBehaviour()
    {
        // �n���̏d�͉����x��2�{
        const float GRAVITY = 9.81f * 2f;
        //�@���ˑ��x��v�Ƃ��Ď擾�B
        var v = _weaponGenerator.BulletSpeed;
        _t += Time.fixedDeltaTime;
        var vx = v * Mathf.Cos(Mathf.Deg2Rad * _degree) * _t;
        var vy = v * Mathf.Sin(Mathf.Deg2Rad * _degree) * _t - (1f / 2f) * GRAVITY * _t * _t;
        transform.position = new Vector2(_start.x + vx, _start.y + vy);
        if (transform.position.y < -300) Destroy(gameObject);
    }
    /// <summary>
    /// �D�̎��A��ɏ�������悤�ɒ����B
    /// </summary>
    private void FudaBehaviour()
    {
        transform.localRotation = Quaternion.Inverse(_weaponGenerator.transform.rotation);
    }
    /// <summary>
    /// ���E�̃_���[�W����B
    /// </summary>
    private void ShieldBehaviour()
    {
        int damage = (int)(_playerBehaviour.PlayerAttack + _weaponGenerator.AttackPower * _powerUpManager.CurrentAttackAdd + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange + 1));
        var hits = Physics2D.CircleCastAll(transform.position, _weaponGenerator.BulletSize, transform.forward)
                           .Select(hit => hit.collider.GetComponent<EnemyBehaviour>())
                           .Where(component => component != null);
        foreach (var enemy in hits)
        {
            if (_playerBehaviour == null) continue;
            if (enemy.InvincibleTime >= _weaponGenerator.AttackInterval * _powerUpManager.CurrentAttackSpeedAdd)
            {
                enemy.InvincibleTime = 0;
                enemy.RemoveHealth(damage);
                enemy.transform.position -= (_playerBehaviour.transform.position - enemy.transform.position).normalized * _knockback;
            }
            else
            {
                enemy.InvincibleTime += Time.deltaTime;
            }
        }
    }
    /// <summary>
    /// ���_�̒��i�ړ��B
    /// </summary>
    private void ShikigamiBehaviour()
    {
        transform.position += transform.up * _weaponGenerator.BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_playerBehaviour == null) return;
        if (_weaponGenerator.WeaponType != WeaponType.Shield && collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            if (_damagedList.Contains(enemyBehaviour)) return;

            int damage = (int)(_playerBehaviour.PlayerAttack + _weaponGenerator.AttackPower * _powerUpManager.CurrentAttackAdd + Random.Range(-_weaponGenerator.DamageRange, _weaponGenerator.DamageRange + 1));
            enemyBehaviour.RemoveHealth(damage);
            enemyBehaviour.transform.position -= (_playerBehaviour.transform.position - enemyBehaviour.transform.position).normalized * _knockback;
            _damagedList.Add(enemyBehaviour);

            if (!_isPierceEnemy) Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_playerBehaviour == null) return;
        if (_weaponGenerator.WeaponType != WeaponType.Shield && collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            if (_damagedList.Contains(enemyBehaviour))
            {
                _damagedList.Remove(enemyBehaviour);
            }
        }
    }
    public void Pause()
    {
        _isPaused = true;
        if (TryGetComponent<Animator>(out var anim))
        {
            anim.speed = 0;
        }
    }

    public void Resume()
    {
        _isPaused = false;
        if (TryGetComponent<Animator>(out var anim))
        {
            anim.speed = 1;
        }
    }
}
