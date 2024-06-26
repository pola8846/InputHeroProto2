using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeEnemy_idle : State
{

    public TestRangeEnemy_idle(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        Enemy enemy = (Enemy)unit;
        if (enemy.FindPlayer())
        {
            ChangeState<TestRangeEnemy_chase>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
