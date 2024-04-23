using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float speed = 5;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        { 
            transform.position = transform.position - new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
