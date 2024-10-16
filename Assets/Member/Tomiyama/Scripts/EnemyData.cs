using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Header("“G‚Ì–¼‘O")]
    private string _name;
    [SerializeField, Header("“G‚Ì‘Ì—Í")]
    private int _maxHealth;
    [SerializeField, Header("“G‚ÌUŒ‚—Í")]
    private int _damage;
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    private float _moveSpeed;
    [SerializeField, Header("UŒ‚•p“x")]
    private float _attackSpeed;
    [SerializeField, Header("ŒoŒ±’lƒhƒƒbƒvŠm—¦"), Range(0, 100)]
    private int _expProbability;
    [SerializeField, Header("ŒoŒ±’l‚ÌŽí—Þ")]
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
