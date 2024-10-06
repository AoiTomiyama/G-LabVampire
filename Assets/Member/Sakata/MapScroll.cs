using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScroll : MonoBehaviour
{
    // プレイヤーのTransform
    public Transform player;
    // 背景として使うImage (RawImage)
    public RawImage backgroundImage;
    // スクロール速度（パララックス効果）
    public float parallaxEffectMultiplier = 0.5f;
    // カメラのTransform
    public Transform cameraTransform;
    // カメラのオフセット
    public Vector3 cameraOffset = new Vector3(0, 0, -10);

    private Vector2 startPos;
    private Vector2 backgroundSize;

    void Start()
    {
        // 背景画像のサイズを取得
        startPos = backgroundImage.uvRect.position;
        backgroundSize = backgroundImage.rectTransform.rect.size;
    }

    void Update()
    {
        // プレイヤーの移動に応じて背景をスクロールさせる
        Vector2 offset = new Vector2(player.position.x * parallaxEffectMultiplier, player.position.y * parallaxEffectMultiplier);

        // 背景のUV座標を変更してスクロールさせる
        backgroundImage.uvRect = new Rect(startPos + offset, backgroundImage.uvRect.size);

        // カメラがプレイヤーを追従する
        cameraTransform.position = player.position + cameraOffset;
    }
}
