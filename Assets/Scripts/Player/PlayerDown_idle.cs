using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDown_idle : State
{
    private PlayerUnit source => (PlayerUnit)unit;

    public PlayerDown_idle(StateMachine machine) : base(machine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        if (source.CanMove)
        {
            // [IDLE -> WALK]
            if (InputManager.IsKeyDownOrPushing(InputType.MoveLeft) || InputManager.IsKeyDownOrPushing(InputType.MoveRight))
            {
                ChangeState<PlayerDown_walk>();
            }

            // [IDLE -> JUMP]
            if (InputManager.IsKeyDown(InputType.Jump))
            {
                ChangeState<PlayerDown_jump>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
