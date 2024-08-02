using UnityEngine;

public class TestEnemy_Boss_1_AtkB1 : DelayedState
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
    private BulletShooter Shooter => Source.Shooter;

    public TestEnemy_Boss_1_AtkB1(StateMachine machine) : base(machine)
    {
        earlyDelay = DelayE;
        mainDelay = DelayM;
        lateDelay = DelayL;
    }
    protected override void OnEnterEarly()
    {
        Source.targetPlatform = UnityEngine.Random.Range(0, 2) == 0 ? Source.PlatformL : Source.PlatformR;

        Source.transform.position = TargetPlatform.transform.position + 
            (Vector3.up * TargetPlatform.lossyScale.y * 0.5f) + (Vector3.up * 2f);
        Shooter.shootType = ShootType.fan;
        Shooter.BulletNum = Source.BarrageAttack1BulletNum;
        Shooter.BulletSpeed = 6f;
        Source.MoverV.SetVelocity(new Vector2(0, 0.2f));
    }
    protected override void OnEnterMain()
    {
        Source.ShootToPlayer(Source.BarrageAttack1Angle);
    }
    protected override void OnEnterLate()
    {
        Source.StopMove();
    }

    protected override void OnEnterIdle()
    {
        machine.ChangeState<TestEnemy_Boss_1_Idle>();
    }
}
