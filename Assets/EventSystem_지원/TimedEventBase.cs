using UnityEngine;

/// <summary>
/// 지원: 지속성 이벤트들의 베이스 클래스
/// </summary>

public class TimedEventBase
{
    bool eventStarted = false;  // 이벤트가 시작 되었는가
    bool eventEnded = false;    // 이벤트가 끝났는가

    float delayTimeLeft;        // 이벤트 직전 딜레이 시간 카운트    
    float eventLifetimeLeft;    // 본격 이벤트 시간 카운트

    // eventDuration을 0으로 설정시 단발성 이벤트로 간주되어 OnEventUpdate문은 한 번도 실행되지 않는다
    public TimedEventBase(float eventDuration = 0.0F, float delayTime = 0.0F)
    {
        delayTimeLeft = delayTime;
        eventLifetimeLeft = eventDuration;
    }

    public bool EventEnded
    {
        get { return eventEnded; }
    }

    public void Update()
    {
        // 지연시간 카운트
        if (delayTimeLeft > 0)
        {
            delayTimeLeft -= Time.deltaTime;
            return;
        }

        // 지연시간이 지나면 이벤트 시작
        if (!eventStarted)
        {
            eventStarted = true;
            OnEventStart();
        }

        if (!eventEnded)
        {
            // 이벤트 끝
            if (eventLifetimeLeft <= 0)
            {
                eventEnded = true;
                return;
            }

            // 이벤트 실행 중
            eventLifetimeLeft -= Time.deltaTime;
            OnEventUpdate();
        }
    }

    protected virtual void OnEventStart() { }
    protected virtual void OnEventUpdate() { }
    protected virtual void OnEventEnd() { }

    // 강제로 이벤트 마무리
    public void Release()
    {
        eventEnded = true;
        OnEventEnd();
    }
}