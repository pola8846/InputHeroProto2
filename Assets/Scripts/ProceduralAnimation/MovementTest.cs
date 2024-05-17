using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    public float spd;

    void Update()
    {
        if(Input.GetAxis("horizontal") >= 0.1)
        {
            transform.position = transform.position + transform.position * spd * Time.deltaTime;
        } else if (Input.GetAxis("horizontal") <= -0.1)
        {
            transform.position = transform.position - transform.position * spd * Time.deltaTime;
        }


        Debug.Log(Input.GetAxis("horizontal"));

    }
}
