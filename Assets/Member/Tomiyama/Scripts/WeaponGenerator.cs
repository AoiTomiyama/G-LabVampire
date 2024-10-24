using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 武器の生成を行うスクリプト。
/// 武器自体の判定は別に用意する。
/// </summary>
public class WeaponGenerator : MonoBehaviour, IPausable, ILevelUppable
{
    [SerializeField, Header("武器の名前（リザルト表示用）")]
    private string _weaponName;
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
    /// <summary>ポーズの状態</summary>
    private bool _isPaused = false;
    /// <summary>アイテムによる強化状態を取得</summary>
    private ItemPowerUpManager _powerUpManager;

    public WeaponType WeaponType => _weaponType;
    public int DamageRange => _damageRange;
    public float BulletSpeed => _bulletSpeed;
    public int AttackPower => _attackPower;
    public int Level => _level;
    public float AttackInterval => _attackInterval;
    public float BulletSize => _bulletSize;

    public string WeaponName => _weaponName;

    private void Start()
    {
        _powerUpManager = FindAnyObjectByType<ItemPowerUpManager>();
        if (_weaponType == WeaponType.Shield || _weaponType == WeaponType.Fuda)
        {
            GenerateWeapon();
        }
    }
    private void Update()
    {
        if (_weaponType == WeaponType.Shield || _weaponType == WeaponType.Fuda || _isPaused)
        {
            return;
        }

        if (_timer < _attackInterval * _powerUpManager.CurrentAttackSpeedAdd)
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
        if (_weaponType == WeaponType.Fuda && !_isPaused)
        {
            transform.Rotate(0, 0, _bulletSpeed);
        }
    }
    /// <summary>
    /// 指定された武器を生成する。
    /// </summary>
    private void GenerateWeapon()
    {
        for (int i = 0; i < _count + _powerUpManager.CurrentCountAdd; i++)
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
                case WeaponType.Katana:
                    StartCoroutine(GenerateKatana(i));
                    break;
            }
        }
    }
    /// <summary>
    /// 日本刀を生成。
    /// </summary>
    private IEnumerator GenerateKatana(int index)
    {
        bool playerFlipX = PlayerBehaviour.FlipX;
        const float DELAY = 0.2f;
        var waitTime = 0f;
        while (waitTime < DELAY * index)
        {
            yield return new WaitWhile(() => _isPaused);
            waitTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        var weapon = Instantiate(_weapon, transform.position, Quaternion.identity).GetComponent<WeaponBase>();
        AudioManager.Instance.PlaySE(AudioManager.SE.Katana);
        weapon.Degree = 90 + (playerFlipX ? -10 : 10) * index;
        weapon.WeaponGenerator = this;
    }
    /// <summary>
    /// 薙刀の斬撃生成。
    /// </summary>
    private IEnumerator GenerateNaginata(int index)
    {
        bool playerFlipX = PlayerBehaviour.FlipX;
        const float DELAY = 0.1f;
        var waitTime = 0f;
        while (waitTime < DELAY * index)
        {
            yield return new WaitWhile(() => _isPaused);
            waitTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        var go = Instantiate(_weapon, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySE(AudioManager.SE.Naginata);

        // 二個まで左右、三・四個で上下、それ以上で斜めのように、インデックスに応じて方向が変化させる。
        // インデックスが奇数の場合（1, 3, 5, 7）Z軸を180度回転させる。
        // { 0, 180, 0, 180, 0, 180, 0, 180 }
        if (index % 2 == 1) go.transform.Rotate(Vector3.forward * 180);

        // インデックスが2以上6未満の場合、Z軸を90度回転させる。
        // { 0, 180, 90, 270, 90, 270, 0, 180 }
        if (index >= 2 && index < 6) go.transform.Rotate(Vector3.forward * 90);

        // インデックスが4以上の場合、Z軸を45度回転させる。
        // { 0, 180, 90, 270, 135, 315, 45, 225 }
        if (index >= 4) go.transform.Rotate(Vector3.forward * 45);

        // プレイヤーの向きに応じてY軸で回転させることで反転させる。
        if (!playerFlipX) go.transform.Rotate(Vector3.up * 180);

        go.GetComponentInChildren<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// 札を周囲に生成。
    /// </summary>
    private void GenerateFuda(int index)
    {
        const float RANGE = 3f;
        var angle = (360f / _count) * index * Mathf.Deg2Rad;
        var pos = RANGE * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) + transform.position;
        var go = Instantiate(_weapon, pos, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// 結界を生成。
    /// </summary>
    private void GenerateShield()
    {
        var go = Instantiate(_weapon, transform.position, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// 雷を生成。
    /// </summary>
    private void GenerateThunder()
    {
        var detectedList = SearchEnemies();
        int detectedCount = detectedList.Count;
        if (detectedCount <= 0) return;

        //ランダムな敵の位置に雷を召喚
        var targetPos = detectedList[Random.Range(0, detectedCount)].transform.position;

        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
        AudioManager.Instance.PlaySE(AudioManager.SE.Thunder);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// 式が身を生成。
    /// </summary>
    private void GenerateShikigami(int index)
    {
        var detectedList = SearchEnemies();
        int detectedCount = detectedList.Count;
        if (detectedCount <= 0) return;

        //プレイヤーから敵までの距離でソートし、index番目の敵がいたらその方向を向いて式神を召喚。
        var targetPos = detectedList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
                                    .ElementAt(detectedCount > index ? index : 0).transform.position;
        var go = Instantiate(_weapon, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySE(AudioManager.SE.Shikigami);
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

            //常時発生型の武器はレベルアップ時にパラメーターを適用させるために再生成を行う。
            if (_weaponType == WeaponType.Fuda || _weaponType == WeaponType.Shield)
            {
                foreach (var weapon in transform.GetComponentsInChildren<WeaponBase>())
                {
                    Destroy(weapon.gameObject);
                }
            }
            for (int i = 0; i < _count; i++)
            {
                if (_weaponType == WeaponType.Fuda)
                {
                    GenerateFuda(i);
                }
                else if (_weaponType == WeaponType.Shield)
                {
                    GenerateShield();
                }
            }
        }
    }
    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    [System.Serializable]
    private struct WeaponLevelUpStatus
    {
        public int Damage;
        public int Count;
        public float AttackSpeed;
        public float Scale;
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