using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����̊��N���X
/// ���ꎩ�͎̂g�p�����p�����ċ�̓I�ȋ@�\�𑫂��Ă������Ƃ�z��B
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [SerializeField, Header("�U����")]
    private int _attackPower;
    [SerializeField, Header("�_���[�W�̐U�ꕝ")]
    private int _damageRange;
    [SerializeField, Header("�ђʗ�"),Range(1, 10)]
    private int _piercing;
    [SerializeField, Header("����^�C�v")]
    private WeaponType _weaponType;
    [SerializeField, Header("�e��")]
    private float _bulletSpeed;

    private List<EnemyBehaviour> _damagedList = new();
    private void FixedUpdate()
    {
        switch (_weaponType)
        {
            case WeaponType.Shikigami:
                transform.position += transform.up * _bulletSpeed;
                break;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour) && _piercing > 0)
        {
            if (!_damagedList.Contains(enemyBehaviour))
            {
                enemyBehaviour.RemoveHealth(_attackPower + Random.Range(-_damageRange, _damageRange));
                _damagedList.Add(enemyBehaviour);
                _piercing--;
            }
            if (_piercing <= 0) Destroy(gameObject);
        }
    }
}
