using UnityEngine;

public class TestEnemy_Boss_1_AtkB2 : DelayedState
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

    public TestEnemy_Boss_1_AtkB2(StateMachine machine) : base(machine)
    {
        earlyDelay = DelayE;
        mainDelay = DelayM;
        lateDelay = DelayL;
        repeatNum = RepeatNum;
    }

    protected override void OnEnterEarly()
    {
        Source.SetColor(Color.cyan);
        Source.Shooter.shootType = ShootType.Fan;
        Source.Shooter.BulletNum = Source.BarrageAttack2BulletNum;
        Source.Shooter.bulletAngleMax = 360;
        Source.Shooter.bulletAngleMin = 0;
        Source.Shooter.BulletSpeed = 3f;
    }

    protected override void OnEnterMain()
    {
        float temp = (360 / Source.BarrageAttack2BulletNum) / 2;
        Source.Shooter.bulletAngleMax += temp;
        Source.Shooter.bulletAngleMin += temp;
    }

    protected override void OnEnterIdle()
    {
        machine.ChangeState<TestEnemy_Boss_1_Idle>();
    }
}
