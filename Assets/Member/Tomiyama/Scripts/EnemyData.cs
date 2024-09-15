using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("�G�̖��O")]
    private string _name;
    [SerializeField, Header("�G�̗̑�")]
    private int _maxHealth;
    [SerializeField, Header("�G�̍U����")]
    private int _damage;
    [SerializeField, Header("�G�̖h��́i��{�I�ɂ�0�j")]
    private int _armor;
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;
    [SerializeField, Header("�U���p�x")]
    private float _attackSpeed;
    [SerializeField, Header("�U���͈�")]
    private float _range;
    [SerializeField, Header("�o���l�h���b�v�m��"), Range(0, 100)]
    private int _expProbability;
    [SerializeField, Header("�o���l�̎��")]
    private ExperienceType _expSize;

    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public int Armor => _armor;
    public float MoveSpeed => _moveSpeed;
    public float AttackSpeed => _attackSpeed;
    public float Range => _range;
    public int ExpProbability => _expProbability;
    public ExperienceType ExpSize => _expSize;
    public enum ExperienceType
    {
        None,
        Small,
        Medium,
        Large,
    }
}
