using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackEnd : MonoBehaviour
{
    [SerializeField]
    private PlayerUnit player;
    public GameObject testAtk1;
    private GameObject testAtk1GO;

    public void AttackEnd()
    {
        player.AttackEnd();
    }

    public void AttackEvent1(int option)
    {
        switch (option)
        {
            case 1:
                testAtk1GO = Instantiate(testAtk1, player.transform);
                testAtk1GO.GetComponent<TestAttack1>().Initialization(player, "Enemy");
                break;
            case 4:
                Destroy(testAtk1GO);
                break;
            default:
                break;
        }
        testAtk1GO.GetComponent<TestAttack1>().SetPhase(option);

    }
}
