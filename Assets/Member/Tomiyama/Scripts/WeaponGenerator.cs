using System.Linq;
using UnityEngine;

/// <summary>
/// ����̐������s���X�N���v�g�B
/// ���펩�̂̔���͕ʂɗp�ӂ���B
/// </summary>
public class WeaponGenerator : MonoBehaviour
{
    [SerializeField, Header("�U���p�x")]
    private float _attackInterval;
    [SerializeField, Header("���ˌ�")]
    private int _count;
    [SerializeField, Header("�������镐��")]
    private GameObject _weapon;
    [SerializeField, Header("�U����")]
    private int _attackPower;
    [SerializeField, Header("�_���[�W�̐U�ꕝ")]
    private int _damageRange;
    [SerializeField, Header("�e��")]
    private float _bulletSpeed;
    [SerializeField, Header("�e�̑傫��")]
    private float _bulletSize;
    [SerializeField, Header("�U���^�C�v")]
    private AttackType _attackType;
    [SerializeField, Header("����^�C�v")]
    private WeaponType _weaponType;
    [SerializeField, Header("���x���A�b�v���̃X�e�[�^�X�㏸")]
    private WeaponLevelUpStatus[] _weaponLevelUpStatusArray;

    private int _level = 1; //����̌��݃��x��
    private float _timer;

    public WeaponType WeaponType { get => _weaponType; }
    public int DamageRange { get => _damageRange; }
    public float BulletSpeed { get => _bulletSpeed; }
    public int AttackPower { get => _attackPower; }
    public int Level { get => _level; }

    private void Update()
    {
        if (_timer < _attackInterval)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            Generate();
            _timer = 0;
        }
    }

    private void Generate()
    {
        var playerPos = transform.position;
        var list = FindObjectsOfType<EnemyBehaviour>();
        int count = list.Count();
        for (int i = 0; i < _count; i++)
        {
            switch (_weaponType)
            {
                case WeaponType.Shikigami:
                    if (count > 0)
                    {
                        var targetPos = list.OrderBy(enemy => Vector2.Distance(playerPos, enemy.transform.position))
                            .ElementAt(count > i ? i : 0).transform.position;
                        var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                        go.GetComponent<WeaponBase>().WeaponGenerator = this;
                        go.transform.up = (targetPos - playerPos).normalized;
                    }
                    break;
                case WeaponType.Thunder:
                    if (list.Count() > 0)
                    {
                        var targetPos = list[Random.Range(0, list.Count())].transform.position;
                        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
                        go.GetComponent<WeaponBase>().WeaponGenerator = this;
                    }
                    break;
            }
            if (_attackType == AttackType.NearestEnemy)
            {
            }
            else if (_attackType == AttackType.RandomEnemy)
            {
            }
            else if (_attackType == AttackType.PlayerDirection)
            {
                var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                go.GetComponent<WeaponBase>().WeaponGenerator = this;
                go.transform.up = PlayerBehaviour._flipX ? Vector2.right : Vector2.left;
            }
        }
    }
    public void LevelUp()
    {
        if (_level < 4)
        {
            _attackPower += _weaponLevelUpStatusArray[_level - 1].Damage;
            _count += _weaponLevelUpStatusArray[_level - 1].Count;
            _attackInterval -= _weaponLevelUpStatusArray[_level - 1].AttackSpeed;
            _bulletSize += _weaponLevelUpStatusArray[_level - 1].Scale;
            _level++;
        }
    }
    [System.Serializable]
    private struct WeaponLevelUpStatus
    {
        public int Damage;
        public int Count;
        public int AttackSpeed;
        public int Scale;
    }
}
enum AttackType
{
    NearestEnemy,
    RandomEnemy,
    PlayerDirection,
    Passive,
}
public enum WeaponType
{
    Thunder,
    Shikigami,
    Fuda,
    Shield,
    Naginata,
    Katana,
}