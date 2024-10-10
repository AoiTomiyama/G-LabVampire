using System.Collections;
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
        if (_weaponType == WeaponType.Shield || _weaponType == WeaponType.Fuda) GenerateWeapon();
    }
    private void Update()
    {
        if (_weaponType == WeaponType.Shield || _weaponType == WeaponType.Fuda) return;
        if (_timer < _attackInterval)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            GenerateWeapon();
            _timer = 0;
        }
    }
    private void FixedUpdate()
    {
        if (_weaponType == WeaponType.Fuda)
        {
            transform.Rotate(0, 0, _bulletSpeed);
        }
    }

    private void GenerateWeapon()
    {
        for (int i = 0; i < _count; i++)
        {
            switch (_weaponType)
            {
                case WeaponType.Shikigami:
                    GenerateShikigami(i);
                    break;
                case WeaponType.Thunder:
                    GenerateThunder();
                    break;
                case WeaponType.Shield:
                    GenerateShield();
                    break;
                case WeaponType.Fuda:
                    GenerateFuda(i);
                    break;
                case WeaponType.Naginata:
                    StartCoroutine(GenerateNaginata(i));
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
    private IEnumerator GenerateNaginata(int index)
    {
        yield return new WaitForSeconds(0.1f * index);
        var go = Instantiate(_weapon, transform.position + Vector3.up * index, Quaternion.identity);
        if (index % 2 == 0) go.transform.Rotate(0, 0, 180);
        go.GetComponentInChildren<WeaponBase>().WeaponGenerator = this;
    }
    private void GenerateFuda(int index)
    {
        const float RANGE = 3f;
        var angle = (360f/ _count) * index * Mathf.Deg2Rad;
        var pos = RANGE * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) + transform.position;
        var go = Instantiate(_weapon, pos, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    private void GenerateShield()
    {
        var go = Instantiate(_weapon, transform.position, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    private void GenerateThunder()
    {
        var detectedList = SearchEnemies();
        int detectedCount = detectedList.Count;
        if (detectedCount <= 0) return;

        //ランダムな敵の位置に雷を召喚
        var targetPos = detectedList[Random.Range(0, detectedCount)].transform.position;
        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    private void GenerateShikigami(int index)
    {
        var detectedList = SearchEnemies();
        int detectedCount = detectedList.Count;
        if (detectedCount <= 0) return;

        //プレイヤーから敵までの距離でソートし、index番目の敵がいたらその方向を向いて式神を召喚。
        var targetPos = detectedList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
                                    .ElementAt(detectedCount > index ? index : 0).transform.position;
        var go = Instantiate(_weapon, transform.position, Quaternion.identity);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
        go.transform.up = (targetPos - transform.position).normalized;
    }
    private static List<EnemyBehaviour> SearchEnemies()
    {
        return FindObjectsOfType<EnemyBehaviour>().Where(go =>
        {
            //カメラの範囲内のオブジェクトのみを対象。
            var vp = Camera.main.WorldToViewportPoint(go.transform.position);
            return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
        }).ToList();
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

            if (_weaponType != WeaponType.Fuda) return;

            foreach (var weapon in transform.GetComponentsInChildren<WeaponBase>())
            {
                Destroy(weapon.gameObject);
            }
            for (int i = 0; i < _count; i++)
            {
                GenerateFuda(i);
            }
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
public enum WeaponType
{
    Thunder,
    Shikigami,
    Fuda,
    Shield,
    Naginata,
    Katana,
}