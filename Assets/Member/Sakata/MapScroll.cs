using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour
{
    // �v���C���[��Transform
    public Transform player;
    // �w�i�Ƃ��Ďg��Image (RawImage)
    public RawImage backgroundImage;
    // �X�N���[�����x
    public float parallaxEffectMultiplier = 0.5f;
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);

    private Vector2 startPos;
    private Vector2 backgroundSize;

    void Start()
    {
        startPos = backgroundImage.uvRect.position;
        backgroundSize = backgroundImage.rectTransform.rect.size;
    }

    void Update()
    {
        Vector2 offset = new Vector2(player.position.x * parallaxEffectMultiplier, player.position.y * parallaxEffectMultiplier);

        backgroundImage.uvRect = new Rect(startPos + offset, backgroundImage.uvRect.size);

        cameraTransform.position = player.position + cameraOffset;
    }
}
