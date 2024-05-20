using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSkill_TestRangeAtk : PlayerSkill
{
    public PSkill_TestRangeAtk()
    {
        neededCombo = new()
        {
            InputType.Shoot,
            InputType.MoveUp,
            InputType.Shoot
        };
    }
    public override void Invoke()
    {
        base.Invoke();
        Player.StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        Debug.Log("PSkill_TestRangeAtk ½ÇÇà");
        yield return new WaitForSecondsRealtime(.15f);

        Vector2 dir = Player.IsLookLeft? Vector2.left : Vector2.right;
        float angle = Vector2.SignedAngle(dir, Vector2.up) * -1;

        Player.Shooter_Big.BulletAngle = angle;
        Player.Shooter_Big.Triger();

        yield return new WaitForSecondsRealtime(.6f);
        End();
    }
}
