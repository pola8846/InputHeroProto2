using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger_Location : MonoBehaviour
{
    // Ÿ���� �ݶ��̴��� ������ �̺�Ʈ ȣ��
    public GameObject target;
    public int eventIDToCall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null) return;

        if (collision.gameObject == target)
        {
            Debug.Log(eventIDToCall + "�� �̺�Ʈ ȣ��");
        }
    }
}