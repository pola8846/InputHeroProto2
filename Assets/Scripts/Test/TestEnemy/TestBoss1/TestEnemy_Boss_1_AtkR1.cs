using UnityEngine;

public class TestEnemy_Boss_1_AtkR1 : DelayedState
{
    private const float DelayE = 1.5f;
    private const float DelayM = 1.5f;
    private const float DelayL = 1.5f;
    private const int RepeatNum = 4;
    private TestEnemy_Boss_1 Source
    {
        get
        {
            return (TestEnemy_Boss_1)unit;
        }
    }

    public TestEnemy_Boss_1_AtkR1(StateMachine machine) : base(machine)
    {
        earlyDelay = DelayE;
        mainDelay = DelayM;
        lateDelay = DelayL;
        repeatNum = RepeatNum;
    }

    protected override void OnEnterEarly()
    {
        Source.SetColor(Color.yellow);
        Source.Shooter.shootType = ShootType.oneWay;
        Source.Shooter.BulletNum = 1;
        Source.Shooter.BulletSpeed = 8f;
    }

    protected override void OnEnterMain()
    {
        Source.ShootToPlayer();
    }

    protected override void OnEnterIdle()
    {
        machine.ChangeState<TestEnemy_Boss_1_Idle>();
    }
}
