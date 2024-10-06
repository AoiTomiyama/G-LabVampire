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
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;
    [SerializeField, Header("�U���p�x")]
    private float _attackSpeed;
    [SerializeField, Header("�o���l�h���b�v�m��"), Range(0, 100)]
    private int _expProbability;
    [SerializeField, Header("�o���l�̎��")]
    private ExperienceType _expSize;

    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float AttackSpeed => _attackSpeed;
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
