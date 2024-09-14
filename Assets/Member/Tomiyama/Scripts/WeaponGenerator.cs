using System.CodeDom.Compiler;
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
    [SerializeField, Header("�˒������i���a�j")]
    private float _reach;
    [SerializeField, Header("���ˌ�")]
    private int _count;
    [SerializeField, Header("�������镐��")]
    private GameObject _weapon;
    [SerializeField, Header("�U���^�C�v")]
    private AttackType _attackType;

    private float _timer;
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
        for (int i = 0; i < _count; i++)
        {
            if (_attackType == AttackType.NearestEnemy)
            {
                var list = FindObjectsOfType<EnemyBehaviour>().Where(enemy => Vector2.Distance(playerPos, enemy.transform.position) <= _reach);
                if (list.Count() > 0)
                {
                    var targetPos = list.OrderBy(enemy => Vector2.Distance(playerPos, enemy.transform.position))
                        .First().transform.position;
                    var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                    go.transform.up = (targetPos - playerPos).normalized;
                }
            }
            else if (_attackType == AttackType.RandomEnemy)
            {
                var list = FindObjectsOfType<EnemyBehaviour>().Where(enemy => Vector2.Distance(playerPos, enemy.transform.position) <= _reach).ToList();
                if (list.Count > 0)
                {
                    var targetPos = list[Random.Range(0, list.Count)].transform.position;
                    var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                    go.transform.up = (targetPos - playerPos).normalized;
                }
            }
            else if (_attackType == AttackType.PlayerDirection)
            {
                var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                go.transform.up = PlayerMove._flipX ? Vector2.right : Vector2.left;
            }
        }
    }
}
public enum AttackType
{
    NearestEnemy,
    RandomEnemy,
    PlayerDirection,
    Passive,
}