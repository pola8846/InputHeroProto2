using UnityEngine;

/// <summary>
/// 지원: 이런식으로 TimedEventBase를 상속한 이벤트를 유형에따라 몇개 만들것..
/// </summary>
public class CameraFocusEvent : TimedEventBase
{
    public CameraFocusEvent(float eventDuration = 0.0F, float delayTime = 0.0F)
        : base(eventDuration, delayTime)
    { }

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