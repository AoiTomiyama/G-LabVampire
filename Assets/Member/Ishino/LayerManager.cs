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
            // プレイヤーのY座標がオブジェクトのY座標より高いか低いかで描画順を変更
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
