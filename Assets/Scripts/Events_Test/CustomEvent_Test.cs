using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent_Test : MonoBehaviour
{
    [SerializeReference]
    public List<CustomHandler_Test> handlers = new List<CustomHandler_Test>();

    public float delayTime;
    int currentHandlerIndex = 0;

    // �̺�Ʈ�� �����ֱ� �Һ���
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
        }
    }

    void Update()
    {
        if (isDone) return;

        handlers[currentHandlerIndex].Run();

        if (handlers[currentHandlerIndex].IsDone)
        {
            currentHandlerIndex++;

            if (currentHandlerIndex >= handlers.Count)
            {
                isDone = true;
                return;
            }

            handlers[currentHandlerIndex].Enter();
        }
    }

    [ContextMenu("���̾�α� ����� �߰�")]
    void AddBasicSpriteAnim() { handlers.Add(new DialogueHandler_Test()); }
}
