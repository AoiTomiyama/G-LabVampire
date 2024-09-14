using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    private Rigidbody2D _rb;

    int _h = 0;
    int _v = 0;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        bool pressedUp = Input.GetButton("Up");
        bool pressedDown = Input.GetButton("Down");
        bool pressedLeft = Input.GetButton("Left");
        bool pressedRight = Input.GetButton("Right");

        _v = (pressedUp && !pressedDown) ? 1 : (!pressedUp && pressedDown) ? -1 : (pressedUp && pressedDown) ? _v : 0;
        _h = (pressedRight && !pressedLeft) ? 1 : (!pressedRight && pressedLeft) ? -1 : (pressedLeft && pressedRight) ? _h : 0;

        /*
         * 上下、左右どちらかが押されている場合、入力に応じてそのまま値を入れる。
         * 両方押されている場合、値を変更しない。
         * 無入力の場合、0を入れる。
         */

        _rb.velocity = _moveSpeed * new Vector2(_h, _v);
    }
}
