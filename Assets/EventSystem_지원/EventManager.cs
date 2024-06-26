using System;
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
    public void EnqueueCameraFocusEvent(GameObject o, float eventDuration = 0.0F, float delayTIme = 0.0F)
    {
        GameObject newEventObject = new GameObject("CameraFocusEvent");
        newEventObject.AddComponent<CameraFocusEvent>();
        newEventObject.GetComponent<CameraFocusEvent>().Init(eventDuration, delayTIme);
        eventQueue_test.Enqueue(newEventObject.GetComponent<CameraFocusEvent>());
    }

    // �׽�Ʈ��
    void Update()
    {
        // test
        if (Input.GetKeyDown(KeyCode.U))
        {
            EnqueueCameraFocusEvent(GameObject.Find("Boss"), 0.0F, 1.0F);
        }

        if (eventQueue_test.Count == 0) return;
        TimedEventBase frontEvent = eventQueue_test.Peek();
        if (frontEvent == null) return;

        // ���� ������ �̺�Ʈ�� �������̸� Ǯ�� �ش�
        if (frontEvent.isWaiting == true)
        {
            frontEvent.isWaiting = false;
        }
        
        // ���� ������ �̺�Ʈ�� �������� ����(ť���� ������ ����)
        if (frontEvent.EventEnded == true)
        {
            eventQueue_test.Dequeue().Release();
        }
    }
}
