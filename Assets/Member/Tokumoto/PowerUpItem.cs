using System;
using UnityEngine;

public class PowerUpItem : MonoBehaviour, ILevelUppable
{
    private int _itemLevel = 1;
    [SerializeField, Header("レベルアップ時のステータス上昇")]
    private ItemGainParameter[] _itemGainParameters;
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
    [SerializeField]
    private string _itemName;

    //TODO: プレイヤーと武器が参照できるようにプロパティをつける。
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
        [SerializeField, Header("攻撃力の上昇割合")]
        public float AttackAdd;
        [SerializeField, Header("攻撃頻度の上昇割合")]
        public float AttackSpeedAdd;
        [SerializeField, Header("攻撃範囲の上昇割合")]
        public float RangeAdd;
        [SerializeField, Header("個数の上昇")]
        public int CountAdd;
        [SerializeField, Header("防御力の上昇割合")]
        public float DecreaseAdd;
        [SerializeField, Header("移動速度の上昇割合")]
        public float SpeedAdd;
    }
    private void Start()
    {
        SendParametor();
    }
    /// <summary>
    /// アイテムをレベルアップさせる。
    /// </summary>
    public void LevelUp()
    {
        //TODO: 現在レベルに応じて、アイテムの攻撃力割合などを増加させる。
        //レベル１はデフォルト値、レベル２以降は_itemGainParametersから取得し、加算する。

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