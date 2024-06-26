using UnityEngine;

/// <summary>
/// ����: ���Ӽ� �̺�Ʈ���� ���̽� Ŭ����
/// </summary>

public class TimedEventBase : MonoBehaviour
{
    // ���� - ���������� true�� ���ϸ� �̺�Ʈ�� ������
    bool initialized = false;   // Ÿ�̸Ӱ� �ʱ�ȭ �Ǿ��°�
    bool eventStarted = false;  // �̺�Ʈ�� ���� �Ǿ��°�
    bool eventEnded = false;    // �̺�Ʈ�� �����°�

    // ť�� ���ٿ� ������������ Ǯ���ִ� ��� ���� ����
    public bool isWaiting = true;

    float delayTimeLeft;        // �̺�Ʈ ���� ������ �ð� ī��Ʈ    
    float eventLifetimeLeft;    // ���� �̺�Ʈ �ð� ī��Ʈ

    public bool EventEnded
    {
        get { return eventEnded; }
    }

    // eventDuration�� 0���� ������ �ܹ߼� �̺�Ʈ�� ���ֵǾ� OnEventUpdate���� �� ���� ������� �ʴ´�
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
        OnEventEnd();
        Destroy(gameObject);
    }
}