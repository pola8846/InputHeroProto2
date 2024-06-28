using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 지원: 이벤트매니저 - 싱글톤
/// 이벤트 관련 여러 함수들을 가지고 있다..
/// </summary>

public class EventManager : MonoBehaviour
{
    // 이벤트 대기열
    Queue<TimedEventBase> eventQueue_test;

    // 싱글톤
    private static EventManager instance = null;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        eventQueue_test = new Queue<TimedEventBase>();
    }

    // Test - 대기열 이벤트에 이벤트 추가
    public void EnqueEvent(float eventDuration = 0.0F, float delayTime = 0.0F)
    {
        TimedEventBase newEvent = new CameraFocusEvent(eventDuration, delayTime);
        eventQueue_test.Enqueue(newEvent);
    }

    // 테스트용
    void Update()
    {
        // test
        if (Input.GetKeyDown(KeyCode.U))
        {
            EnqueEvent(1.0F, 1.0F);
        }

        // 큐의 가장 앞쪽의 이벤트만 업데이트시킴
        if (eventQueue_test.Count == 0) return;
        TimedEventBase currentEvent = eventQueue_test.Peek();
        if (currentEvent == null) return;

        currentEvent.Update();
        
        // 가장 앞줄의 이벤트가 끝났으면 해제(큐에서 꺼내서 삭제)
        if (currentEvent.EventEnded == true)
        {
            eventQueue_test.Dequeue().Release();
        }
    }
}
