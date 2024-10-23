using System;
using UnityEngine;

public class PowerUpItem : MonoBehaviour, ILevelUppable
{
    private int _itemLevel = 1;
    [SerializeField, Header("���x���A�b�v���̃X�e�[�^�X�㏸")]
    private ItemGainParameter[] _itemGainParameters;
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
    [SerializeField]
    private string _itemName;

    //TODO: �v���C���[�ƕ��킪�Q�Ƃł���悤�Ƀv���p�e�B������B
    public int ItemLevel { get => _itemLevel; set => _itemLevel = value; }
    public float CurrentAttackAdd { get => _currentAttackAdd; set => _currentAttackAdd = value; }
    public float CurrentAttackSpeedAdd { get => _currentAttackSpeedAdd; set => _currentAttackSpeedAdd = value; }
    public float CurrentRangeAdd { get => _currentRangeAdd; set => _currentRangeAdd = value; }
    public int CurrentCountAdd { get => _currentCountAdd; set => _currentCountAdd = value; }
    public float CurrentDecreaseAdd { get => _currentDecreaseAdd; set => _currentDecreaseAdd = value; }
    public float CurrentSpeedAdd { get => _currentSpeedAdd; set => _currentSpeedAdd = value; }
    public string ItemName { get => _itemName; set => _itemName = value; }

    [Serializable]
    private struct ItemGainParameter
    {
        [SerializeField, Header("�U���͂̏㏸����")]
        public float AttackAdd;
        [SerializeField, Header("�U���p�x�̏㏸����")]
        public float AttackSpeedAdd;
        [SerializeField, Header("�U���͈͂̏㏸����")]
        public float RangeAdd;
        [SerializeField, Header("���̏㏸")]
        public int CountAdd;
        [SerializeField, Header("�h��͂̏㏸����")]
        public float DecreaseAdd;
        [SerializeField, Header("�ړ����x�̏㏸����")]
        public float SpeedAdd;
    }
    private void Start()
    {
        SendParametor();
    }
    /// <summary>
    /// �A�C�e�������x���A�b�v������B
    /// </summary>
    public void LevelUp()
    {
        //TODO: ���݃��x���ɉ����āA�A�C�e���̍U���͊����Ȃǂ𑝉�������B
        //���x���P�̓f�t�H���g�l�A���x���Q�ȍ~��_itemGainParameters����擾���A���Z����B

        ItemLevel++;
        //if (ItemLevel < 4)
        //{
        //    _currentAttackAdd += _itemGainParameters[ItemLevel - 2].AttackAdd;
        //    _currentAttackSpeedAdd += _itemGainParameters[ItemLevel - 2].AttackSpeedAdd;
        //    _currentCountAdd += _itemGainParameters[ItemLevel - 2].CountAdd;
        //    _currentDecreaseAdd += _itemGainParameters[ItemLevel - 2].DecreaseAdd;
        //    _currentSpeedAdd += _itemGainParameters[ItemLevel - 2].SpeedAdd;
        //}
        SendParametor();
    }

    void SendParametor()
    {
        var ItemPowerManager = GameObject.FindAnyObjectByType<ItemPowerUpManager>();
        int index = ItemLevel - 1;
        ItemPowerManager.CurrentAttackAdd += _itemGainParameters[index].AttackAdd;
        ItemPowerManager.CurrentAttackSpeedAdd += _itemGainParameters[index].AttackSpeedAdd;
        ItemPowerManager.CurrentCountAdd += _itemGainParameters[index].CountAdd;
        ItemPowerManager.CurrentDecreaseAdd += _itemGainParameters[index].DecreaseAdd;
        ItemPowerManager.CurrentSpeedAdd += _itemGainParameters[index].SpeedAdd;
    }

}