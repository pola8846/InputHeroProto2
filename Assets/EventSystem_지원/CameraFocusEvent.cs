using UnityEngine;

public class CameraFocusEvent : TimedEventBase
{
    protected override void OnEventStart()
    {
        Debug.Log("OnEventStart");
    }

    protected override void OnEventUpdate()
    {
        Debug.Log("OnEventUpdate");
    }

    protected override void OnEventEnd()
    {
        Debug.Log("OnEventEnd");
    }
}