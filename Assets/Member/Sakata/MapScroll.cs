using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour
{
    // プレイヤーのTransform
    public Transform Player;
    // 背景として使うImage (RawImage)
    public RawImage BackGroundImage;
    // カメラのTransform
    public Transform CameraTransform;
    // カメラのオフセット
    public Vector3 CameraOffset = new Vector3(0, 0, -10);

    // スクロール速度（パララックス効果）
    [SerializeField] private float _scrollspeed = 0.5f;

    private Vector2 _startPos;
    private Vector2 _backgroundSize;
    private Vector3 _beforePlayerPosition;


    public float Num { get; private set; } = 1.0f;

    void Start()
    {
        // 背景画像のサイズを取得
        _startPos = BackGroundImage.uvRect.position;
        _backgroundSize = BackGroundImage.rectTransform.rect.size;
        _beforePlayerPosition = Player.position;
    }

    void Update()
    {
        // 1フレーム前と現在のプレイヤー座標の差を求める
        Vector2 diffPosition = Player.position - _beforePlayerPosition;

        // プレイヤーの移動に応じて背景をスクロールさせる
        //Vector2 offset = new Vector2(Player.position.x * scrollspeed, Player.position.y * scrollspeed);
        Vector2 offset = diffPosition * _scrollspeed;

        // 背景のUV座標を変更してスクロールさせる
        BackGroundImage.uvRect = new Rect(BackGroundImage.uvRect.position + offset, BackGroundImage.uvRect.size);

        // カメラがプレイヤーを追従する
        CameraTransform.position = Player.position + CameraOffset;

        // 現在のプレイヤー座標を保存する
        _beforePlayerPosition = Player.position;
    }
}
