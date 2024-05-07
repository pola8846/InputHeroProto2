using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletDestroier : MonoBehaviour
{

    private bool b = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (b)
        {
            return;
        }
        if (Time.timeScale < 1)
        {
            var temp = collision.GetComponent<TestBulletChecker>();
            if (temp != null)
            {
                b= true;
                collision.GetComponent<DamageArea>()?.Destroy();
                //Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
