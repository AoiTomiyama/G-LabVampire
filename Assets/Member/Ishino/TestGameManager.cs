using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : MonoBehaviour
{

    public TimeManager timeManager;

    [Header("�Q�[���I�[�o�[���̏���")]
    public UnityEvent onGameOver;

      void Start()
    {
        if (timeManager != null)
        {
            timeManager.StartTime();
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
