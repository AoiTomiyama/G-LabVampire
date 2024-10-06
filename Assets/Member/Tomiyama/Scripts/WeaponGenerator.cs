using System.Linq;
using UnityEngine;

/// <summary>
/// 武器の生成を行うスクリプト。
/// 武器自体の判定は別に用意する。
/// </summary>
public class WeaponGenerator : MonoBehaviour
{
    [SerializeField, Header("攻撃頻度")]
    private float _attackInterval;
    [SerializeField, Header("発射個数")]
    private int _count;
    [SerializeField, Header("生成する武器")]
    private GameObject _weapon;
    [SerializeField, Header("攻撃タイプ")]
    private AttackType _attackType;
    [SerializeField, Header("武器タイプ")]
    private WeaponType _weaponType;

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
                var list = FindObjectsOfType<EnemyBehaviour>();
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
                var list = FindObjectsOfType<EnemyBehaviour>();
                if (list.Count() > 0)
                {
                    var targetPos = list[Random.Range(0, list.Count())].transform.position;
                    var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                    go.transform.up = (targetPos - playerPos).normalized;
                }
            }
            else if (_attackType == AttackType.PlayerDirection)
            {
                var go = Instantiate(_weapon, playerPos, Quaternion.identity);
                go.transform.up = PlayerBehaviour._flipX ? Vector2.right : Vector2.left;
            }
        }
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