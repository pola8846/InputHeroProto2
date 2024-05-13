using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedState : State
{
    protected float time;
    private TickTimer timer;
    public TimedState(StateMachine machine):base(machine)
    {
        unit = machine.Unit;
        this.machine = machine;
        timer = new(time, autoReset: true);
    }

    private Act enterAct;
    private Act exitAct;

    public override void Enter()
    {
        enterAct?.Invoke();
        timer.Reset();
    }

    public override void Execute()
    {
        if (timer.Check())
        {

        }
    }

    public override void Exit() 
    {
        exitAct?.Invoke();
    }
}

