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

    public override void Run() // isDone_This를 true로 만드는 조건을 추가해주세요
    {
        base.Run();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDone_This = true;
        }
    }

    protected override void Exit() // 나갈때 한번 실행할 코드
    {
        Debug.Log(dialogueIndex + "번 대사 끝~");

        base.Exit();
    }
}
