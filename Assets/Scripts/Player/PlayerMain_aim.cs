using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain_aim : State
{
    public PlayerMain_aim(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
        //ChangeState<TestFsmEnemy_a>();
    }

    public override void Exit()
    {
        base.Exit();
    }
}