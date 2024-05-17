using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkill_TestDash : PlayerSkill
{
    public PSkill_TestDash()
    {
        neededCombo = new();
        neededCombo.Add(InputType.Shoot);
        neededCombo.Add(InputType.MoveUp);
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
        Debug.Log("PSkill_TestDash ½ÇÇà");
        yield return null;
        End();
    }
}
