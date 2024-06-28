using UnityEngine;

/// <summary>
/// ����: �̷������� TimedEventBase�� ����� �̺�Ʈ�� ���������� � �����..
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