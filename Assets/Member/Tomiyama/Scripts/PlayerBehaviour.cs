using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("プレイヤーの体力")]
    private int _maxHealth = 100;
    [SerializeField, Header("プレイヤーの防御力（基本的には0）")]
    private int _playerDefence = default;
    [SerializeField, Header("プレイヤーの攻撃力（基本的には0）")]
    private int _playerAttack = default;
    [SerializeField, Header("プレイヤーの攻撃速度（基本的には0）")]
    private float _playerAttackSpeed = default;
    [SerializeField, Header("プレイヤーの攻撃範囲（基本的には0）")]
    private float _playerAttackRange = default;
    [SerializeField, Header("使用する武器")]
    private List<GameObject> _weapons;
    [SerializeField, Header("所持中のアイテム")]
    private List<GameObject> _items;
    [SerializeField, Header("次のレベルに進むのに必要なEXP")]
    private List<int> _requireExpToNextLevel;
    [SerializeField, Header("レベルアップ時に増加させるステータス値")]
    private List<LevelUpStatusUp> _levelUpStatusUps;
    [Space]

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

    public static bool _flipX;

    /// <summary>プレイヤー自身の持つ攻撃力</summary>
    public int PlayerAttack { get => _playerAttack; set => _playerAttack = value; }
    /// <summary>プレイヤー自身の持つ攻撃頻度</summary>
    public float PlayerAttackSpeed { get => _playerAttackSpeed; set => _playerAttackSpeed = value; }
    /// <summary>プレイヤー自身の持つ攻撃範囲</summary>
    public float PlayerAttackRange { get => _playerAttackRange; set => _playerAttackRange = value; }
    /// <summary>プレイヤー自身の持つ防御力</summary>
    public int PlayerDefence { get => _playerDefence; set => _playerDefence = value; }

    /// <summary>現在の体力。読み取り専用。</summary>
    public int CurrentHP => _currentHP;
    /// <summary>現在の獲得経験値量。読み取り専用。</summary>
    public int CurrentExp => _currentExp;
    /// <summary>現在のレベル。読み取り専用。</summary>
    public int CurrentLevel => _currentLevel;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _currentHP = _maxHealth;
        foreach (var weapon in _weapons)
        {
            Instantiate(weapon, transform.position, Quaternion.identity, transform);
        }
    }
    void Update()
    {
        Move();
        CollectExp();
    }
    /// <summary>
    /// プレイヤーの移動処理。
    /// </summary>
    private void Move()
    {
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
        _rb.velocity = _moveSpeed * new Vector2(_h, _v);
    }
    /// <summary>
    /// 経験値の回収処理。
    /// </summary>
    private void CollectExp()
    {
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
    private void GainExperience(int value)
    {
        if (_currentLevel - 1 < _requireExpToNextLevel.Count)
        {
            _currentExp += value;
            if (_currentExp > _requireExpToNextLevel[_currentLevel - 1])
            {
                _currentExp -= _requireExpToNextLevel[_currentLevel - 1];
                _currentLevel++;

                if (_currentLevel < _levelUpStatusUps.Count)
                {
                    _moveSpeed += _levelUpStatusUps[_currentLevel].Speed;
                    _playerAttack += _levelUpStatusUps[_currentLevel].Attack;
                    _playerDefence += _levelUpStatusUps[_currentLevel].Defense;
                    _maxHealth += _levelUpStatusUps[_currentLevel].MaxHP;
                }
                else
                {
                    Debug.LogWarning("レベルアップ時のステータス上昇量が割り当てられていません");
                }

                OnLevelUp.Invoke();
                Debug.Log("レベルアップ！");
            }
        }
        else
        {
            Debug.LogWarning("最大レベルに到達しました。");
        }
    }
    public void Heal(int value)
    {
        _currentHP = Mathf.Min(value + _currentHP, _maxHealth);
    }
    public void RemoveHealth(int damage)
    {
        Debug.Log($"Player Take Damage: {damage}");
        if (_currentHP + _playerDefence - damage <= 0)
        {
            Debug.Log("Game Over");
            OnGameOver.Invoke();
            Destroy(gameObject);
        }
        else
        {
            _currentHP -= damage - _playerDefence;
        }
    }
    [Serializable]
    private struct LevelUpStatusUp
    {
        public int MaxHP;
        public int Attack;
        public int Defense;
        public int Speed;
    }
}
