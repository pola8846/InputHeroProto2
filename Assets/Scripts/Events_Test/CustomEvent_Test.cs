using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomEvent_Test_JW")]
public class CustomEvent_Test : ScriptableObject // ����Ʈ�� ���� ����� Ŭ�������� ���� ����
{
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    [SerializeField]
    float delayTime; // ������ ���õȴ�

    public List<int> nextEventIDToCall = new List<int>();

    // �ߵ� Ƚ���� ���� �ٸ� �̺�Ʈ�� ��ȯ�ϱ� ���� ��
    public int countValue = 0;

    int currentHandlerIndex = 0;

    public enum EventStage
    {
        INACTIVE,
        ENTER,
        RUN,
        EXIT
    }

    EventStage stage;
    public EventStage Stage
    {
        get { return stage; }
    }

    public void SetStageEnter()
    {
        stage = EventStage.ENTER;
    }

    // ���Ӵ� �ѹ��� �ʱ�ȭ�� �κ�
    public void Initialize()
    {
        stage = EventStage.INACTIVE;
        countValue = 0;
    }

    void Enter()
    {
        // �̺�Ʈ ����ø��� �ʱ�ȭ�� �κ�
        currentHandlerIndex = 0;

        if (handlers.Count == 0)
        {
            stage = EventStage.EXIT; //isDone = true;
            return;
        }
        else
        {
            handlers[0].Enter();
            stage = EventStage.RUN; //isRunning = true;
        }
    }

    void Run()
    {
        handlers[currentHandlerIndex].Run();

        if (handlers[currentHandlerIndex].lifeCycleBools.GetIsDone())
        {
            handlers[currentHandlerIndex].lifeCycleBools.Reset();
            currentHandlerIndex++;

            if (currentHandlerIndex >= handlers.Count)
            {
                stage = EventStage.EXIT; //isDone = true;
                return;
            }

            handlers[currentHandlerIndex].Enter();
        }
    }

    void Exit()
    {
        stage = EventStage.INACTIVE;

        countValue++;

        if (nextEventIDToCall.Count > 0)
        {
            int nextIdx = countValue - 1;

            if (0 <= nextIdx && nextIdx < nextEventIDToCall.Count)
            {
                bool result = EventManager_Test.Instance.TriggerEventID(nextEventIDToCall[nextIdx]);

                if (result) { Debug.Log(this.name + " �̺�Ʈ�� " + nextIdx + "�� �̺�Ʈ ȣ�� ����"); }
                else { Debug.Log(this.name + " �̺�Ʈ�� " + nextIdx + "�� �̺�Ʈ ȣ�� ����"); }
            }
        }
    }

    public void Process()
    {
        if (stage == EventStage.ENTER) Enter();
        else if (stage == EventStage.RUN) Run();
        else if (stage == EventStage.EXIT) Exit();
    }

    [ContextMenu("��ȭâ ����� �߰�")]
    void AddDialogueHandler_Test() { handlers.Add(new DialogueHandler_Test()); }
}