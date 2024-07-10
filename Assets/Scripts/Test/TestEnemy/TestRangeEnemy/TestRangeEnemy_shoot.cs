using UnityEngine;
public class TestRangeEnemy_shoot : TimedState
{
    private TestRangeEnemy source => (TestRangeEnemy)unit;
    private bool isRight
    {
        get
        {
            if (GameManager.Player!=null)
            {
                return GameManager.Player.transform.position.x >= unit.transform.position.x;//플레이어가 오른쪽에 있는가?
            }
            else
                return false;
        }
    }
    public TestRangeEnemy_shoot(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        source._Top.SetTargetSprite("Move");
        timer.checkTime = source.aimTIme;
    }

    public override void Execute()
    {
        move();
        AimPlayer();
        base.Execute();
    }

    protected override void Main()
    {
        base.Execute();
        ChangeState<TestRangeEnemy_shoot2>();
    }

    public override void Exit()
    {
        source.mover.StopMove();
        base.Exit();
    }

    private void AimPlayer()
    {
        if (GameManager.Player==null)
        {
            return;
        }

        if (source.IsLookLeft == isRight)
        {
            source.Turn();
        }
        var dir = -(GameManager.Player.transform.position - source.transform.position).normalized;
        source.armBox.transform.right = dir;
        source.angle = GameTools.GetDegreeAngleFormDirection(-dir);
        source.shooter.SetBulletAngle(source.angle, source.angleRange);
    }


    private void move()
    {
        float movementX = Mathf.Max(0, unit.Speed) * (isRight ? 1 : -1) * source.engageSpeedRate;
        if (isRight == unit.IsLookLeft)
        {
            unit.Turn();
        }
        source.mover.SetVelocityX(movementX);
    }


    private void move()
    {
        float movementX = Mathf.Max(0, unit.Speed) * (isRight ? 1 : -1) * source.engageSpeedRate;
        if (isRight == unit.IsLookLeft)
        {
            unit.Turn();
        }
        source.mover.SetVelocityX(movementX);
    }
}
