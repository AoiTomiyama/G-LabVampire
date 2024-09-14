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
            if (_attackType == AttackType.NearestEnemy)
            {
                var targetPos = FindObjectsOfType<EnemyBehaviour>().Where(enemy => Vector2.Distance(transform.position, enemy.transform.position) <= _reach)
                    .OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
                    .First().transform.position;
                var go = Instantiate(_weapon, transform.position, Quaternion.identity);
                go.transform.up = (targetPos - transform.position).normalized;
            }
            else if (_attackType == AttackType.RandomEnemy)
            {
                var list = FindObjectsOfType<EnemyBehaviour>().Where(enemy => Vector2.Distance(transform.position, enemy.transform.position) <= _reach).ToList();
                var targetPos = list[Random.Range(0, list.Count)].transform.position;
                var go = Instantiate(_weapon, transform.position, Quaternion.identity);
                go.transform.up = (targetPos - transform.position).normalized;
            }
            else if (_attackType == AttackType.PlayerDirection)
            {
                var go = Instantiate(_weapon, transform.position, Quaternion.identity);
                go.transform.up = transform.right;
            }
            _timer = 0;
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