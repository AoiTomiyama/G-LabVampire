using System.Collections.Generic;
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
    private float _bulletSize = 1;
    [SerializeField, Header("�U���^�C�v")]
    private AttackType _attackType;
    [SerializeField, Header("����^�C�v")]
    private WeaponType _weaponType;
    [SerializeField, Header("���x���A�b�v���̃X�e�[�^�X�㏸")]
    private WeaponLevelUpStatus[] _weaponLevelUpStatusArray;

    //����̌��݃��x��
    private int _level = 1;
    private float _timer;

    public WeaponType WeaponType { get => _weaponType; }
    public int DamageRange { get => _damageRange; }
    public float BulletSpeed { get => _bulletSpeed; }
    public int AttackPower { get => _attackPower; }
    public int Level { get => _level; }
    public float AttackInterval { get => _attackInterval; }
    public float BulletSize { get => _bulletSize; }

    private void Start()
    {
        if (_weaponType == WeaponType.Shield) Generate();
    }
    private void Update()
    {
        if (_weaponType != WeaponType.Shield)
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
    }

    private void Generate()
    {
        var detectedList = FindObjectsOfType<EnemyBehaviour>().Where(go =>
        {
            //�J�����͈͓̔��̃I�u�W�F�N�g�݂̂�ΏہB
            var vp = Camera.main.WorldToViewportPoint(go.transform.position);
            return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
        }).ToList();
        int detectedCount = detectedList.Count;
        for (int i = 0; i < _count; i++)
        {
            switch (_weaponType)
            {
                case WeaponType.Shikigami:
                    if (detectedCount > 0)
                    {
                        GenerateShikigami(detectedList, detectedCount, i);
                    }
                    break;
                case WeaponType.Thunder:
                    if (detectedCount > 0)
                    {
                        GenerateThunder(detectedList, detectedCount);
                    }
                    break;
                case WeaponType.Shield:
                    GenerateShield();
                    break;
            }
            //if (_attackType == AttackType.PlayerDirection)
            //{
            //    var go = Instantiate(_weapon, playerPos, Quaternion.identity);
            //    go.GetComponent<WeaponBase>().WeaponGenerator = this;
            //    go.transform.up = PlayerBehaviour._flipX ? Vector2.right : Vector2.left;
            //}
        }
    }
    private void GenerateShield()
    {
        var go = Instantiate(_weapon, transform.position, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    private void GenerateThunder(List<EnemyBehaviour> detectedList, int detectedCount)
    {
        var targetPos = detectedList[Random.Range(0, detectedCount)].transform.position;
        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }

    private void GenerateShikigami(List<EnemyBehaviour> detectedList, int detectedCount, int i)
    {
        var playerPos = transform.position;
        var targetPos = detectedList.OrderBy(enemy => Vector2.Distance(playerPos, enemy.transform.position))
                                    .ElementAt(detectedCount > i ? i : 0).transform.position;
        var go = Instantiate(_weapon, playerPos, Quaternion.identity);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
        go.transform.up = (targetPos - playerPos).normalized;
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