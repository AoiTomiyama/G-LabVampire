using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testplayer : MonoBehaviour
{
    private float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.up * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.up * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right * speed * Time.deltaTime;

    }
}
