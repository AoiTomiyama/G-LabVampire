using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : MonoBehaviour
{

    public TimeManager timeManager;

    [Header("�Q�[���X�^�[�g���̏���")]
    public UnityEvent onGameStart;

    [Header("�Q�[���I�[�o�[���̏���")]
    public UnityEvent onGameOver;

    [Header("�Q�[���N���A�̏���")]
    public UnityEvent onGameClear;



    void Start()
    {
        TriggerStart();
    }

    public void TriggerStart()
    {
        if (onGameStart != null)
        {
            PauseManager.Instance.PauseAll();
            onGameStart.Invoke();
        }
    }

    // �Q�[���I�[�o�[�𔭐�������
    public void TriggerGameOver()
    {
        if (onGameOver != null)
        {
            PauseManager.Instance.PauseAll();
            onGameOver.Invoke();
        }
    }

    public void TriggerGameClear()
    {
        if (onGameClear != null)
        {
            PauseManager.Instance.PauseAll();
            onGameClear.Invoke();
        }
    }
}
