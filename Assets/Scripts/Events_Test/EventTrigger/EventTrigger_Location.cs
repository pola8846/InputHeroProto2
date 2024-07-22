using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger_Location : MonoBehaviour
{
    // 타겟이 콜라이더에 들어오면 이벤트 호출
    public GameObject target;
    public int eventIDToCall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null) return;

        if (collision.gameObject == target)
        {
            Debug.Log(eventIDToCall + "번 이벤트 호출");
            EventManager_Test.Instance.TriggerEventID(eventIDToCall);
        }
    }
}