using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;
    [SerializeField, Header("�v���C���[�̗̑�")]
    private int _maxHealth = 100;
    [SerializeField, Header("�v���C���[�̖h���")]
    private int _decrease;
    [SerializeField, Header("���̃��x���ɐi�ނ̂ɕK�v��EXP")]
    private List<int> _requireExpToNextLevel;
    [SerializeField, Header("�g�p���镐��")]
    private GameObject[] _weapons;
    [SerializeField, Header("�������̃A�C�e��")]
    private List<GameObject> _items;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private int _health;
    private int _h = 0;
    private int _v = 0;
    public static bool _flipX;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _health = _maxHealth;
        foreach (var weapon in _weapons)
        {
            Instantiate(weapon, transform.position, Quaternion.identity, transform);
        }
    }
    void Update()
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
    public void Heal(int value)
    {
        _health = Mathf.Min(value + _health, _maxHealth);
    }
    public void RemoveHealth(int damage)
    {
        if (_health + _decrease - damage <= 0)
        {
            Debug.Log("Game Over");
            Destroy(gameObject);
        }
        else
        {
            _health -= damage - _decrease;
        }
    }
}
