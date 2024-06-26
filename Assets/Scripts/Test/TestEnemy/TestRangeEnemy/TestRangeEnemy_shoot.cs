using UnityEngine;
public class TestRangeEnemy_shoot : TimedState
{
    private TestRangeEnemy source => (TestRangeEnemy)unit;
    private bool isRight => GameManager.Player.transform.position.x >= unit.transform.position.x;//플레이어가 오른쪽에 있는가?
    public TestRangeEnemy_shoot(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer.checkTime = source.aimTIme;
    }

    public override void Execute()
    {
        if (!source.FindPlayerInEngage())
        {
            ChangeState<TestRangeEnemy_chase>();
            return;
        }
        else
        {
            AimPlayer();
        }
        base.Execute();
    }

    protected override void Main()
    {
        base.Execute();
        ChangeState<TestRangeEnemy_shoot2>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void AimPlayer()
    {
        if (source.IsLookLeft == isRight)
        {
            source.Turn();
        }
        var dir = -(GameManager.Player.transform.position - source.transform.position).normalized;
        source.armBox.transform.right = dir;
        source.angle = GameTools.GetDegreeAngleFormDirection(-dir);
    }
}
