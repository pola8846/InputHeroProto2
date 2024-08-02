public class TestEnemy_Boss_1_Move : State
{
    private TickTimer timer_MaxTime;
    public TestEnemy_Boss_1_Move(StateMachine machine) : base(machine)
    {
        timer_MaxTime = new();
    }

    private TestEnemy_Boss_1 Source
    {
        get
        {
            return (TestEnemy_Boss_1)unit;
        }
    }

    public override void Enter()
    {
        base.Enter();
        timer_MaxTime.Reset();
        Source.MoveToPlayer();
    }

    public override void Execute()
    {
        if (Source.IsLookLeft == (GameManager.Player.transform.position.x > 
            Source.transform.position.x + Source.MeleeAttack1CheckDistance * (Source.IsLookLeft ? -1 : 1)))
        {
            machine.ChangeState<TestEnemy_Boss_1_AtkM1>();
            return;
        }
        else if (timer_MaxTime.Check(Source.Move_MaxTime))
        {
            machine.ChangeState<TestEnemy_Boss_1_Idle>();
            return;
        }
        base.Execute();
    }

    public override void Exit()
    {
        Source.StopMove();
    }
}
