using UnityEngine;

public class ItemPowerUpManager : MonoBehaviour
{
    [SerializeField, Header("���݂̍U���͏㏸����")]
    private float _currentAttackAdd;
    [SerializeField, Header("���݂̍U���p�x�㏸����")]
    private float _currentAttackSpeedAdd;
    [SerializeField, Header("���݂̍U���͈͏㏸����")]
    private float _currentRangeAdd;
    [SerializeField, Header("���݂̌��㏸")]
    private int _currentCountAdd;
    [SerializeField, Header("���݂̖h��͏㏸����")]
    private float _currentDecreaseAdd;
    [SerializeField, Header("���݂̈ړ����x�㏸����")]
    private float _currentSpeedAdd;

    //TODO: �v���C���[�ƕ��킪�Q�Ƃł���悤�Ƀv���p�e�B������B
    public float CurrentAttackAdd { get => _currentAttackAdd; set => _currentAttackAdd = value; }
    public float CurrentAttackSpeedAdd { get => _currentAttackSpeedAdd; set => _currentAttackSpeedAdd = value; }
    public float CurrentRangeAdd { get => _currentRangeAdd; set => _currentRangeAdd = value; }
    public int CurrentCountAdd { get => _currentCountAdd; set => _currentCountAdd = value; }
    public float CurrentDecreaseAdd { get => _currentDecreaseAdd; set => _currentDecreaseAdd = value; }
    public float CurrentSpeedAdd { get => _currentSpeedAdd; set => _currentSpeedAdd = value; }

}
