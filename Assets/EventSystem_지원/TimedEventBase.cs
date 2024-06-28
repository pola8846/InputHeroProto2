using UnityEngine;

/// <summary>
/// ����: ���Ӽ� �̺�Ʈ���� ���̽� Ŭ����
/// </summary>

public class TimedEventBase
{
    bool eventStarted = false;  // �̺�Ʈ�� ���� �Ǿ��°�
    bool eventEnded = false;    // �̺�Ʈ�� �����°�

    float delayTimeLeft;        // �̺�Ʈ ���� ������ �ð� ī��Ʈ    
    float eventLifetimeLeft;    // ���� �̺�Ʈ �ð� ī��Ʈ

    // eventDuration�� 0���� ������ �ܹ߼� �̺�Ʈ�� ���ֵǾ� OnEventUpdate���� �� ���� ������� �ʴ´�
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
        // �����ð� ī��Ʈ
        if (delayTimeLeft > 0)
        {
            delayTimeLeft -= Time.deltaTime;
            return;
        }

        // �����ð��� ������ �̺�Ʈ ����
        if (!eventStarted)
        {
            eventStarted = true;
            OnEventStart();
        }

        if (!eventEnded)
        {
            // �̺�Ʈ ��
            if (eventLifetimeLeft <= 0)
            {
                eventEnded = true;
                return;
            }

            // �̺�Ʈ ���� ��
            eventLifetimeLeft -= Time.deltaTime;
            OnEventUpdate();
        }
    }

    protected virtual void OnEventStart() { }
    protected virtual void OnEventUpdate() { }
    protected virtual void OnEventEnd() { }

    // ������ �̺�Ʈ ������
    public void Release()
    {
        eventEnded = true;
        OnEventEnd();
    }
}