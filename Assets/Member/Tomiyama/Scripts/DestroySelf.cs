using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    void OnAnimationComplete()
    {
        gameObject.SetActive(false);
    }
}
