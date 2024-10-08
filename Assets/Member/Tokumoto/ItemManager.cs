using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemID _itemID;
    [SerializeField] int _itemLevel;
    [SerializeField] string _itemName;
    [SerializeField] float _hpGain;
    [Tooltip("消耗品(0)か所持品(1)")]
    [SerializeField] bool _itemType;
    [SerializeField] float _attackAdd;
    [SerializeField] float _attackSpeedAdd;
    [SerializeField] float _rangeAdd;
    [SerializeField] float _decreaseAdd;

    enum ItemID { Test1, Test2 }
}
