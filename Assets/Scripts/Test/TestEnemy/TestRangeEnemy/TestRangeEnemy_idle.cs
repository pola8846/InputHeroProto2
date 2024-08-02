public class TestRangeEnemy_idle : State
{
    private TestRangeEnemy source => (TestRangeEnemy)unit;

    public TestRangeEnemy_idle(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        source._Top.SetTargetSprite("Wait");
    }

    public override void Execute()
    {
        base.Execute();

        Enemy enemy = (Enemy)unit;
        if (enemy.FindPlayer())
        {
            ChangeState<TestRangeEnemy_chase>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
