using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack1 : Attack
{
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;

    public void SetPhase(int option)
    {
        switch (option)
        {
            case 1:
                EnrollDamage(box1);
                box2.SetActive(false);
                box3.SetActive(false);
                break;
            case 2:
                box2.SetActive(true);
                EnrollDamage(box2);
                WithdrawDamage(box1);
                box1.SetActive(false);
                break;
            case 3:
                box3.SetActive(true);
                EnrollDamage(box3);
                WithdrawDamage(box2);
                box2.SetActive(false);
                break;
            default:
                break;
        }
    }
}
