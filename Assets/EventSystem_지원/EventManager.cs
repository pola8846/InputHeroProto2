using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ����: �̺�Ʈ�Ŵ��� - �̱���
/// �̺�Ʈ ���� ���� �Լ����� ������ �ִ�..
/// </summary>

public class EventManager : MonoBehaviour
{
    // �̺�Ʈ ��⿭
    Queue<TimedEventBase> eventQueue_test;

    // �̱���
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

    // Test - ��⿭ �̺�Ʈ�� �̺�Ʈ �߰�
    public void EnqueEvent(float eventDuration = 0.0F, float delayTime = 0.0F)
    {
        TimedEventBase newEvent = new CameraFocusEvent(eventDuration, delayTime);
        eventQueue_test.Enqueue(newEvent);
    }

    // �׽�Ʈ��
    void Update()
    {
        // test
        if (Input.GetKeyDown(KeyCode.U))
        {
            EnqueEvent(1.0F, 1.0F);
        }

        // ť�� ���� ������ �̺�Ʈ�� ������Ʈ��Ŵ
        if (eventQueue_test.Count == 0) return;
        TimedEventBase currentEvent = eventQueue_test.Peek();
        if (currentEvent == null) return;

        currentEvent.Update();
        
        // ���� ������ �̺�Ʈ�� �������� ����(ť���� ������ ����)
        if (currentEvent.EventEnded == true)
        {
            eventQueue_test.Dequeue().Release();
        }
    }
}
