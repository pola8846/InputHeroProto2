using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// ����� = Handler
public class CustomHandler_Test : ScriptableObject // ����Ʈ�� ���� ����� Ŭ�������� ���� ����
{
    //public CustomHandler_Test parent = null; // ������ �ʿ� ����

    // ����� ����Ʈ
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    // ������� �����ֱ� �Һ���
    public LifeCycleBools lifeCycleBools;

    public virtual void Enter()
    {
        lifeCycleBools.Reset();
        foreach (CustomHandler_Test handler in handlers)
        {
            handler.lifeCycleBools.Reset();
        }

        lifeCycleBools.isRunning = true;

        // ���۰� ���ÿ� �ڽ� ����ε鵵 ����
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

        // ����� ����Ʈ�� ��ü���� ��� isDone�� �Ǹ� �� ����ε� ������
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

    [ContextMenu("��ȭâ ����� �߰�")]
    void AddDialogueHandler_Test() { handlers.Add(new DialogueHandler_Test()); }
}

public struct LifeCycleBools
{
    public bool isRunning;

    // isDone_ChildHandlers�� isDone_This�� ��� true�� �Ǹ� isDone�� true�� �Ǹ� ����ΰ� ���������� ������ ����
    public bool isDone_This;
    public bool isDone_ChildHandlers;
    public bool isDone;

    // �Һ����� �ʱ�ȭ�ϱ�
    public void Reset()
    {
        isRunning = false;
        isDone = false;
        isDone_ChildHandlers = false;
        isDone_This = false;
    }

    public bool GetIsDone() { return isDone; }

    public bool IsReset() // �Һ������� �ʱ�ȭ�� �������� (�ƴϸ� �������� �ν��Ͻ��� ���)
    {
        return (!isRunning && !isDone && !isDone_ChildHandlers && !isDone_This);
    }
}