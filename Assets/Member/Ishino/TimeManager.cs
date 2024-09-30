using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timeLimit = 600f; 
    private float currentTime = 0f;
    private bool isTimeRunning = false;

    public Text timeText;


    void Update()
    {
        if (isTimeRunning)
        {
            currentTime += Time.deltaTime; 

            UpdateTimeText();

            if (currentTime >= timeLimit)
            {
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
        Debug.Log("時間を止める");
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
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
