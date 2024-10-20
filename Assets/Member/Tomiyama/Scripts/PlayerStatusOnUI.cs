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
    private void Start()
    {
        FindObjectOfType<PlayerBehaviour>().DisplayOnUI = (float value, UpdateParameterType type) =>
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
                    _killCountText.text = value.ToString("00000") + " 討";
                    break;
            }
        };
    }
}
public enum UpdateParameterType
{
    Health,
    Experience,
    KillCount,
}
