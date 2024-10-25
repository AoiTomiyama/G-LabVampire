using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����̐������s���X�N���v�g�B
/// ���펩�̂̔���͕ʂɗp�ӂ���B
/// </summary>
public class WeaponGenerator : MonoBehaviour, IPausable, ILevelUppable
{
    [SerializeField, Header("����̖��O�i���U���g�\���p�j")]
    private string _weaponName;
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
    [SerializeField, Header("����^�C�v")]
    private WeaponType _weaponType;
    [SerializeField, Header("���x���A�b�v���̃X�e�[�^�X�㏸")]
    private WeaponLevelUpStatus[] _weaponLevelUpStatusArray;

    //����̌��݃��x��
    private int _level = 1;
    private float _timer;
    /// <summary>�|�[�Y�̏��</summary>
    private bool _isPaused = false;
    /// <summary>�A�C�e���ɂ�鋭����Ԃ��擾</summary>
    private ItemPowerUpManager _powerUpManager;

    public WeaponType WeaponType => _weaponType;
    public int DamageRange => _damageRange;
    public float BulletSpeed => _bulletSpeed;
    public int AttackPower => _attackPower;
    public int Level => _level;
    public float AttackInterval => _attackInterval;
    public float BulletSize => _bulletSize;

    public string WeaponName => _weaponName;

    private void Start()
    {
        _powerUpManager = FindAnyObjectByType<ItemPowerUpManager>();
        if (_weaponType == WeaponType.Shield || _weaponType == WeaponType.Fuda)
        {
            GenerateWeapon();
        }
    }
    private void Update()
    {
        if (_weaponType == WeaponType.Shield || _weaponType == WeaponType.Fuda || _isPaused)
        {
            return;
        }

        if (_timer < _attackInterval * _powerUpManager.CurrentAttackSpeedAdd)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            GenerateWeapon();
            _timer = 0;
        }
    }
    private void FixedUpdate()
    {
        if (_weaponType == WeaponType.Fuda && !_isPaused)
        {
            transform.Rotate(0, 0, _bulletSpeed);
        }
    }
    /// <summary>
    /// �w�肳�ꂽ����𐶐�����B
    /// </summary>
    private void GenerateWeapon()
    {
        for (int i = 0; i < _count + _powerUpManager.CurrentCountAdd; i++)
        {
            switch (_weaponType)
            {
                case WeaponType.Shikigami:
                    GenerateShikigami(i);
                    break;
                case WeaponType.Thunder:
                    GenerateThunder();
                    break;
                case WeaponType.Shield:
                    GenerateShield();
                    break;
                case WeaponType.Fuda:
                    GenerateFuda(i);
                    break;
                case WeaponType.Naginata:
                    StartCoroutine(GenerateNaginata(i));
                    break;
                case WeaponType.Katana:
                    StartCoroutine(GenerateKatana(i));
                    break;
            }
        }
    }
    /// <summary>
    /// ���{���𐶐��B
    /// </summary>
    private IEnumerator GenerateKatana(int index)
    {
        bool playerFlipX = PlayerBehaviour.FlipX;
        const float DELAY = 0.2f;
        var waitTime = 0f;
        while (waitTime < DELAY * index)
        {
            yield return new WaitWhile(() => _isPaused);
            waitTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        var weapon = Instantiate(_weapon, transform.position, Quaternion.identity).GetComponent<WeaponBase>();
        AudioManager.Instance.PlaySE(AudioManager.SE.Katana);
        weapon.Degree = 90 + (playerFlipX ? -10 : 10) * index;
        weapon.WeaponGenerator = this;
    }
    /// <summary>
    /// �㓁�̎a�������B
    /// </summary>
    private IEnumerator GenerateNaginata(int index)
    {
        bool playerFlipX = PlayerBehaviour.FlipX;
        const float DELAY = 0.1f;
        var waitTime = 0f;
        while (waitTime < DELAY * index)
        {
            yield return new WaitWhile(() => _isPaused);
            waitTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        var go = Instantiate(_weapon, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySE(AudioManager.SE.Naginata);

        // ��܂ō��E�A�O�E�l�ŏ㉺�A����ȏ�Ŏ΂߂̂悤�ɁA�C���f�b�N�X�ɉ����ĕ������ω�������B
        // �C���f�b�N�X����̏ꍇ�i1, 3, 5, 7�jZ����180�x��]������B
        // { 0, 180, 0, 180, 0, 180, 0, 180 }
        if (index % 2 == 1) go.transform.Rotate(Vector3.forward * 180);

        // �C���f�b�N�X��2�ȏ�6�����̏ꍇ�AZ����90�x��]������B
        // { 0, 180, 90, 270, 90, 270, 0, 180 }
        if (index >= 2 && index < 6) go.transform.Rotate(Vector3.forward * 90);

        // �C���f�b�N�X��4�ȏ�̏ꍇ�AZ����45�x��]������B
        // { 0, 180, 90, 270, 135, 315, 45, 225 }
        if (index >= 4) go.transform.Rotate(Vector3.forward * 45);

        // �v���C���[�̌����ɉ�����Y���ŉ�]�����邱�ƂŔ��]������B
        if (!playerFlipX) go.transform.Rotate(Vector3.up * 180);

        go.GetComponentInChildren<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// �D�����͂ɐ����B
    /// </summary>
    private void GenerateFuda(int index)
    {
        const float RANGE = 3f;
        var angle = (360f / _count) * index * Mathf.Deg2Rad;
        var pos = RANGE * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) + transform.position;
        var go = Instantiate(_weapon, pos, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// ���E�𐶐��B
    /// </summary>
    private void GenerateShield()
    {
        var go = Instantiate(_weapon, transform.position, Quaternion.identity, transform);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// ���𐶐��B
    /// </summary>
    private void GenerateThunder()
    {
        var detectedList = SearchEnemies();
        int detectedCount = detectedList.Count;
        if (detectedCount <= 0) return;

        //�����_���ȓG�̈ʒu�ɗ�������
        var targetPos = detectedList[Random.Range(0, detectedCount)].transform.position;

        var go = Instantiate(_weapon, targetPos, Quaternion.identity);
        AudioManager.Instance.PlaySE(AudioManager.SE.Thunder);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
    }
    /// <summary>
    /// �����g�𐶐��B
    /// </summary>
    private void GenerateShikigami(int index)
    {
        var detectedList = SearchEnemies();
        int detectedCount = detectedList.Count;
        if (detectedCount <= 0) return;

        //�v���C���[����G�܂ł̋����Ń\�[�g���Aindex�Ԗڂ̓G�������炻�̕����������Ď��_�������B
        var targetPos = detectedList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
                                    .ElementAt(detectedCount > index ? index : 0).transform.position;
        var go = Instantiate(_weapon, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySE(AudioManager.SE.Shikigami);
        go.GetComponent<WeaponBase>().WeaponGenerator = this;
        go.transform.up = (targetPos - transform.position).normalized;
    }
    private static List<EnemyBehaviour> SearchEnemies()
    {
        return FindObjectsOfType<EnemyBehaviour>().Where(go =>
        {
            //�J�����͈͓̔��̃I�u�W�F�N�g�݂̂�ΏہB
            var vp = Camera.main.WorldToViewportPoint(go.transform.position);
            return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
        }).ToList();
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

            //�펞�����^�̕���̓��x���A�b�v���Ƀp�����[�^�[��K�p�����邽�߂ɍĐ������s���B
            if (_weaponType == WeaponType.Fuda || _weaponType == WeaponType.Shield)
            {
                foreach (var weapon in transform.GetComponentsInChildren<WeaponBase>())
                {
                    Destroy(weapon.gameObject);
                }
            }
            for (int i = 0; i < _count; i++)
            {
                if (_weaponType == WeaponType.Fuda)
                {
                    GenerateFuda(i);
                }
                else if (_weaponType == WeaponType.Shield)
                {
                    GenerateShield();
                }
            }
        }
    }
    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }

    [System.Serializable]
    private struct WeaponLevelUpStatus
    {
        public int Damage;
        public int Count;
        public float AttackSpeed;
        public float Scale;
    }
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