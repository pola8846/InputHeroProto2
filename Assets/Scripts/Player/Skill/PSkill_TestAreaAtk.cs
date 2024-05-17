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
}
