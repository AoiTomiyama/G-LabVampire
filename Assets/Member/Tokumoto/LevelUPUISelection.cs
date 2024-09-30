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
    [SerializeField] public List<GameObject> _weaponButtons;
    public List<GameObject> _useWeaponList = new List<GameObject>();
    [SerializeField] List<Transform> _buttonPositions;
    List<Button> _useButton;
    GameObject _player;
    WeaponManager _weaponManager;
    public int _hudaLv = 0;
    public int _kaminariLv = 0;
    public int _kekkaiLv = 0;
    public int _naginataLv = 0;
    public int _nihontouLv = 0;
    public int _sikigamiLv = 0;

    private void Start()
    {
        _levelUpPanel.SetActive(false);
        _player = GameObject.Find("Player");
        _isLevelUp = true;
    }
    private void Update()
    {
        if (_isLevelUp)
        {
            _isLevelUp = false;
            LevelUpUI();
        }

        if (Input.GetKeyDown(KeyCode.Z) && _isLevelUp == false)
        {
            _isLevelUp = true;
        }

    }
    public void LevelUpUI()
    {
        _levelUpPanel.SetActive(true);

        for(int i = 0; i < 3; i++)
        {
            //weaopnButtons‚©‚ç1‚Âƒ‰ƒ“ƒ_ƒ€‚Å‘I‚Ô
            GameObject randomObj = _weaponButtons[UnityEngine.Random.Range(0, _weaponButtons.Count)];
            _useWeaponList.Add(randomObj);
            int choiceNum = _weaponButtons.IndexOf(randomObj);
            GameObject obj = Instantiate(randomObj, _levelUpPanel.transform);
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(CloseUI);

            _weaponButtons.RemoveAt(choiceNum);
        }
    }

    public void CloseUI()
    {
        _levelUpPanel.SetActive(false);
        _weaponButtons.AddRange(_useWeaponList);
        _useWeaponList.Clear();

        int children = _levelUpPanel.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject child = _levelUpPanel.transform.GetChild(i).gameObject;
            GameObject.Destroy(child);
        }
    }
}
