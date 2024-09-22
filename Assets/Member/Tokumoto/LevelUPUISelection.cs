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
    [SerializeField] List<GameObject> _weaponButtons;
    public List<GameObject> _useWeaponList = new List<GameObject>();
    [SerializeField] List<Transform> _buttonPositions;
    List<Button> _useButton;


    private void Start()
    {
        _isLevelUp = false;
        _levelUpPanel.SetActive(false);
    }
    private void Update()
    {
        if (_isLevelUp)
        {
            LevelUpUI();
        }
    }
    public void LevelUpUI()
    {
        _levelUpPanel.SetActive(true);

        for(int i = 0; i < 3; i++)
        {
            //weaopnButtons‚©‚ç1‚Æ‚Âƒ‰ƒ“ƒ_ƒ€‚Å‘I‚Ô
            GameObject randomObj = _weaponButtons[UnityEngine.Random.Range(0, _weaponButtons.Count)];
            _useWeaponList.Add(randomObj);
            int choiceNum = _weaponButtons.IndexOf(randomObj);
            Instantiate(randomObj, _levelUpPanel.transform);
            _weaponButtons.RemoveAt(choiceNum);
        }

    }

    public void CloseLvUPUI()
    {
        _levelUpPanel.SetActive(false);
    }
}
