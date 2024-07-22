using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneHandler_Test : CustomHandler_Test
{
    public override void Enter()
    {
        base.Enter();

        //
    }

    public override void Run() // isDone_This를 true로 만드는 조건을 추가
    {
        base.Run();

        //
    }

    protected override void Exit() // 나갈때 한번 실행할 코드
    {
        //

        base.Exit();
    }
}
