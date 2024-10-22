using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timeLimit = 600f; 
    private float currentTime = 0f;
    private bool isTimeRunning = false;

    public Text TimeText;
    public Text GameoverText;
    public UnityEvent TimeUP;

    void Update()
    {
        if (isTimeRunning)
        {
            currentTime += Time.deltaTime;

            UpdateTimeText();

            if (currentTime >= timeLimit)
            {
                TimeUP.Invoke();
                StopTime();
            }
        }
    }

    // 時間を開始する
    public void StartTime()
    {
        isTimeRunning = true;
    }

    // 時間を停止する
    public void StopTime()
    {
        isTimeRunning = false;
    }

    // 現在の時間を取得する
    public float GetCurrentTime()
    {
        return currentTime;
    }


    // 時間をリセットして再スタート
    public void ResetTime()
    {
        currentTime = 0f;
        isTimeRunning = false;
        UpdateTimeText();
    }

    // テキストを時間のフォーマットで更新する
    void UpdateTimeText()
    {
        if(isTimeRunning)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f); // 経過時間の分数
            int seconds = Mathf.FloorToInt(currentTime % 60f); // 経過時間の秒数
            TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            GameoverText.text = TimeText.text;
        }
    }
}
