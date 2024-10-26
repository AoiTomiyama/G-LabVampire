using UnityEngine;
using UnityEngine.Events;

public class TestGameManager : MonoBehaviour
{

    public TimeManager timeManager;

    [Header("ゲームスタート時の処理")]
    public UnityEvent onGameStart;

    [Header("ゲームオーバー時の処理")]
    public UnityEvent onGameOver;

    [Header("ゲームクリアの処理")]
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

    // ゲームオーバーを発生させる
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
