using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1_Idle : TimedState
{
    private TickTimer timer_waitMaxTime;
    public TestEnemy_Boss_1_Idle(StateMachine machine) : base(machine)
    {
        timer_waitMaxTime = new();
    }
    private TestEnemy_Boss_1 Source
    {
        get
        {
            return (TestEnemy_Boss_1)unit;
        }
    }

    //public override void Enter()
    //{
    //    Source.SetColor(Color.clear);
    //    Source.StopMove();
    //    timer.time = 0.1f;
    //}

    //public override void Execute()
    //{
    //    if (timer_waitMaxTime.Check(Source.Wait_MaxTime))
    //    {
    //        machine.ChangeState<TestEnemy_Boss_1_Move>();
    //    }
    //    else
    //    {
    //        base.Execute();
    //    }
    //}

    protected override void Main()
    {
        
    }

    public override void Exit()
    {

    }
}
