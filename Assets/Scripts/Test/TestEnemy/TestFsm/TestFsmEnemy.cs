using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class TestFsmEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();

        stateMachine = new(this);
        Debug.Log(1);
        stateMachine.ChangeState<TestFsmEnemy_a>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
    }
}
