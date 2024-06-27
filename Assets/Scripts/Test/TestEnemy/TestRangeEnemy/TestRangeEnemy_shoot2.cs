using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestRangeEnemy_shoot2 : TimedState
{
    private TestRangeEnemy source => (TestRangeEnemy)unit;
    public TestRangeEnemy_shoot2(StateMachine machine) : base(machine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer.checkTime = source.shootTime;
    }

    public override void Execute()
    {
        base.Execute();
    }

    protected override void Main()
    {
        base.Main();

        source.shooter.BulletAngle = source.angle;
        source.shooter.Triger();

        ChangeState<TestRangeEnemy_shoot>();
    }

    public override void Exit() 
    {
        base.Exit();
    }
}
