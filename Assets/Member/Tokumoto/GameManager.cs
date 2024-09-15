using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float _time;
    public bool _isPaused = false;
        
    private void Start()
    {
        _isPaused = false;
    }
    private void Update()
    {
        _time = Time.deltaTime;
    }

    
}
