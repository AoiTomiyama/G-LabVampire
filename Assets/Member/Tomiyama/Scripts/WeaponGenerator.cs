using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 武器の生成を行うスクリプト。
/// 武器自体の判定は別に用意する。
/// </summary>
public class WeaponGenerator : MonoBehaviour
{
    [SerializeField, Header("攻撃頻度")]
    private float _attackInterval;
    [SerializeField, Header("発射個数")]
    private int _count;
    [SerializeField, Header("生成する武器")]
    private GameObject _weapon;
    [SerializeField, Header("攻撃力")]
    private int _attackPower;
    [SerializeField, Header("ダメージの振れ幅")]
    private int _damageRange;
    [SerializeField, Header("弾速")]
    private float _bulletSpeed;
    [SerializeField, Header("弾の大きさ")]
    private float _bulletSize = 1;
    [SerializeField, Header("攻撃タイプ")]
    private AttackType _attackType;
    [SerializeField, Header("武器タイプ")]
    private WeaponType _weaponType;
    [SerializeField, Header("レベルアップ時のステータス上昇")]
    private WeaponLevelUpStatus[] _weaponLevelUpStatusArray;

    //武器の現在レベル
    private int _level = 1;
    private float _timer;

    public WeaponType WeaponType { get => _weaponType; }
    public int DamageRange { get => _damageRange; }
    public float BulletSpeed { get => _bulletSpeed; }
    public int AttackPower { get => _attackPower; }
    public int Level { get => _level; }
    public float AttackInterval { get => _attackInterval; }
    public float BulletSize { get => _bulletSize; }

    private void Start()
    {
        if (_weaponType == WeaponType.Shield) Generate();
    }
    private void Update()
    {
        if (_weaponType != WeaponType.Shield)
        {
            if (_timer < _attackInterval)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                Generate();
                _timer = 0;
            }
        }
    }

    private void Generate()
    {
        var detectedList = FindObjectsOfType<EnemyBehaviour>().Where(go =>
        {
            //カメラの範囲内のオブジェクトのみを対象。
            var vp = Camera.main.WorldToViewportPoint(go.transform.position);
            return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
        }).ToList();
        int detectedCount = detectedList.Count;
        for (int i = 0; i < _count; i++)
        {
            switch (_weaponType)
            {
                case WeaponType.Shikigami:
                    if (detectedCount > 0)
                    {
                        GenerateShikigami(detectedList, detectedCount, i);
                    }
                    break;
                case WeaponType.Thunder:
                    if (detectedCount > 0)
                    {
                        GenerateThunder(detectedList, detectedCount);
                    }
                    break;
                case WeaponType.Shield:
                    GenerateShield();
                    break;
            }
            //if (_attackType == AttackType.PlayerDirection)
            //{
            //    var go = Instantiate(_weapon, playerPos, Quaternion.identity);
            //    go.GetComponent<WeaponBase>().WeaponGenerator = this;
            //    go.transform.up = PlayerBehaviour._flipX ? Vector2.right : Vector2.left;
            //}
        }
    }
    private void GenerateShield()
    {
        var go = Instantiate(_weapon, transform.position, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    private void GenerateThunder(List<EnemyBehaviour> detectedList, int detectedCount)
    {
        var targetPos = detectedList[Random.Range(0, detectedCount)].transform.position;
        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }

    private void GenerateShikigami(List<EnemyBehaviour> detectedList, int detectedCount, int i)
    {
        var playerPos = transform.position;
        var targetPos = detectedList.OrderBy(enemy => Vector2.Distance(playerPos, enemy.transform.position))
                                    .ElementAt(detectedCount > i ? i : 0).transform.position;
        var go = Instantiate(_weapon, playerPos, Quaternion.identity);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
        go.transform.up = (targetPos - playerPos).normalized;
    }

    public void LevelUp()
    {
        if (_level < 4)
        {
            _attackPower += _weaponLevelUpStatusArray[_level - 1].Damage;
            _count += _weaponLevelUpStatusArray[_level - 1].Count;
            _attackInterval -= _weaponLevelUpStatusArray[_level - 1].AttackSpeed;
            _bulletSize += _weaponLevelUpStatusArray[_level - 1].Scale;
            _level++;
        }
    }
    [System.Serializable]
    private struct WeaponLevelUpStatus
    {
        public int Damage;
        public int Count;
        public int AttackSpeed;
        public int Scale;
    }
}
enum AttackType
{
    NearestEnemy,
    RandomEnemy,
    PlayerDirection,
    Passive,
}
public enum WeaponType
{
    Thunder,
    Shikigami,
    Fuda,
    Shield,
    Naginata,
    Katana,
}