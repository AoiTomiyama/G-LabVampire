using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    LevelUPUISelection _lvSelection;
    [SerializeField] GameObject Weapon;
    GameObject _player;
    GameObject _system;
    [SerializeField] ButtonType _buttonType;
    Button button;
    WeaponGenerator _weaponGenerator;


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
        if(_lvSelection._hudaLv != 0)
        {
            GameObject wpn = _player.transform.Find("FudaCore(Clone)").gameObject;
            _weaponGenerator = wpn.gameObject.GetComponent<WeaponGenerator>();
        }

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
            _weaponGenerator.LevelUp();
        }
        else
        {
            _lvSelection._hudaLv++;
            _weaponGenerator.LevelUp();
        }

    }

    public void KaminariButtonPressed()
    {
        if (_lvSelection._kaminariLv != 0)
        {
            GameObject wpn = _player.transform.Find("ThunderCore(Clone)").gameObject;
            _weaponGenerator = wpn.gameObject.GetComponent<WeaponGenerator>();
        }

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
            _weaponGenerator.LevelUp();
        }
        else
        {
            _lvSelection._kaminariLv++;
            _weaponGenerator.LevelUp();
        }
    }

    public void KekkaiButtonPressed()
    {
        if (_lvSelection._kekkaiLv != 0)
        {
            GameObject wpn = _player.transform.Find("ShieldCore(Clone)").gameObject;
            _weaponGenerator = wpn.gameObject.GetComponent<WeaponGenerator>();
        }

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
            _weaponGenerator.LevelUp();
        }
        else
        {
            _lvSelection._kekkaiLv++;
            _weaponGenerator.LevelUp();
        }
    }
    public void NaginataButtonPressed()
    {
        if (_lvSelection._naginataLv != 0)
        {
            GameObject wpn = _player.transform.Find("NaginataCore(Clone)").gameObject;
            _weaponGenerator = wpn.gameObject.GetComponent<WeaponGenerator>();
        }

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
            _weaponGenerator.LevelUp();
        }
        else
        {
            _lvSelection._naginataLv++;
            _weaponGenerator.LevelUp();
        }
    }
    public void NihontouButtonPressed()
    {
        if (_lvSelection._nihontouLv != 0)
        {
            GameObject wpn = _player.transform.Find("KatanaCore(Clone)").gameObject;
            _weaponGenerator = wpn.gameObject.GetComponent<WeaponGenerator>();
        }

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
            _weaponGenerator.LevelUp();
        }
        else
        {
            _lvSelection._nihontouLv++;
            _weaponGenerator.LevelUp();
        }
    }
    public void SikigamiButtonPressed()
    {
        if (_lvSelection._sikigamiLv != 0)
        {
            GameObject wpn = _player.transform.Find("ShikigamiCore(Clone)").gameObject;
            _weaponGenerator = wpn.gameObject.GetComponent<WeaponGenerator>();
        }

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
            _weaponGenerator.LevelUp();
        }
        else
        {
            _lvSelection._sikigamiLv++;
            _weaponGenerator.LevelUp();
        }
    }

    enum ButtonType { Huda, Kaminari, Kekkai, Naginata, Nihontou, Sikigami }
}
