using UnityEngine;

/// <summary>
/// 지원: 지속성 이벤트들의 베이스 클래스
/// </summary>

public class TimedEventBase : MonoBehaviour
{
    // 상태 - 순차적으로 true로 변하며 이벤트가 끝난다
    bool initialized = false;   // 타이머가 초기화 되었는가
    bool eventStarted = false;  // 이벤트가 시작 되었는가
    bool eventEnded = false;    // 이벤트가 끝났는가

    // 큐의 앞줄에 도착했을때만 풀어주는 대기 상태 변수
    public bool isWaiting = true;

    float delayTimeLeft;        // 이벤트 직전 딜레이 시간 카운트    
    float eventLifetimeLeft;    // 본격 이벤트 시간 카운트

    public bool EventEnded
    {
        get { return eventEnded; }
    }

    // eventDuration을 0으로 설정시 단발성 이벤트로 간주되어 OnEventUpdate문은 한 번도 실행되지 않는다
    public void Init(float eventDuration = 0.0F, float delayTime = 0.0F)
    {
        if (initialized) return;

        delayTimeLeft = delayTime;
        eventLifetimeLeft = eventDuration;

        initialized = true;
    }

    void Update()
    {
        if (isWaiting) return;
        if (!initialized) return;

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
        OnEventEnd();
        Destroy(gameObject);
    }
}