using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusOnUI : MonoBehaviour
{
    [SerializeField, Header("�̗̓Q�[�W")]
    private Image _healthBar;
    [SerializeField, Header("�o���l�Q�[�W")]
    private Image _expBar;
    [SerializeField, Header("�L���J�E���g��\��������e�L�X�g")]
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
                    _killCountText.text = value.ToString("00000") + " ��";
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
