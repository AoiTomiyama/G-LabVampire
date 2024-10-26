using UnityEngine;

public class ItemPowerUpManager : MonoBehaviour
{
    [SerializeField, Header("現在の攻撃力上昇割合")]
    private float _currentAttackAdd;
    [SerializeField, Header("現在の攻撃頻度上昇割合")]
    private float _currentAttackSpeedAdd;
    [SerializeField, Header("現在の攻撃範囲上昇割合")]
    private float _currentRangeAdd;
    [SerializeField, Header("現在の個数上昇")]
    private int _currentCountAdd;
    [SerializeField, Header("現在の防御力上昇割合")]
    private float _currentDecreaseAdd;
    [SerializeField, Header("現在の移動速度上昇割合")]
    private float _currentSpeedAdd;

    //TODO: プレイヤーと武器が参照できるようにプロパティをつける。
    public float CurrentAttackAdd { get => _currentAttackAdd; set => _currentAttackAdd = value; }
    public float CurrentAttackSpeedAdd { get => _currentAttackSpeedAdd; set => _currentAttackSpeedAdd = value; }
    public float CurrentRangeAdd { get => _currentRangeAdd; set => _currentRangeAdd = value; }
    public int CurrentCountAdd { get => _currentCountAdd; set => _currentCountAdd = value; }
    public float CurrentDecreaseAdd { get => _currentDecreaseAdd; set => _currentDecreaseAdd = value; }
    public float CurrentSpeedAdd { get => _currentSpeedAdd; set => _currentSpeedAdd = value; }

}
