using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// 실행부 = Handler
[Serializable]
public class CustomHandler_Test // 리스트로 가진 실행부 클래스들을 동시 실행
{
    CustomHandler_Test parent = null;

    // 실행부 리스트
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    // 실행부의 생명주기 불변수 isRunning -> isDone 순으로 순차적으로 true가 되며 생명이 끝남
    bool isRunning = false;

    // 모체가 isDone 변수를 보고 이 실행부가 완전히 끝났는지 판단함
    bool isDone = false;
    public bool IsDone { get { return isDone; } }

    // 아래 두 불변수가 모두 true가 되면 isDone이 최종적으로 true가 됨
    bool isDone_childHandlers = false;
    protected bool isDone_This = false;

    public virtual void Enter()
    {
        isRunning = true;

        // 시작과 동시에 자식 실행부들도 실행
        foreach (CustomHandler_Test handler in handlers)
        {
            handler.parent = this;
            handler.Enter();
        }
    }

    public virtual void Run()
    {
        if (!isRunning || isDone) return;

        foreach (CustomHandler_Test handler in handlers)
        {
            handler.Run();
        }

        // 실행부 리스트의 개체들이 모두 isDone이 되면 이 실행부도 끝난다
        if (!isDone_childHandlers && handlers.Count(handler => handler.isDone) >= handlers.Count)
        {
            isDone_childHandlers = true;
        }

        if (isDone_This && isDone_childHandlers)
        {
            Exit();
        }
    }

    protected virtual void Exit()
    {
        isDone = true;
    }
}