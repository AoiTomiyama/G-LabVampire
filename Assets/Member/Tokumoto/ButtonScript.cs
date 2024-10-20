using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    LevelUPUISelection _lvSelection;
    [SerializeField] GameObject _object;
    PlayerBehaviour _player;
    GameObject _system;
    Button button;
    WeaponGenerator _weaponGenerator;
    bool _hasAdded;
    private int buttonLv;
    ILevelUppable _generatedObject;
    [SerializeField] string[] _infoTexts;

    public int ButtonLv { get => buttonLv; set => buttonLv = value; }

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerBehaviour>();
        _system = GameObject.Find("System");
        _lvSelection = _system.GetComponent<LevelUPUISelection>();

        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonPressed);
        button.onClick.AddListener(_lvSelection.CloseUI);

    }
    void ButtonPressed()
    {
        if (ButtonLv == 0)
        {
            _generatedObject = Instantiate(_object, _player.transform).GetComponent<ILevelUppable>();
        }
        else
        {
            _generatedObject.LevelUp();
        }
        ButtonLv++;
    }
    void ModifyDescription()
    {

    }
}
