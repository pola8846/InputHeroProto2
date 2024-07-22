using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger_KeyInput : MonoBehaviour
{
    public int eventIDToCall;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            EventManager_Test.Instance.TriggerEventID(eventIDToCall);
        }
    }
}
