using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_Test : MonoBehaviour // ����Ʈ�� ���� ����� Ŭ�������� ���� ����
{
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    public float delayTime;
    int currentHandlerIndex = 0;

    // �̺�Ʈ�� �����ֱ� �Һ���
    bool isRunning = false;
    public bool isDone = false;

    void Start()
    {
        if (handlers.Count == 0)
        {
            isDone = true;
            return;
        }

        Invoke("StartEvent", delayTime);
    }

    void StartEvent()
    {
        if (handlers.Count > 0)
        {
            handlers[0].Enter();
            isRunning = true;
        }
    }

    void Update()
    {
        if (!isRunning || isDone) return;

        handlers[currentHandlerIndex].Run();

        if (handlers[currentHandlerIndex].lifeCycleBools.GetIsDone())
        {
            handlers[currentHandlerIndex].lifeCycleBools.Reset();
            currentHandlerIndex++;

            if (currentHandlerIndex >= handlers.Count)
            {
                isDone = true;
                return;
            }

            handlers[currentHandlerIndex].Enter();
        }
    }

    [ContextMenu("��ȭâ ����� �߰�")]
    void AddDialogueHandler_Test() { handlers.Add(new DialogueHandler_Test()); }
}
