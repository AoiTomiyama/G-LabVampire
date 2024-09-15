using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Healingitem : MonoBehaviour
{
    [SerializeField] int _healingAmount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour))
                
        {
            
        }
    }
}
