using UnityEngine;

public class TestEnemy_Boss_1_AtkM1 : DelayedState
{

    private const float DelayE = 1.5f;
    private const float DelayM = 1.5f;
    private const float DelayL = 1.5f;
    private GameObject meleeAttackGO;
    private TestEnemy_Boss_1 Source
    {
        get
        {
            return (TestEnemy_Boss_1)unit;
        }
    }

    public TestEnemy_Boss_1_AtkM1(StateMachine machine) : base(machine)
    {
        earlyDelay = DelayE;
        mainDelay = DelayM;
        lateDelay = DelayL;
    }

    protected override void OnEnterEarly()
    {
        Source.SetColor(Color.red);
    }

    protected override void OnEnterMain()
    {
        meleeAttackGO = Object.Instantiate(Source.MeleeAttackObject, Source.transform);
        meleeAttackGO.GetComponent<Attack>().Initialization(Source, "Player", meleeAttackGO);
        Object.Destroy(meleeAttackGO, DelayM);
    }

    protected override void OnEnterIdle()
    {
        machine.ChangeState<TestEnemy_Boss_1_Idle>();
    }
}
