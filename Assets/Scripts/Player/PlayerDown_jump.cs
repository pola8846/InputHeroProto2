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

        // �����߿��� �̵� ����
        if (InputManager.IsKeyDownOrPushing(InputType.MoveLeft))//���� ������ �ִٸ�
        {
            source.MoverV.SetVelocityX(Mathf.Max(0, source.Speed) * -1);//�̵�
            if (!source.IsLookLeft)//���� ��ȯ
            {
                source.Turn();
            }
        }
        else if (InputManager.IsKeyDownOrPushing(InputType.MoveRight))//������ ������ �ִٸ�
        {
            source.MoverV.SetVelocityX(Mathf.Max(0, source.Speed));//�̵�
            if (source.IsLookLeft)//���� ��ȯ
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