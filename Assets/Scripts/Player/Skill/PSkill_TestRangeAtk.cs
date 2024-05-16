using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkill_TestRangeAtk : PlayerSkill
{
    public PSkill_TestRangeAtk()
    {
        neededCombo = new();
        neededCombo.Add(InputType.Shoot);
        neededCombo.Add(InputType.MoveRight);
    }
}
