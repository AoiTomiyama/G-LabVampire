using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using UnityEngine.Rendering.VirtualTexturing;
using System;
using System.Xml.Linq;

public class ButtonScript : MonoBehaviour
{
    LevelUPUISelection _lvSelection;
    [SerializeField] GameObject Weapon;
    GameObject _player;
    GameObject _system;
    [SerializeField] ButtonType _buttonType;
    Button button;



    private void Start()
    {
        _player = GameObject.Find("Player");
        _system = GameObject.Find("System");
        _lvSelection = _system.GetComponent<LevelUPUISelection>();

        button = GetComponent<Button>();


        switch (_buttonType)
        {
            case ButtonType.Huda:
                button.onClick.AddListener(HudaButtonPressed);
                break;
            case ButtonType.Kaminari:
                button.onClick.AddListener(KaminariButtonPressed);
                break;
            case ButtonType.Kekkai:
                 button.onClick.AddListener(KekkaiButtonPressed);
                break;
            case ButtonType.Naginata:
                button.onClick.AddListener(NaginataButtonPressed);
                break;
            case ButtonType.Nihontou:
                button.onClick.AddListener(NihontouButtonPressed);
                break;
            case ButtonType.Sikigami:
                button.onClick.AddListener(SikigamiButtonPressed);
                break;
        }
    }

    public void HudaButtonPressed()
    {
        if (_lvSelection._hudaLv == 0)
        {
            //札取得処理
            Instantiate(Weapon, _player.transform);
            _lvSelection._hudaLv++;
        }
        else if(_lvSelection._hudaLv == 3)
        {
            //札のレベルを4にする、札ボタンを消す
            _lvSelection._hudaLv++;
            Debug.Log("札レベル" + _lvSelection._hudaLv);
            _lvSelection._weaponButtons.RemoveAll(obj => obj.name == "GetHudaButton");
        }
        else
        {
            _lvSelection._hudaLv++;
        }
    }

    public void KaminariButtonPressed()
    {
        if (_lvSelection._kaminariLv == 0)

        {
            Instantiate(Weapon, _player.transform);
            _lvSelection._kaminariLv++;
        }
        else if (_lvSelection._kaminariLv == 3)
        {
            _lvSelection._kaminariLv++;
            Debug.Log("雷レベル" + _lvSelection._kaminariLv);
            _lvSelection._weaponButtons.RemoveAll(obj => obj.name == "GetKaminariButton");
        }
        else
        {
            _lvSelection._kaminariLv++;
        }
    }

    public void KekkaiButtonPressed()
    {
        if (_lvSelection._kekkaiLv == 0)
        {
            Instantiate(Weapon, _player.transform);
            _lvSelection._kekkaiLv++;
        }
        else if (_lvSelection._kekkaiLv == 3)
        {
            _lvSelection._kekkaiLv++;
            Debug.Log("結界レベル" + _lvSelection._kekkaiLv);
            _lvSelection._weaponButtons.RemoveAll(obj => obj.name == "GetHudaButton");
        }
        else
        {
            _lvSelection._kekkaiLv++;
        }
    }
    public void NaginataButtonPressed()
    {
        if (_lvSelection._naginataLv == 0)
        {
            Instantiate(Weapon, _player.transform);
            _lvSelection._naginataLv++;
        }
        else if (_lvSelection._naginataLv == 3)
        {
            _lvSelection._naginataLv++;
            Debug.Log("薙刀レベル" + _lvSelection._naginataLv);
            _lvSelection._weaponButtons.RemoveAll(obj => obj.name == "GetHudaButton");
        }
        else
        {
            _lvSelection._naginataLv++;
        }
    }
    public void NihontouButtonPressed()
    {
        if (_lvSelection._nihontouLv == 0)
        {
            Instantiate(Weapon, _player.transform);
            _lvSelection._nihontouLv++;
        }
        else if (_lvSelection._nihontouLv == 3)
        {
            _lvSelection._nihontouLv++;
            Debug.Log("日本刀レベル" + _lvSelection._nihontouLv);
            _lvSelection._weaponButtons.RemoveAll(obj => obj.name == "GetHudaButton");
        }
        else
        {
            _lvSelection._nihontouLv++;
        }
    }
    public void SikigamiButtonPressed()
    {
        if (_lvSelection._sikigamiLv == 0)
        {
            Instantiate(Weapon, _player.transform);
            _lvSelection._sikigamiLv++;
        }
        else if (_lvSelection._sikigamiLv == 3)
        {
            _lvSelection._sikigamiLv++;
            Debug.Log("式神レベル" + _lvSelection._sikigamiLv);
            _lvSelection._weaponButtons.RemoveAll(obj => obj.name == "GetHudaButton");
        }
        else
        {
            _lvSelection._sikigamiLv++;
        }
    }

    enum ButtonType { Huda, Kaminari, Kekkai, Naginata, Nihontou, Sikigami }
}
