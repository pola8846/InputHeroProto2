using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// ����� = Handler
[Serializable]
public class CustomHandler_Test // ����Ʈ�� ���� ����� Ŭ�������� ���� ����
{
    CustomHandler_Test parent = null;

    // ����� ����Ʈ
    [SerializeReference, SubclassSelector]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    // ������� �����ֱ� �Һ��� isRunning -> isDone ������ ���������� true�� �Ǹ� ������ ����
    bool isRunning = false;

    // ��ü�� isDone ������ ���� �� ����ΰ� ������ �������� �Ǵ���
    bool isDone = false;
    public bool IsDone { get { return isDone; } }

    // �Ʒ� �� �Һ����� ��� true�� �Ǹ� isDone�� ���������� true�� ��
    bool isDone_childHandlers = false;
    protected bool isDone_This = false;

    public virtual void Enter()
    {
        isRunning = true;

        // ���۰� ���ÿ� �ڽ� ����ε鵵 ����
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

        // ����� ����Ʈ�� ��ü���� ��� isDone�� �Ǹ� �� ����ε� ������
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