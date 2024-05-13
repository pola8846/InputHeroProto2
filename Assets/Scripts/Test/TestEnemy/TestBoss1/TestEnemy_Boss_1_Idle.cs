using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1_Idle : State
{
    public TestEnemy_Boss_1_Idle(StateMachine machine) : base(machine)
    {

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
     Source.SetColor(Color.clear);   
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}
