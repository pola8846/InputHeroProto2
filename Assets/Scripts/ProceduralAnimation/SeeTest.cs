using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SeeTest : MonoBehaviour
{
    public Transform center;
    public float radious = 2f;
    public float moveSpd = 3.0f;

    private float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");

        //ansform.position += new Vector3(h, V, 0) * moveSpd * Time.deltaTime;


        angle += moveSpd * Time.deltaTime;

        transform.position = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radious;

    }
}
