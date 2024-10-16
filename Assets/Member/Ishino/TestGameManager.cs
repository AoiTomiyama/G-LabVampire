using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : MonoBehaviour
{

    public TimeManager timeManager;

    [Header("ゲームスタート時の処理")]
    public UnityEvent onGameStart;

    [Header("ゲームオーバー時の処理")]
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

    // ゲームオーバーを発生させる
    public void TriggerGameOver()
    {
        if (onGameOver != null)
        {
            onGameOver.Invoke();
        }
    }
}
