using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkill_TestAreaAtk : PlayerSkill
{
    public PSkill_TestAreaAtk()
    {
        neededCombo = new();
        neededCombo.Add(InputType.Shoot);
        neededCombo.Add(InputType.Shoot);
        neededCombo.Add(InputType.MoveDown);
    }
    public override void Invoke()
    {
        base.Invoke();
        Player.StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        yield return null;
        Debug.Log("PSkill_TestAreaAtk ½ÇÇà");
        yield return null;
        End();
    }
}
