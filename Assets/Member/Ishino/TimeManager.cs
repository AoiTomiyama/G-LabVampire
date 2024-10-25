using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour, IPausable
{
    public float timeLimit = 600f;
    private float currentTime = 0f;
    private bool isTimeRunning = false;

    public Text TimeText;
    public Text GameoverText;
    public UnityEvent TimeUP;

    private void Update()
    {
        if (isTimeRunning)
        {
            currentTime += Time.deltaTime;

            UpdateTimeText();

            if (currentTime >= timeLimit)
            {
                TimeUP.Invoke();
            }
        }
    }

    // ���Ԃ��J�n����
    public void Resume()
    {
        isTimeRunning = true;
    }

    // ���Ԃ��~����
    public void Pause()
    {
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
    private void UpdateTimeText()
    {
        if (isTimeRunning)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f); // �o�ߎ��Ԃ̕���
            int seconds = Mathf.FloorToInt(currentTime % 60f); // �o�ߎ��Ԃ̕b��
            TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            GameoverText.text = TimeText.text;
        }
    }
}
