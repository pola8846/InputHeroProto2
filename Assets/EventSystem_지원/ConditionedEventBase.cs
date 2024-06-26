using UnityEngine;

/// <summary>
/// 지원: 조건부 이벤트들의 베이스 클래스
/// </summary>

public class ConditionedEventBase : MonoBehaviour
{
    bool eventStarted = false;
    bool eventEnded = false;

    public bool EventEnded
    {
        get { return eventEnded; }
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
