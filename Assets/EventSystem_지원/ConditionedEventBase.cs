using UnityEngine;

/// <summary>
/// ����: ���Ǻ� �̺�Ʈ���� ���̽� Ŭ����
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
