using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallGameOver : MonoBehaviour
{
    private TestGameManager gameManager;

    private void Start()
    {
        // ƒV[ƒ““à‚Ì TestGameManager ‚ğ’T‚µ‚ÄQÆ‚ğæ“¾
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

