using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FollowToTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target;


    [SerializeField]
    private Vector3 xPos;
    [SerializeField]
    private Vector3 y;
    [SerializeField] private Vector3 dy;
    [SerializeField] Vector3 x;
    [SerializeField] Vector3 dx;

    float t;




    [Header("속도 및 가속도")]
    public float k1;
    public float k2;
    public float k3;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void secondOrderDynamics(float f, float z, float r, Vector3 x0)
    {
        // 각 가속도 및 속도 변인 도함수 변수 - 미적보정치
        k1 = z / Mathf.PI * f;
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);

        // 각 변수 값 호출
        xPos = x0;
        y = x0;
        dy = x0;

    }

    // Update is called once per frame
    void Update()
    {
        xPos = target.transform.position;

        if (dx == null)
        {
            dx = (x - xPos) / t;
            xPos = x;
        }

        y = y + t * dy;
        dy = dy + t * (x + k3 * dx - y - k1 * dy) / k2;


    }
}