using UnityEngine;

public class LayerManager : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;

    void Start()
    {

        playerTransform = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(playerTransform != null)
        {
            float playerY = playerTransform.position.y;
            float objectY = transform.position.y;
            // �v���C���[��Y���W���I�u�W�F�N�g��Y���W��荂�����Ⴂ���ŕ`�揇��ύX
            if (playerY > objectY)
            {
                spriteRenderer.sortingOrder = 1;
            }
            else
            {
                spriteRenderer.sortingOrder = -1;
            }
        }
    }
        
}
