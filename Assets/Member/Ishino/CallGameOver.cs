using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGameOver : MonoBehaviour
{
    private TestGameManager gameManager;

    private void Start()
    {
        // シーン内の TestGameManager を探して参照を取得
        gameManager = GameObject.FindObjectOfType<TestGameManager>();
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.TriggerGameOver();
        }
    }
}

