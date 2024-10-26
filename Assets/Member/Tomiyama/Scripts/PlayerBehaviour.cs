using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 自機の情報を管轄するクラス。
/// </summary>
public class PlayerBehaviour : MonoBehaviour, IPausable
{
    [Header("----------- ステータス関連 ------------")]
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("プレイヤーの体力")]
    private int _maxHealth = 100;
    [SerializeField, Header("プレイヤーの防御力（基本的には0）")]
    private int _playerDefense = default;
    [SerializeField, Header("プレイヤーの攻撃力（基本的には0）")]
    private int _playerAttack = default;
    [SerializeField, Header("プレイヤーの攻撃速度（基本的には0）")]
    private float _playerAttackSpeed = default;
    [SerializeField, Header("プレイヤーの攻撃範囲（基本的には0）")]
    private float _playerAttackRange = default;
    [SerializeField, Header("一秒間に回復するHPの量")]
    private int _playerRegenerationHP;
    [SerializeField, Header("復活回数")]
    private int _playerResurrectionCount;
    [SerializeField, Header("使用する武器")]
    private List<GameObject> _weapons;
    [SerializeField, Header("レベルアップに必要なEXPと増加させるステータス値")]
    private List<LevelUpStatusUp> _levelUpStatusUps;
    [Space]

    [Header("----------- 経験値の回収関連 ------------")]
    [SerializeField, Header("アイテム回収範囲（半径）")]
    private float _itemGetRange = 5f;
    [SerializeField, Header("アイテムを引き寄せる力")]
    private float _pullPower;
    [SerializeField, Header("経験値のレイヤー")]
    private LayerMask _layer;
    [Header("経験値（大、中、小）取得時の増加量")]
    [SerializeField] private int _expLarge;
    [SerializeField] private int _expMedium;
    [SerializeField] private int _expSmall;
    [Space]

    [Header("----------- 特定の条件時に実行する処理 ------------")]
    [Header("ゲームオーバー時の処理")]
    public UnityEvent OnGameOver;
    [Header("レベルアップ時の処理")]
    public UnityEvent OnLevelUp;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    /// <summary>水平方向の移動</summary>
    private int _h = 0;
    /// <summary>垂直方向の移動</summary>
    private int _v = 0;
    /// <summary>現在の体力</summary>
    private int _currentHP = default;
    /// <summary>現在の獲得経験値量</summary>
    private int _currentExp = 0;
    /// <summary>現在のレベル</summary>
    private int _currentLevel = 1;
    /// <summary>敵の撃破カウント</summary>
    private static int _playerKillCount;
    private static bool _flipX;

    /// <summary>プレイヤー自身の持つ攻撃力</summary>
    public int PlayerAttack { get => _playerAttack; set => _playerAttack = value; }
    /// <summary>プレイヤー自身の持つ攻撃頻度</summary>
    public float PlayerAttackSpeed { get => _playerAttackSpeed; set => _playerAttackSpeed = value; }
    /// <summary>プレイヤー自身の持つ攻撃範囲</summary>
    public float PlayerAttackRange { get => _playerAttackRange; set => _playerAttackRange = value; }
    /// <summary>プレイヤー自身の持つ防御力</summary>
    public int PlayerDefense { get => _playerDefense; set => _playerDefense = value; }
    /// <summary>プレイヤー時針の持つ自己再生量</summary>
    public int PlayerRegenerationHP { get => _playerRegenerationHP; set => _playerRegenerationHP = value; }
    /// <summary>プレイヤーの復活回数</summary>
    public int PlayerResurrectionCount { get => _playerResurrectionCount; set => _playerResurrectionCount = value; }
    /// <summary>プレイヤーが倒した敵の数</summary>
    public static int PlayerKillCount { get => _playerKillCount; set => _playerKillCount = value; }
    /// <summary>プレイヤーの向いている方向。読み取り専用。</summary>
    public static bool FlipX => _flipX;
    /// <summary>プレイヤーのパラメーターをUIに反映させる。</summary>
    public Action<float, UpdateParameterType> DisplayOnUI;
    /// <summary>ポーズの状態</summary>
    private bool _isPaused = false;
    /// <summary>アイテムによる強化状態を取得</summary>
    private ItemPowerUpManager _powerUpManager;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _powerUpManager = FindAnyObjectByType<ItemPowerUpManager>();
        _currentHP = _maxHealth;
        _playerKillCount = 0;
        StartCoroutine(Regeneration());
        foreach (var weapon in _weapons)
        {
            Instantiate(weapon, transform.position, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        Move();
        CollectExp();
        DisplayOnUI?.Invoke(_playerKillCount, UpdateParameterType.KillCount);
    }
    /// <summary>
    /// プレイヤーの体力自動再生。
    /// </summary>
    private IEnumerator Regeneration()
    {
        while (true)
        {
            var waitTime = 0f;
            while (waitTime < 1)
            {
                yield return new WaitWhile(() => _isPaused);
                waitTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Heal(_playerRegenerationHP);
        }
    }
    /// <summary>
    /// プレイヤーの移動処理。
    /// </summary>
    private void Move()
    {
        //ポーズ時は入力を受け付けない。
        if (_isPaused)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        bool pressedUp = Input.GetButton("Up");
        bool pressedDown = Input.GetButton("Down");
        bool pressedLeft = Input.GetButton("Left");
        bool pressedRight = Input.GetButton("Right");

        _v = (pressedUp && !pressedDown) ? 1 : (!pressedUp && pressedDown) ? -1 : (pressedUp && pressedDown) ? _v : 0;
        _h = (pressedRight && !pressedLeft) ? 1 : (!pressedRight && pressedLeft) ? -1 : (pressedLeft && pressedRight) ? _h : 0;

        /*
         * 上下、左右どちらかが押されている場合、入力に応じてそのまま値を入れる。
         * 両方押されている場合、値を変更しない。
         * 無入力の場合、0を入れる。
         */

        _sr.flipX = _flipX = (_h != 0) ? _h == 1 : _sr.flipX;
        _rb.velocity = _powerUpManager.CurrentSpeedAdd * _moveSpeed * new Vector2(_h, _v);
    }
    /// <summary>
    /// 経験値の回収処理。
    /// </summary>
    private void CollectExp()
    {
        //ポーズ時は経験値収集を行わない。
        if (_isPaused) return;

        var hits = Physics2D.CircleCastAll(transform.position, _itemGetRange, transform.up, 0, _layer);
        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;
            var dir = (transform.position - hit.transform.position).normalized;
            hit.transform.position += dir * _pullPower;

            var distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < 0.25f)
            {
                var tag = hit.collider.tag;
                switch (tag)
                {
                    case "SmallExp":
                        GainExperience(_expSmall);
                        break;
                    case "MediumExp":
                        GainExperience(_expMedium);
                        break;
                    case "LargeExp":
                        GainExperience(_expLarge);
                        break;
                }
                hit.collider.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 経験値を増加させる。
    /// </summary>
    /// <param name="value">増加させる量</param>
    private void GainExperience(int value)
    {
        if (_currentLevel - 1 < _levelUpStatusUps.Count)
        {
            _currentExp += value;
            if (_currentExp > _levelUpStatusUps[_currentLevel - 1].RequireExp)
            {
                _currentExp = 0;
                _moveSpeed += _levelUpStatusUps[_currentLevel - 1].Speed;
                _playerAttack += _levelUpStatusUps[_currentLevel - 1].Attack;
                _playerDefense += _levelUpStatusUps[_currentLevel - 1].Defense;
                _maxHealth += _levelUpStatusUps[_currentLevel - 1].MaxHP;
                if (_levelUpStatusUps[_currentLevel - 1].MaxHP > 0)
                {
                    Heal(_maxHealth);
                }

                _currentLevel++;
                OnLevelUp.Invoke();
                Debug.Log("レベルアップ！");
            }
            if (_currentLevel - 1 < _levelUpStatusUps.Count)
            {
                DisplayOnUI?.Invoke(1f * _currentExp / _levelUpStatusUps[_currentLevel - 1].RequireExp, UpdateParameterType.Experience);
            }
        }
    }
    /// <summary>
    /// プレイヤーの体力を回復させる。
    /// </summary>
    /// <param name="value">回復量</param>
    public void Heal(int value)
    {
        Debug.Log($"体力を{value}回復");
        _currentHP = Mathf.Min(value + _currentHP, _maxHealth);
        DisplayOnUI?.Invoke(1.0f * _currentHP / _maxHealth, UpdateParameterType.Health);
    }
    /// <summary>
    /// プレイヤーにダメージを与える。
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void RemoveHealth(int damage)
    {
        Debug.Log($"Player Take Damage: {damage}");
        if (_currentHP + _playerDefense * _powerUpManager.CurrentDecreaseAdd - damage <= 0)
        {
            if (_playerResurrectionCount > 0)
            {
                Debug.Log("Player Revived");
                _currentHP = _maxHealth / 2;
                _playerResurrectionCount--;
            }
            else
            {
                Death();
            }
        }
        else
        {
            _currentHP -= damage - _playerDefense;
            DisplayOnUI?.Invoke(1.0f * _currentHP / _maxHealth, UpdateParameterType.Health);
        }
    }
    /// <summary>
    /// プレイヤー死亡時の処理。
    /// </summary>
    private void Death()
    {
        if (DataManagerBetweenScenes.Instance != null)
        {
            DataManagerBetweenScenes.Instance.PlayerLevelOnEnd = _currentLevel;
            foreach (var core in transform.GetComponentsInChildren<WeaponGenerator>())
            {
                DataManagerBetweenScenes.Instance.WeaponsData[core.WeaponName] = core.Level;
            }
            foreach (var item in transform.GetComponentsInChildren<PowerUpItem>())
            {
                DataManagerBetweenScenes.Instance.ItemsData[item.ItemName] = item.ItemLevel;
            }
        }
        Debug.Log("Game Over");

        OnGameOver.Invoke();
    }
    /// <summary>
    /// 武器をプレイヤーのインベントリに追加して生成。
    /// </summary>
    /// <param name="weapon">追加する武器</param>
    public void AddWeapon(GameObject weapon)
    {
        Instantiate(weapon, transform.position, Quaternion.identity, transform);
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    [Serializable]
    private struct LevelUpStatusUp
    {
        public int RequireExp;
        public int MaxHP;
        public int Attack;
        public int Defense;
        public int Speed;
    }
}
