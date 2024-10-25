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
    [SerializeField, Header("�Q�[���I�[�o�[���ɃL���J�E���g��\��������e�L�X�g")]
    private Text _GameOverkillCountText;
    private void Start()
    {
        //UI�̏�����
        RefreshUI(1, UpdateParameterType.Health);
        RefreshUI(0, UpdateParameterType.Experience);
        RefreshUI(0, UpdateParameterType.KillCount);
        //�f���Q�[�g�֋L�^
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
                _killCountText.text = ((int)value).ToString() + " ��";
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
