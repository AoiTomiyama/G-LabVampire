using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusOnUI : MonoBehaviour
{
    [SerializeField, Header("体力ゲージ")]
    private Image _healthBar;
    [SerializeField, Header("経験値ゲージ")]
    private Image _expBar;
    [SerializeField, Header("キルカウントを表示させるテキスト")]
    private Text _killCountText;
    [SerializeField, Header("ゲームオーバー時にキルカウントを表示させるテキスト")]
    private Text _GameOverkillCountText;
    private void Start()
    {
        //UIの初期化
        RefreshUI(1, UpdateParameterType.Health);
        RefreshUI(0, UpdateParameterType.Experience);
        RefreshUI(0, UpdateParameterType.KillCount);
        //デリゲートへ記録
        FindObjectOfType<PlayerBehaviour>().DisplayOnUI = RefreshUI;
    }
    private void RefreshUI(float value, UpdateParameterType type)
    {
        switch (type)
        {
            case UpdateParameterType.Health:
                _healthBar.fillAmount = value;
                break;
            case UpdateParameterType.Experience:
                _expBar.fillAmount = value;
                break;
            case UpdateParameterType.KillCount:
                _killCountText.text = ((int)value).ToString() + " 討";
                _GameOverkillCountText.text = _killCountText.text;
                break;
        };
    }
}
public enum UpdateParameterType
{
    Health,
    Experience,
    KillCount,
}
