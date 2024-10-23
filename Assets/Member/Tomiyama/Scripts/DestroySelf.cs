using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    void OnAnimationComplete()
    {
        Destroy(gameObject);
    }
}
