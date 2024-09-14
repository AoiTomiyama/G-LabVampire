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
    [SerializeField, Header("�U���p�x")]
    private float _attackSpeed;
    [SerializeField, Header("�U���͈́i���a�j")]
    private float _attackRange;
    [SerializeField, Header("�ђʗ�"),Range(1, 10)]
    private int _piercing;

    private List<EnemyBehaviour> _damagedList = new();
    /// <summary>
    /// �U���͂�萔����������B
    /// </summary>
    /// <param name="attackPower">�����������</param>
    public void Upgrade(int attackPower)
    {
        _attackPower += attackPower;
    }
    /// <summary>
    /// �U���͂������ő�������B
    /// </summary>
    /// <param name="attackMultiplier">���������銄��</param>
    public void Upgrade(float attackMultiplier = 1f)
    {
        _attackPower = Mathf.FloorToInt(_attackPower * attackMultiplier);
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
