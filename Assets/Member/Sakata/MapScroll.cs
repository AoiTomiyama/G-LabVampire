using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour
{
    // �v���C���[��Transform
    public Transform Player;
    // �w�i�Ƃ��Ďg��Image (RawImage)
    public RawImage BackGroundImage;
    // �J������Transform
    public Transform CameraTransform;
    // �J�����̃I�t�Z�b�g
    public Vector3 CameraOffset = new Vector3(0, 0, -10);

    // �X�N���[�����x�i�p�����b�N�X���ʁj
    [SerializeField] private float _scrollspeed = 0.5f;

    private Vector2 _startPos;
    private Vector2 _backgroundSize;
    private Vector3 _beforePlayerPosition;


    public float Num { get; private set; } = 1.0f;

    void Start()
    {
        // �w�i�摜�̃T�C�Y���擾
        _startPos = BackGroundImage.uvRect.position;
        _backgroundSize = BackGroundImage.rectTransform.rect.size;
        _beforePlayerPosition = Player.position;
    }

    void Update()
    {
        // 1�t���[���O�ƌ��݂̃v���C���[���W�̍������߂�
        Vector2 diffPosition = Player.position - _beforePlayerPosition;

        // �v���C���[�̈ړ��ɉ����Ĕw�i���X�N���[��������
        //Vector2 offset = new Vector2(Player.position.x * scrollspeed, Player.position.y * scrollspeed);
        Vector2 offset = diffPosition * _scrollspeed;

        // �w�i��UV���W��ύX���ăX�N���[��������
        BackGroundImage.uvRect = new Rect(BackGroundImage.uvRect.position + offset, BackGroundImage.uvRect.size);

        // �J�������v���C���[��Ǐ]����
        CameraTransform.position = Player.position + CameraOffset;

        // ���݂̃v���C���[���W��ۑ�����
        _beforePlayerPosition = Player.position;
    }
}
