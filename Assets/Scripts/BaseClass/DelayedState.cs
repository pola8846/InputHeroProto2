using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 선/후딜을 가지고 시간에 의해 진행되는 상태. 원하는 만큼 주 효과를 반복할 수 있음.
/// </summary>
public class DelayedState : State
{
    /*
     상태 진입 시 선딜 진입, OnEnterEarly() 실행 -> 
    earlyDelay 경과 후 주 효과 진입 -> 
    (OnEnterMain() 실행-> mainDelay 경과 후 repeatCounter 증가)를 repeatNum만큼 실행 ->
    OnEnterLate() 실행 ->
    lateDelay 경과 후 OnEnterIdle() 실행
     */

    /// <summary>
    /// 선딜 시간
    /// </summary>
    protected float earlyDelay;

    /// <summary>
    /// 주 효과 시간
    /// </summary>
    protected float mainDelay;

    /// <summary>
    /// 후딜 시간
    /// </summary>
    protected float lateDelay;

    /// <summary>
    /// 주 효과 반복 횟수
    /// </summary>
    protected int repeatNum;

    protected int repeatCounter;
    protected TickTimer timer;
    protected DelayCondition delayCondition;

    //상속한 클래스의 생성자에서 Set() 호출 필요
    public DelayedState(StateMachine machine) : base(machine)
    {
        timer = new();
        delayCondition = DelayCondition.idle;
    }

    /// <summary>
    /// 초기 설정
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

    //선후딜 계산에 사용되므로 EarlyAct/MainAct/LateAct를 대신 사용할 것.
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
    /// 선딜 시작 시 실행될 함수
    /// </summary>
    protected virtual void OnEnterEarly()
    {

    }

    /// <summary>
    /// 선딜 도중 매 프레임마다 실행될 함수
    /// </summary>
    protected virtual void EarlyAct()
    {

    }

    /// <summary>
    /// 본 행동 시작 시마다 실행될 함수. 
    /// 본 행동을 여러 번 반복하면 매번 실행
    /// </summary>
    protected virtual void OnEnterMain()
    {

    }

    /// <summary>
    /// 본 행동 도중 매 프레임마다 실행될 함수
    /// </summary>
    protected virtual void MainAct()
    {

    }

    /// <summary>
    /// 후딜 시작 시 실행될 함수
    /// </summary>
    protected virtual void OnEnterLate()
    {

    }

    /// <summary>
    /// 후딜 도중 매 프레임마다 실행될 함수
    /// </summary>
    protected virtual void LateAct()
    {

    }

    /// <summary>
    /// 후딜 종료 시 실행될 함수
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