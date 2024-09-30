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

    // ���Ԃ��J�n����
    public void StartTime()
    {
        isTimeRunning = true;
    }

    // ���Ԃ��~����
    public void StopTime()
    {
        Debug.Log("���Ԃ��~�߂�");
        isTimeRunning = false;
    }

    // ���݂̎��Ԃ��擾����
    public float GetCurrentTime()
    {
        return currentTime;
    }

    // ���Ԃ����Z�b�g���čăX�^�[�g
    public void ResetTime()
    {
        currentTime = 0f;
        isTimeRunning = false;
        UpdateTimeText();
    }

    // �e�L�X�g�����Ԃ̃t�H�[�}�b�g�ōX�V����
    void UpdateTimeText()
    {
        if(isTimeRunning)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f); // �o�ߎ��Ԃ̕���
            int seconds = Mathf.FloorToInt(currentTime % 60f); // �o�ߎ��Ԃ̕b��
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
