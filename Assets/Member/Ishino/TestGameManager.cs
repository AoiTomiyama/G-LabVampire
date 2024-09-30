using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public TimeManager timeManager;

      void Start()
    {
        if (timeManager != null)
        {
            timeManager.StartTime();
        }
    }
}
