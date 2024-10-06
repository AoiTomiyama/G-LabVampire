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
    private float _bulletSize;
    [SerializeField, Header("攻撃タイプ")]
    private AttackType _attackType;
    [SerializeField, Header("武器タイプ")]
    private WeaponType _weaponType;
    [SerializeField, Header("レベルアップ時のステータス上昇")]
    private WeaponLevelUpStatus[] _weaponLevelUpStatusArray;

    private int _level = 1; //武器の現在レベル
    private float _timer;

    public WeaponType WeaponType { get => _weaponType; }
    public int DamageRange { get => _damageRange; }
    public float BulletSpeed { get => _bulletSpeed; }
    public int AttackPower { get => _attackPower; }
    public int Level { get => _level; }

    private void Update()
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

    private void Generate()
    {
        var playerPos = transform.position;
        var list = FindObjectsOfType<EnemyBehaviour>();
        int count = list.Count();
        for (int i = 0; i < _count; i++)
        {
            switch (_weaponType)
            {
                case WeaponType.Shikigami:
                    if (count > 0)
                    {
                        var targetPos = list.OrderBy(enemy => Vector2.Distance(playerPos, enemy.transform.position))
                            .ElementAt(count > i ? i : 0).transform.position;
                        var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                        go.GetComponent<WeaponBase>().WeaponGenerator = this;
                        go.transform.up = (targetPos - playerPos).normalized;
                    }
                    break;
                case WeaponType.Thunder:
                    if (list.Count() > 0)
                    {
                        var targetPos = list[Random.Range(0, list.Count())].transform.position;
                        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
                        go.GetComponent<WeaponBase>().WeaponGenerator = this;
                    }
                    break;
            }
            if (_attackType == AttackType.NearestEnemy)
            {
            }
            else if (_attackType == AttackType.RandomEnemy)
            {
            }
            else if (_attackType == AttackType.PlayerDirection)
            {
                var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                go.GetComponent<WeaponBase>().WeaponGenerator = this;
                go.transform.up = PlayerBehaviour._flipX ? Vector2.right : Vector2.left;
            }
        }
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