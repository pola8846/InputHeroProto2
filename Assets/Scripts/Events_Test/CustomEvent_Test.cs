using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomEvent_Test_JW")]
public class CustomEvent_Test : ScriptableObject // 리스트로 가진 실행부 클래스들을 순차 실행
{
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    [SerializeField]
    float delayTime; // 아직은 무시된다

    public List<int> nextEventIDToCall = new List<int>();

    // 발동 횟수에 따라 다른 이벤트를 반환하기 위한 값
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

    // 게임당 한번만 초기화할 부분
    public void Initialize()
    {
        stage = EventStage.INACTIVE;
        countValue = 0;
    }

    void Enter()
    {
        // 이벤트 실행시마다 초기화할 부분
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

                if (result) { Debug.Log(this.name + " 이벤트가 " + nextIdx + "번 이벤트 호출 성공"); }
                else { Debug.Log(this.name + " 이벤트가 " + nextIdx + "번 이벤트 호출 실패"); }
            }
        }
    }

    public void Process()
    {
        if (stage == EventStage.ENTER) Enter();
        else if (stage == EventStage.RUN) Run();
        else if (stage == EventStage.EXIT) Exit();
    }

    [ContextMenu("대화창 실행부 추가")]
    void AddDialogueHandler_Test() { handlers.Add(new DialogueHandler_Test()); }
}