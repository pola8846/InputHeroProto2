using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFsmEnemy_timed : TimedState
{
    public TestFsmEnemy_timed(StateMachine machine) : base(machine)
    {
        timer.checkTime = 2;
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("TestFsmEnemy_timed.Enter");

    }

    public override void Execute()
    {
        base.Execute();
    }


    protected override void Main()
    {
        base.Main();
        Debug.Log("TestFsmEnemy_timed.Main");
        if (counter >= 3)
        {
            ChangeState<TestFsmEnemy_a>();
        }
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("TestFsmEnemy_timed.Exit");
    }
}
