using UnityEngine;

public class DamageTextDestroy : MonoBehaviour
{
    void OnAnimationComplete()
    {
        Destroy(gameObject);
    }
}
