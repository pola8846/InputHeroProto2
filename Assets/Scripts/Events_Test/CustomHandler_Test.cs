using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// 실행부 = Handler
public class CustomHandler_Test : ScriptableObject // 리스트로 가진 실행부 클래스들을 동시 실행
{
    //public CustomHandler_Test parent = null; // 아직은 필요 없다

    // 실행부 리스트
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    // 실행부의 생명주기 불변수
    public LifeCycleBools lifeCycleBools;

    public virtual void Enter()
    {
        lifeCycleBools.Reset();
        foreach (CustomHandler_Test handler in handlers)
        {
            handler.lifeCycleBools.Reset();
        }

        lifeCycleBools.isRunning = true;

        // 시작과 동시에 자식 실행부들도 실행
        foreach (CustomHandler_Test handler in handlers)
        {
            //handler.parent = this;
            handler.Enter();
        }
    }

    public virtual void Run()
    {
        if (!lifeCycleBools.isRunning || lifeCycleBools.isDone) return;

        foreach (CustomHandler_Test handler in handlers)
        {
            handler.Run();
        }

        // 실행부 리스트의 개체들이 모두 isDone이 되면 이 실행부도 끝난다
        if (!lifeCycleBools.isDone_ChildHandlers && handlers.Count(handler => handler.lifeCycleBools.isDone) >= handlers.Count)
        {
            lifeCycleBools.isDone_ChildHandlers = true;
        }

        if (lifeCycleBools.isDone_This && lifeCycleBools.isDone_ChildHandlers)
        {
            Exit();
        }
    }

    protected virtual void Exit()
    {
        lifeCycleBools.isDone = true;
    }

    [ContextMenu("대화창 실행부 추가")]
    void AddDialogueHandler_Test() { handlers.Add(new DialogueHandler_Test()); }
}

public struct LifeCycleBools
{
    public bool isRunning;

    // isDone_ChildHandlers와 isDone_This가 모두 true가 되면 isDone도 true가 되며 실행부가 최종적으로 끝나는 구조
    public bool isDone_This;
    public bool isDone_ChildHandlers;
    public bool isDone;

    // 불변수들 초기화하기
    public void Reset()
    {
        isRunning = false;
        isDone = false;
        isDone_ChildHandlers = false;
        isDone_This = false;
    }

    public bool GetIsDone() { return isDone; }

    public bool IsReset() // 불변수들이 초기화된 상태인지 (아니면 진행중인 인스턴스로 취급)
    {
        return (!isRunning && !isDone && !isDone_ChildHandlers && !isDone_This);
    }
}