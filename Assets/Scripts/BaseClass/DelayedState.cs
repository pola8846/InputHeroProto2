using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 선/후딜을 가지고 시간에 의해 진행되는 상태. 원하는 만큼 주 효과를 반복할 수 있음.
/// </summary>
public class DelayedState : State
{
    protected float earlyDelay;
    protected float mainDelay;
    protected float lateDelay;
    protected int repeatNum;
    protected int repeatCounter;
    protected TickTimer timer;
    protected DelayCondition delayCondition;

    public DelayedState(StateMachine machine) : base(machine)
    {
        timer = new();
        delayCondition = DelayCondition.idle;
    }

    public override void Enter()
    {
        timer.Reset();
        delayCondition = DelayCondition.early;
        repeatCounter = 0;
        OnEnterEarly();
    }

    public override void Execute()
    {
        switch (delayCondition)
        {
            case DelayCondition.early:
                if (timer.Check(earlyDelay))
                {
                    delayCondition = DelayCondition.main;
                    timer.Reset();
                    OnEnterMain();
                }
                break;
            case DelayCondition.main:
                if (timer.Check(mainDelay))
                {
                    repeatCounter++;
                    if (repeatCounter >= repeatNum)
                    {
                        delayCondition = DelayCondition.late;
                        timer.Reset();
                        OnEnterLate();
                    }
                    else
                    {
                        timer.Reset();
                        OnEnterMain();
                    }
                }
                else
                {
                    MainAct();
                }
                break;
            case DelayCondition.late:
                if (timer.Check(lateDelay))
                {
                    timer.Reset();
                    delayCondition = DelayCondition.idle;
                    OnEnterIdle();
                }
                break;
        }
    }

    protected virtual void OnEnterEarly()
    {

    }

    protected virtual void OnEnterMain()
    {

    }

    protected virtual void MainAct()
    {

    }

    protected virtual void OnEnterLate()
    {

    }


    protected virtual void OnEnterIdle()
    {

    }


    protected enum DelayCondition
    {
        early,
        main,
        late,
        idle
    }
}