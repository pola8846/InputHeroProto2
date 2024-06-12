using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCornChecker : MonoBehaviour
{
    public GameObject target;
    public float angle;
    public float angleSize;
    public float distance;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameTools.IsInCorn(target.transform.position, transform.position, angle, angleSize, distance));
    }
}
