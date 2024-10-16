using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : MonoBehaviour
{

    public TimeManager timeManager;

    [Header("�Q�[���X�^�[�g���̏���")]
    public UnityEvent onGameStart;

    [Header("�Q�[���I�[�o�[���̏���")]
    public UnityEvent onGameOver;



      void Start()
    {
        TriggerStart();
    }

    public void TriggerStart()
    {
        if (onGameStart != null)
        {
            onGameStart.Invoke();
        }
    }

    // �Q�[���I�[�o�[�𔭐�������
    public void TriggerGameOver()
    {
        if (onGameOver != null)
        {
            onGameOver.Invoke();
        }
    }
}
