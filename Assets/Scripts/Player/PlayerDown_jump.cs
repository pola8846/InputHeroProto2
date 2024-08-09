using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDown_jump : State
{
    private PlayerUnit source => (PlayerUnit)unit;

    public PlayerDown_jump(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        source.MoverV.SetVelocityY(0, true);
        source.MoverV.AddForceY(source.JumpPower);
    }

    public override void Execute()
    {
        base.Execute();

        // 점프중에도 이동 가능
        if (InputManager.IsKeyDownOrPushing(InputType.MoveLeft))//왼쪽 누르고 있다면
        {
            source.MoverV.SetVelocityX(Mathf.Max(0, source.Speed) * -1);//이동
            if (!source.IsLookLeft)//방향 전환
            {
                source.Turn();
            }
        }
        else if (InputManager.IsKeyDownOrPushing(InputType.MoveRight))//오른쪽 누르고 있다면
        {
            source.MoverV.SetVelocityX(Mathf.Max(0, source.Speed));//이동
            if (source.IsLookLeft)//방향 전환
            {
                source.Turn();
            }
        }

        // [JUMP -> IDLE]
        if (source.GroundCheck() && source.MoverV.Velocity.y < 0.01F)
        {
            ChangeState<PlayerDown_idle>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}