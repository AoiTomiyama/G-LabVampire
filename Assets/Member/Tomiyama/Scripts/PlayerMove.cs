using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
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
         * �㉺�A���E�ǂ��炩��������Ă���ꍇ�A���͂ɉ����Ă��̂܂ܒl������B
         * ����������Ă���ꍇ�A�l��ύX���Ȃ��B
         * �����͂̏ꍇ�A0������B
         */

        _rb.velocity = _moveSpeed * new Vector2(_h, _v);
    }
}
