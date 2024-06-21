using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFsmEnemy_b : State
{
    private TickTimer _timer;

    public TestFsmEnemy_b(StateMachine machine) : base(machine)
    {
        _timer = new TickTimer();
    }

    public override void Enter()
    {
        base.Enter();
        _timer.Reset();
        Debug.Log("TestFsmEnemy_b.Enter");
    }

    public override void Execute()
    {
        base.Execute();
        //Debug.Log("TestFsmEnemy_b.Execute");
        if (_timer != null && _timer.Check(3))
        {
            machine.ChangeState<TestFsmEnemy_a>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("TestFsmEnemy_b.Exit");
    }
}
