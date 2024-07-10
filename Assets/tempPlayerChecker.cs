using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayerChecker : MonoBehaviour
{
    public GameObject t;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Å¬¸®¾î");
            t.SetActive(true);
        }
    }
}
