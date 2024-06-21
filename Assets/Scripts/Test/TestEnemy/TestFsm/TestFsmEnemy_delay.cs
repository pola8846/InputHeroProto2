using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFsmEnemy_delay : DelayedState
{
    public TestFsmEnemy_delay(StateMachine machine) : base(machine)
    {
        Set(1, 1.5f, 1, 4);
    }

    protected override void EarlyAct()
    {
        base.EarlyAct();
    }

    protected override void LateAct()
    {
        base.LateAct();
    }

    protected override void MainAct()
    {
        base.MainAct();
    }

    protected override void OnEnterEarly()
    {
        base.OnEnterEarly();
        Debug.Log("TestFsmEnemy_delay.OnEnterEarly");
    }

    protected override void OnEnterIdle()
    {
        base.OnEnterIdle();
        Debug.Log("TestFsmEnemy_delay.OnEnterIdle");
        ChangeState<TestFsmEnemy_a>();
    }

    protected override void OnEnterLate()
    {
        base.OnEnterLate();
        Debug.Log("TestFsmEnemy_delay.OnEnterLate");
    }

    protected override void OnEnterMain()
    {
        base.OnEnterMain();
        Debug.Log("TestFsmEnemy_delay.OnEnterMain");
    }
}
