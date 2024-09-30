using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : MonoBehaviour
{

    public TimeManager timeManager;

    [Header("ゲームオーバー時の処理")]
    public UnityEvent onGameOver;

      void Start()
    {
        if (timeManager != null)
        {
            timeManager.StartTime();
        }
    }

    // ゲームオーバーを発生させる
    public void TriggerGameOver()
    {
        if (onGameOver != null)
        {
            onGameOver.Invoke();
        }
    }
}
