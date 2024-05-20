using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PSkill_TestAreaAtk : PlayerSkill
{
    public PSkill_TestAreaAtk()
    {
        neededCombo = new()
        {
            InputType.Shoot,
            InputType.Shoot,
            InputType.MoveDown
        };
    }

    private GameObject atkGO;
    public override void Invoke()
    {
        base.Invoke();
        Player.StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        Debug.Log("PSkill_TestAreaAtk ½ÇÇà");
        
        yield return new WaitForSecondsRealtime(.25f);
        atkGO = Object.Instantiate(Player.areaAttackPrefab, Player.transform);
        atkGO.GetComponent<Attack>().Initialization(Player, "Enemy", atkGO);

        yield return new WaitForSecondsRealtime(.1f);
        Object.Destroy(atkGO);

        yield return new WaitForSecondsRealtime(.4f);
        End();
    }
}
