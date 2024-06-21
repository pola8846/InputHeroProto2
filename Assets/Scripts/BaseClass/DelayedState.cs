using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��/�ĵ��� ������ �ð��� ���� ����Ǵ� ����. ���ϴ� ��ŭ �� ȿ���� �ݺ��� �� ����.
/// </summary>
public class DelayedState : State
{
    /*
     ���� ���� �� ���� ����, OnEnterEarly() ���� -> 
    earlyDelay ��� �� �� ȿ�� ���� -> 
    (OnEnterMain() ����-> mainDelay ��� �� repeatCounter ����)�� repeatNum��ŭ ���� ->
    OnEnterLate() ���� ->
    lateDelay ��� �� OnEnterIdle() ����
     */

    /// <summary>
    /// ���� �ð�
    /// </summary>
    protected float earlyDelay;

    /// <summary>
    /// �� ȿ�� �ð�
    /// </summary>
    protected float mainDelay;

    /// <summary>
    /// �ĵ� �ð�
    /// </summary>
    protected float lateDelay;

    /// <summary>
    /// �� ȿ�� �ݺ� Ƚ��
    /// </summary>
    protected int repeatNum;

    protected int repeatCounter;
    protected TickTimer timer;
    protected DelayCondition delayCondition;

    //����� Ŭ������ �����ڿ��� Set() ȣ�� �ʿ�
    public DelayedState(StateMachine machine) : base(machine)
    {
        timer = new();
        delayCondition = DelayCondition.idle;
    }

    /// <summary>
    /// �ʱ� ����
    /// </summary>
    protected void Set(float earlyDelay, float mainDelay, float lateDelay, int repeatNum)
    {
        this.earlyDelay = earlyDelay;
        this.mainDelay = mainDelay;
        this.lateDelay = lateDelay;
        this.repeatNum = repeatNum;
    }

    public override void Enter()
    {
        timer.Reset();
        delayCondition = DelayCondition.early;
        repeatCounter = 0;
        OnEnterEarly();
    }

    //���ĵ� ��꿡 ���ǹǷ� EarlyAct/MainAct/LateAct�� ��� ����� ��.
    public override void Execute()
    {
        switch (delayCondition)
        {
            case DelayCondition.early:
                EarlyAct();
                if (timer.Check(earlyDelay))
                {
                    delayCondition = DelayCondition.main;
                    timer.Reset();
                    OnEnterMain();
                }
                break;
            case DelayCondition.main:
                MainAct();
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
                break;
            case DelayCondition.late:
                LateAct();
                if (timer.Check(lateDelay))
                {
                    timer.Reset();
                    delayCondition = DelayCondition.idle;
                    OnEnterIdle();
                }
                break;
        }
    }

    /// <summary>
    /// ���� ���� �� ����� �Լ�
    /// </summary>
    protected virtual void OnEnterEarly()
    {

    }

    /// <summary>
    /// ���� ���� �� �����Ӹ��� ����� �Լ�
    /// </summary>
    protected virtual void EarlyAct()
    {

    }

    /// <summary>
    /// �� �ൿ ���� �ø��� ����� �Լ�. 
    /// �� �ൿ�� ���� �� �ݺ��ϸ� �Ź� ����
    /// </summary>
    protected virtual void OnEnterMain()
    {

    }

    /// <summary>
    /// �� �ൿ ���� �� �����Ӹ��� ����� �Լ�
    /// </summary>
    protected virtual void MainAct()
    {

    }

    /// <summary>
    /// �ĵ� ���� �� ����� �Լ�
    /// </summary>
    protected virtual void OnEnterLate()
    {

    }

    /// <summary>
    /// �ĵ� ���� �� �����Ӹ��� ����� �Լ�
    /// </summary>
    protected virtual void LateAct()
    {

    }

    /// <summary>
    /// �ĵ� ���� �� ����� �Լ�
    /// </summary>
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