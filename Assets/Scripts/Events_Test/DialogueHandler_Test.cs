using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler_Test : CustomHandler_Test
{
    public int dialogueIndex;
    public DialoguesSO_Test dialogueSO;

    public override void Enter()
    {
        base.Enter();

        Debug.Log(dialogueSO.dialogues[dialogueIndex]);
    }

    public override void Run() // isDone_This�� true�� ����� ������ �߰����ּ���
    {
        base.Run();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDone_This = true;
        }
    }

    protected override void Exit() // ������ �ѹ� ������ �ڵ�
    {
        Debug.Log(dialogueIndex + "�� ��� ��~");

        base.Exit();
    }
}
