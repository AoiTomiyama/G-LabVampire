using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            //検知した敵がボスエネミーの場合、破壊処理をスキップする。
            if (enemyBehaviour.HasBossFlag) return;

            enemyBehaviour.ReturnToPool();
        }
    }
}
