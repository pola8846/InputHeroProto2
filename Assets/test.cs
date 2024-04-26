using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float speed = 5;
    private void Start()
    {
        StartCoroutine(tt());
    }

    private IEnumerator tt()
    {
        while (true)
        {
            Debug.Log(transform.position.x);
            yield return new WaitForSeconds(1f);
        }
    }
}
