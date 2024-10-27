using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGameClear : MonoBehaviour
{
    private TestGameManager gameManager;

    private void Start()
    {
        // �V�[������ TestGameManager ��T���ĎQ�Ƃ��擾
        gameManager = GameObject.FindObjectOfType<TestGameManager>();
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.TriggerGameClear();
        }
    }
}

