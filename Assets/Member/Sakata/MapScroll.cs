using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour
{
    // プレイヤーのTransform
    public Transform _Player;
    // 背景として使うImage (RawImage)
    public RawImage _BackGroundImage;
    // カメラのTransform
    public Transform _CameraTransform;
    // カメラのオフセット
    public Vector3 _CameraOffset = new Vector3(0, 0, -10);

    // 座標をスクロールするためのスケール
    [SerializeField] private Vector2 _scrollScale = new Vector2(0.1f, 0.1f);
    // スクロール速度
    [SerializeField] private float _scrollspeed = 0.5f;

    private Vector2 _startPos;
    private Vector2 _backgroundSize;
    private Vector3 _beforePlayerPosition;

    public float Num { get; private set; } = 1.0f;

    void Start()
    {
        // 背景画像のサイズを取得
        _startPos = _BackGroundImage.uvRect.position;
        _backgroundSize = _BackGroundImage.rectTransform.rect.size;
        _beforePlayerPosition = _Player.position;
    }

    void Update()
    {
        // 1フレーム前と現在のプレイヤー座標の差を求める
        Vector2 diffPosition = _Player.position - _beforePlayerPosition;

        // プレイヤーの移動に応じて背景をスクロールさせる
        Vector2 offset = diffPosition * _scrollScale;

        // 背景のUV座標を変更してスクロールさせる
        _BackGroundImage.uvRect = new Rect(_BackGroundImage.uvRect.position + offset, _BackGroundImage.uvRect.size);

        // カメラがプレイヤーを追従する
        _CameraTransform.position = _Player.position + _CameraOffset;

        // 現在のプレイヤー座標を保存する
        _beforePlayerPosition = _Player.position;
    }
}
