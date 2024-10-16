using UnityEngine;

public class DebugTools : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private KeyCode keyCode;

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Instantiate(obj, transform.position, Quaternion.identity);
        }
    }
}
