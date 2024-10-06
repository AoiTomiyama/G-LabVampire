using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour
{
    // �v���C���[��Transform
    public Transform _Player;
    // �w�i�Ƃ��Ďg��Image (RawImage)
    public RawImage _BackGroundImage;
    // �J������Transform
    public Transform _CameraTransform;
    // �J�����̃I�t�Z�b�g
    public Vector3 _CameraOffset = new Vector3(0, 0, -10);

    // ���W���X�N���[�����邽�߂̃X�P�[��
    [SerializeField] private Vector2 _scrollScale = new Vector2(0.1f, 0.1f);
    // �X�N���[�����x
    [SerializeField] private float _scrollspeed = 0.5f;

    private Vector2 _startPos;
    private Vector2 _backgroundSize;
    private Vector3 _beforePlayerPosition;

    public float Num { get; private set; } = 1.0f;

    void Start()
    {
        // �w�i�摜�̃T�C�Y���擾
        _startPos = _BackGroundImage.uvRect.position;
        _backgroundSize = _BackGroundImage.rectTransform.rect.size;
        _beforePlayerPosition = _Player.position;
    }

    void Update()
    {
        // 1�t���[���O�ƌ��݂̃v���C���[���W�̍������߂�
        Vector2 diffPosition = _Player.position - _beforePlayerPosition;

        // �v���C���[�̈ړ��ɉ����Ĕw�i���X�N���[��������
        Vector2 offset = diffPosition * _scrollScale;

        // �w�i��UV���W��ύX���ăX�N���[��������
        _BackGroundImage.uvRect = new Rect(_BackGroundImage.uvRect.position + offset, _BackGroundImage.uvRect.size);

        // �J�������v���C���[��Ǐ]����
        _CameraTransform.position = _Player.position + _CameraOffset;

        // ���݂̃v���C���[���W��ۑ�����
        _beforePlayerPosition = _Player.position;
    }
}
