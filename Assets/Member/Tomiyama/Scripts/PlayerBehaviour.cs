using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;
    [SerializeField, Header("�v���C���[�̗̑�")]
    private int _maxHealth = 100;
    [SerializeField, Header("�v���C���[�̖h��́i��{�I�ɂ�0�j")]
    private int _playerDefence = default;
    [SerializeField, Header("�v���C���[�̍U���́i��{�I�ɂ�0�j")]
    private int _playerAttack = default;
    [SerializeField, Header("�v���C���[�̍U�����x�i��{�I�ɂ�0�j")]
    private float _playerAttackSpeed = default;
    [SerializeField, Header("�v���C���[�̍U���͈́i��{�I�ɂ�0�j")]
    private float _playerAttackRange = default;
    [SerializeField, Header("�g�p���镐��")]
    private List<GameObject> _weapons;
    [SerializeField, Header("�������̃A�C�e��")]
    private List<GameObject> _items;
    [SerializeField, Header("���̃��x���ɐi�ނ̂ɕK�v��EXP")]
    private List<int> _requireExpToNextLevel;
    [SerializeField, Header("���x���A�b�v���ɑ���������X�e�[�^�X�l")]
    private List<LevelUpStatusUp> _levelUpStatusUps;
    [Space]

    [SerializeField, Header("�A�C�e������͈́i���a�j")]
    private float _itemGetRange = 5f;
    [SerializeField, Header("�A�C�e���������񂹂��")]
    private float _pullPower;
    [SerializeField, Header("�o���l�̃��C���[")]
    private LayerMask _layer;
    [Header("�o���l�i��A���A���j�擾���̑�����")]
    [SerializeField] private int _expLarge;
    [SerializeField] private int _expMedium;
    [SerializeField] private int _expSmall;

    [Header("�Q�[���I�[�o�[���̏���")]
    public UnityEvent OnGameOver;
    [Header("���x���A�b�v���̏���")]
    public UnityEvent OnLevelUp;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    /// <summary>���������̈ړ�</summary>
    private int _h = 0;
    /// <summary>���������̈ړ�</summary>
    private int _v = 0;

    /// <summary>���݂̗̑�</summary>
    private int _currentHP = default;
    /// <summary>���݂̊l���o���l��</summary>
    private int _currentExp = 0;
    /// <summary>���݂̃��x��</summary>
    private int _currentLevel = 1;

    public static bool _flipX;

    /// <summary>�v���C���[���g�̎��U����</summary>
    public int PlayerAttack { get => _playerAttack; set => _playerAttack = value; }
    /// <summary>�v���C���[���g�̎��U���p�x</summary>
    public float PlayerAttackSpeed { get => _playerAttackSpeed; set => _playerAttackSpeed = value; }
    /// <summary>�v���C���[���g�̎��U���͈�</summary>
    public float PlayerAttackRange { get => _playerAttackRange; set => _playerAttackRange = value; }
    /// <summary>�v���C���[���g�̎��h���</summary>
    public int PlayerDefence { get => _playerDefence; set => _playerDefence = value; }

    /// <summary>���݂̗̑́B�ǂݎ���p�B</summary>
    public int CurrentHP => _currentHP;
    /// <summary>���݂̊l���o���l�ʁB�ǂݎ���p�B</summary>
    public int CurrentExp => _currentExp;
    /// <summary>���݂̃��x���B�ǂݎ���p�B</summary>
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
    /// �v���C���[�̈ړ������B
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
         * �㉺�A���E�ǂ��炩��������Ă���ꍇ�A���͂ɉ����Ă��̂܂ܒl������B
         * ����������Ă���ꍇ�A�l��ύX���Ȃ��B
         * �����͂̏ꍇ�A0������B
         */

        _sr.flipX = _flipX = (_h != 0) ? _h == 1 : _sr.flipX;
        _rb.velocity = _moveSpeed * new Vector2(_h, _v);
    }
    /// <summary>
    /// �o���l�̉�������B
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
                    Debug.LogWarning("���x���A�b�v���̃X�e�[�^�X�㏸�ʂ����蓖�Ă��Ă��܂���");
                }

                OnLevelUp.Invoke();
                Debug.Log("���x���A�b�v�I");
            }
        }
        else
        {
            Debug.LogWarning("�ő僌�x���ɓ��B���܂����B");
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
