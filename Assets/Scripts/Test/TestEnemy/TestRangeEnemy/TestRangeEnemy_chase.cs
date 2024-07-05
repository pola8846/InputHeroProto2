using UnityEngine;

public class TestRangeEnemy_chase : State
{

    private TestRangeEnemy source => (TestRangeEnemy)unit;
    private bool lastDir;
    private bool isRight
    {
        get
        {
            if (GameManager.Player == null)
            {
                return lastDir;
            }
            var result = GameManager.Player.transform.position.x >= unit.transform.position.x;//플레이어가 오른쪽에 있는가?
            lastDir = result;
            return lastDir;
        }
    }
    public TestRangeEnemy_chase(StateMachine machine) : base(machine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        move();
    }

    public override void Execute()
    {
        base.Execute();

        if (!source.FindPlayer())
        {
            ChangeState<TestRangeEnemy_idle>();
        }
        else if (source.FindPlayerInEngage())
        {
            ChangeState<TestRangeEnemy_shoot>();
        }
        else if (isRight == unit.IsLookLeft)
        {
            move();
        }
    }

    public override void Exit()
    {
        base.Exit();

        source.mover.StopMove();

    }

    private void move()
    {
        float movementX = Mathf.Max(0, unit.Speed) * (isRight ? 1 : -1);
        if (isRight == unit.IsLookLeft)
        {
            unit.Turn();
        }
        source.mover.SetVelocityX(movementX);
    }
}
