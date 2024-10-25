using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 武器のダメージ判定、移動を行う。
/// </summary>
public class WeaponBase : MonoBehaviour, IPausable
{
    [SerializeField, Header("敵を貫通するかどうか")]
    private bool _isPierceEnemy = false;
    [SerializeField, Header("ノックバックの威力")]
    private float _knockback = 0f;
    /// <summary>プレイヤーのステータスを取得。</summary>
    private PlayerBehaviour _playerBehaviour;
    /// <summary>武器ジェネレータのパラメータを取得。</summary>
    private WeaponGenerator _weaponGenerator;
    public WeaponGenerator WeaponGenerator { set => _weaponGenerator = value; }
    /// <summary>敵にコライダーを二つ用いているので、多重ヒットさせないための対策</summary>
    private readonly List<EnemyBehaviour> _damagedList = new();
    /// <summary>ポーズの状態</summary>
    private bool _isPaused = false;
    /// <summary>アイテムによる強化状態を取得</summary>
    private ItemPowerUpManager _powerUpManager;

    // 斜方投射の実装に必要なパラメータ群
    /// <summary>開始位置</summary>
    private Vector3 _start;
    /// <summary>経過時間</summary>
    private float _t = 0f;
    /// <summary>発射角度</summary>
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
    /// 刀を斜方投射させる。
    /// </summary>
    private void KatanaBehaviour()
    {
        // 地球の重力加速度の2倍
        const float GRAVITY = 9.81f * 2f;
        //　発射速度をvとして取得。
        var v = _weaponGenerator.BulletSpeed;
        _t += Time.fixedDeltaTime;
        var vx = v * Mathf.Cos(Mathf.Deg2Rad * _degree) * _t;
        var vy = v * Mathf.Sin(Mathf.Deg2Rad * _degree) * _t - (1f / 2f) * GRAVITY * _t * _t;
        transform.position = new Vector2(_start.x + vx, _start.y + vy);
        if (transform.position.y < -300) Destroy(gameObject);
    }
    /// <summary>
    /// 札の時、常に上を向くように調整。
    /// </summary>
    private void FudaBehaviour()
    {
        transform.localRotation = Quaternion.Inverse(_weaponGenerator.transform.rotation);
    }
    /// <summary>
    /// 結界のダメージ判定。
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
    /// 式神の直進移動。
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
