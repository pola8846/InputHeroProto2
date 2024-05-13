using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1_AtkA1 : DelayedState
{
    private const float DelayE = 1.5f;
    private const float DelayM = 1.5f;
    private const float DelayL = 1.5f;
    private TestEnemy_Boss_1 Source
    {
        get
        {
            return (TestEnemy_Boss_1)unit;
        }
    }
    private Transform TargetPlatform => Source.targetPlatform;
    private GameObject AttackGO;

    public TestEnemy_Boss_1_AtkA1(StateMachine machine) : base(machine)
    {
        earlyDelay = DelayE;
        mainDelay = DelayM;
        lateDelay = DelayL;
    }

    protected override void OnEnterEarly()
    {
        Source.SetColor(Color.red);
    }

    protected override void OnEnterMain()
    {
        if (ReferenceEquals(TargetPlatform, Source.PlatformD))
        {
            AttackGO = UnityEngine.Object.Instantiate(Source.AreaAttackObjectD);
        }
        else if (ReferenceEquals(TargetPlatform, Source.PlatformR))
        {
            AttackGO = UnityEngine.Object.Instantiate(Source.AreaAttackObjectR);
        }
        else if (ReferenceEquals(TargetPlatform, Source.PlatformL))
        {
            AttackGO = UnityEngine.Object.Instantiate(Source.AreaAttackObjectL);
        }
        AttackGO.GetComponent<Attack>().Initialization(Source, "Player", AttackGO);

        UnityEngine.Object.Destroy(AttackGO, DelayM);
    }
    protected override void OnEnterIdle()
    {
        machine.ChangeState<TestEnemy_Boss_1_Idle>();
    }
}
