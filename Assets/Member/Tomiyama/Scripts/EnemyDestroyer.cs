using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            enemyBehaviour.ReturnToPool();
        }
    }
}
