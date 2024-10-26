using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ���@�̏����Ǌ�����N���X�B
/// </summary>
public class PlayerBehaviour : MonoBehaviour, IPausable
{
    [Header("----------- �X�e�[�^�X�֘A ------------")]
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;
    [SerializeField, Header("�v���C���[�̗̑�")]
    private int _maxHealth = 100;
    [SerializeField, Header("�v���C���[�̖h��́i��{�I�ɂ�0�j")]
    private int _playerDefense = default;
    [SerializeField, Header("�v���C���[�̍U���́i��{�I�ɂ�0�j")]
    private int _playerAttack = default;
    [SerializeField, Header("�v���C���[�̍U�����x�i��{�I�ɂ�0�j")]
    private float _playerAttackSpeed = default;
    [SerializeField, Header("�v���C���[�̍U���͈́i��{�I�ɂ�0�j")]
    private float _playerAttackRange = default;
    [SerializeField, Header("��b�Ԃɉ񕜂���HP�̗�")]
    private int _playerRegenerationHP;
    [SerializeField, Header("������")]
    private int _playerResurrectionCount;
    [SerializeField, Header("�g�p���镐��")]
    private List<GameObject> _weapons;
    [SerializeField, Header("���x���A�b�v�ɕK�v��EXP�Ƒ���������X�e�[�^�X�l")]
    private List<LevelUpStatusUp> _levelUpStatusUps;
    [Space]

    [Header("----------- �o���l�̉���֘A ------------")]
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
    [Space]

    [Header("----------- ����̏������Ɏ��s���鏈�� ------------")]
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
    /// <summary>�G�̌��j�J�E���g</summary>
    private static int _playerKillCount;
    private static bool _flipX;

    /// <summary>�v���C���[���g�̎��U����</summary>
    public int PlayerAttack { get => _playerAttack; set => _playerAttack = value; }
    /// <summary>�v���C���[���g�̎��U���p�x</summary>
    public float PlayerAttackSpeed { get => _playerAttackSpeed; set => _playerAttackSpeed = value; }
    /// <summary>�v���C���[���g�̎��U���͈�</summary>
    public float PlayerAttackRange { get => _playerAttackRange; set => _playerAttackRange = value; }
    /// <summary>�v���C���[���g�̎��h���</summary>
    public int PlayerDefense { get => _playerDefense; set => _playerDefense = value; }
    /// <summary>�v���C���[���j�̎����ȍĐ���</summary>
    public int PlayerRegenerationHP { get => _playerRegenerationHP; set => _playerRegenerationHP = value; }
    /// <summary>�v���C���[�̕�����</summary>
    public int PlayerResurrectionCount { get => _playerResurrectionCount; set => _playerResurrectionCount = value; }
    /// <summary>�v���C���[���|�����G�̐�</summary>
    public static int PlayerKillCount { get => _playerKillCount; set => _playerKillCount = value; }
    /// <summary>�v���C���[�̌����Ă�������B�ǂݎ���p�B</summary>
    public static bool FlipX => _flipX;
    /// <summary>�v���C���[�̃p�����[�^�[��UI�ɔ��f������B</summary>
    public Action<float, UpdateParameterType> DisplayOnUI;
    /// <summary>�|�[�Y�̏��</summary>
    private bool _isPaused = false;
    /// <summary>�A�C�e���ɂ�鋭����Ԃ��擾</summary>
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
    /// �v���C���[�̗͎̑����Đ��B
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
    /// �v���C���[�̈ړ������B
    /// </summary>
    private void Move()
    {
        //�|�[�Y���͓��͂��󂯕t���Ȃ��B
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
         * �㉺�A���E�ǂ��炩��������Ă���ꍇ�A���͂ɉ����Ă��̂܂ܒl������B
         * ����������Ă���ꍇ�A�l��ύX���Ȃ��B
         * �����͂̏ꍇ�A0������B
         */

        _sr.flipX = _flipX = (_h != 0) ? _h == 1 : _sr.flipX;
        _rb.velocity = _powerUpManager.CurrentSpeedAdd * _moveSpeed * new Vector2(_h, _v);
    }
    /// <summary>
    /// �o���l�̉�������B
    /// </summary>
    private void CollectExp()
    {
        //�|�[�Y���͌o���l���W���s��Ȃ��B
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
    /// �o���l�𑝉�������B
    /// </summary>
    /// <param name="value">�����������</param>
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
                Debug.Log("���x���A�b�v�I");
            }
            if (_currentLevel - 1 < _levelUpStatusUps.Count)
            {
                DisplayOnUI?.Invoke(1f * _currentExp / _levelUpStatusUps[_currentLevel - 1].RequireExp, UpdateParameterType.Experience);
            }
        }
    }
    /// <summary>
    /// �v���C���[�̗̑͂��񕜂�����B
    /// </summary>
    /// <param name="value">�񕜗�</param>
    public void Heal(int value)
    {
        Debug.Log($"�̗͂�{value}��");
        _currentHP = Mathf.Min(value + _currentHP, _maxHealth);
        DisplayOnUI?.Invoke(1.0f * _currentHP / _maxHealth, UpdateParameterType.Health);
    }
    /// <summary>
    /// �v���C���[�Ƀ_���[�W��^����B
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
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
    /// �v���C���[���S���̏����B
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
    /// ������v���C���[�̃C���x���g���ɒǉ����Đ����B
    /// </summary>
    /// <param name="weapon">�ǉ����镐��</param>
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
