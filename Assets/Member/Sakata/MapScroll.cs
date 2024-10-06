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
    // �X�N���[�����x�i�p�����b�N�X���ʁj
    public float parallaxEffectMultiplier = 0.5f;
    // �J������Transform
    public Transform cameraTransform;
    // �J�����̃I�t�Z�b�g
    public Vector3 cameraOffset = new Vector3(0, 0, -10);

    private Vector2 startPos;
    private Vector2 backgroundSize;

    void Start()
    {
        // �w�i�摜�̃T�C�Y���擾
        startPos = backgroundImage.uvRect.position;
        backgroundSize = backgroundImage.rectTransform.rect.size;
    }

    void Update()
    {
        // �v���C���[�̈ړ��ɉ����Ĕw�i���X�N���[��������
        Vector2 offset = new Vector2(player.position.x * parallaxEffectMultiplier, player.position.y * parallaxEffectMultiplier);

        // �w�i��UV���W��ύX���ăX�N���[��������
        backgroundImage.uvRect = new Rect(startPos + offset, backgroundImage.uvRect.size);

        // �J�������v���C���[��Ǐ]����
        cameraTransform.position = player.position + cameraOffset;
    }
}
