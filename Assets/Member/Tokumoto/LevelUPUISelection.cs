using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUPUISelection : MonoBehaviour
{
    bool _isLevelUp;
    [SerializeField] GameObject _levelUpPanel;
    [SerializeField] GameObject _firstPanel;
    [SerializeField] private List<GameObject> _weaponButtons;
    [SerializeField] private List<GameObject> _itemButtons;
    private List<ButtonScript> _weaponList = new();
    private List<ButtonScript> _itemList = new();


    private void Start()
    {
        _levelUpPanel.SetActive(false);
        
        foreach (var button in _itemButtons)
        {
            var component = Instantiate(button, _levelUpPanel.transform).GetComponent<ButtonScript>();
            component.gameObject.SetActive(false);
            _itemList.Add(component);
        }
        foreach (var button in _weaponButtons)
        {
            var component = Instantiate(button, _levelUpPanel.transform).GetComponent<ButtonScript>();
            component.gameObject.SetActive(false);
            _weaponList.Add(component);
        }
        _levelUpPanel.SetActive(true);
        ShowFirstSelectionPanel();
    }
    private void Update()
    {
        if (_isLevelUp)
        {
            _isLevelUp = false;
            LevelUpUI();
            ButtonScript[] buttons = FindObjectsOfType<ButtonScript>();
            foreach (var button in buttons)
            {
                button.ModifyDescription();
            }
        }
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    _isLevelUp = true;
        //}

    }
    public void LevelUpUI()
    {
        _levelUpPanel.SetActive(true);
        PauseManager.Instance.PauseOrResume();
        SelectButton();
    }

    public void CloseUI()
    {
        foreach (var button in _levelUpPanel.transform.GetComponentsInChildren<ButtonScript>())
        {
            if (button.gameObject.activeSelf)
            {
                button.gameObject.SetActive(false);
            }
        }
        PauseManager.Instance.PauseOrResume();
        Debug.Log("ポーズ再開");
        _levelUpPanel.transform.localScale = Vector3.one;
        _levelUpPanel.SetActive(false);
    }

    public void SelectButton()
    {
        var chooseList = new List<ButtonScript>();
        var useItemList = new List<ButtonScript>(_itemList);
        var useWeaponList = new List<ButtonScript>(_weaponList);

        if (_itemList.Count(component => component.ButtonLv == 1) >= 2)
        {
            useItemList.RemoveAll(component => component.ButtonLv == 0);
        }
        useItemList.RemoveAll(component => component.ButtonLv == 4);

        if (_weaponList.Count(component => component.ButtonLv == 1) >= 4)
        {
            useWeaponList.RemoveAll(component => component.ButtonLv == 0);
        }
        useWeaponList.RemoveAll(component => component.ButtonLv == 4);

        var entryList = new List<ButtonScript>(useItemList.Concat(useWeaponList));

        while (chooseList.Count < 3)
        {
            System.Random random = new System.Random();
            var choose = entryList[random.Next(0, entryList.Count)];
            if (!chooseList.Contains(choose))
            {
                chooseList.Add(choose);
                choose.gameObject.SetActive(true);
            }
        }
        
    }
    private void ShowFirstSelectionPanel()
    {
        _levelUpPanel.transform.localScale *= 0.73f;
        foreach (var button in _weaponList)
        {
            button.gameObject.SetActive(true);
        }
    }

}
