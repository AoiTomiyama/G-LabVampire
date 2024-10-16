using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            //���m�����G���{�X�G�l�~�[�̏ꍇ�A�j�󏈗����X�L�b�v����B
            if (enemyBehaviour.HasBossFlag) return;

            enemyBehaviour.ReturnToPool();
        }
    }
}
