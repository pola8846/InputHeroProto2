using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 시간마다 특정 행위를 실행하는 걸 반복하는 상태
/// </summary>
public class TimedState : State
{
    protected TickTimer timer;
    public TimedState(StateMachine machine):base(machine)
    {
        unit = machine.Unit;
        this.machine = machine;
        timer = new(autoReset: true);
    }

    public override void Enter()
    {
        timer.Reset();
    }

    public override void Execute()
    {
        if (timer.Check())
        {
            Main();
            timer.Reset();
        }
    }
    
    protected virtual void Main()
    {

    }
}

