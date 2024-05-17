using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkill_TestRangeAtk : PlayerSkill
{
    public PSkill_TestRangeAtk()
    {
        neededCombo = new();
        neededCombo.Add(InputType.Shoot);
        neededCombo.Add(InputType.MoveUp);
        neededCombo.Add(InputType.Shoot);
    }
    public override void Invoke()
    {
        base.Invoke();
        Player.StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        yield return null;
        Debug.Log("PSkill_TestRangeAtk ½ÇÇà");
        yield return null;
        End();
    }
}
