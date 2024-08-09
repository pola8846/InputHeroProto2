using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDown_walk : State
{
    private PlayerUnit source => (PlayerUnit)unit;

    public PlayerDown_walk(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        if (source.CanMove)
        {
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
            else
            {
                // [WALK -> IDLE]
                ChangeState<PlayerDown_idle>();
            }

            // [WALK -> JUMP]
            if (InputManager.IsKeyDown(InputType.Jump))
            {
                ChangeState<PlayerDown_jump>();
            }
        }
        else
        {
            // [WALK -> IDLE]
            ChangeState<PlayerDown_idle>();
        }
    }

    public override void Exit()
    {
        source.MoverV.StopMoveX();
        base.Exit();
    }
}