using System.Collections;
using UnityEngine;

public class PSkill_TestDash : PlayerSkill
{
    public PSkill_TestDash()
    {
        neededCombo = new()
        {
            InputType.Shoot,
            InputType.MoveUp,
            InputType.MoveDown
        };
    }

    private float dashDistance = 4f;
    private float dashSpeedRate = 5.0f;
    private float dashTime = 1f;

    public override void Invoke()
    {
        base.Invoke();
        Player.StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        yield return null;
        //float speed = Mathf.Max(0, Player.Speed) * dashSpeedRate * (Player.IsLookLeft ? -1 : 1) / TimeManager.SlowRate;
        //Player.MoverV.speedCap = false;
        //Player.MoverV.SetVelocityX(speed);
        Vector2 target = (Vector2)Player.transform.position + (dashDistance * (Player.IsLookLeft ? Vector2.left : Vector2.right));
        Player.MoverT.StartMove(MoverByTransform.moveType.LinearByPosWithTime, target, dashTime * TimeManager.SlowRate);

        Debug.Log("PSkill_TestDash ½ÇÇà");
        //yield return new WaitForSecondsRealtime(4f / Mathf.Abs(speed));

        yield return new WaitUntil(() => !Player.MoverT.IsMoving);

        //Player.MoverV.speedCap = true;
        //Player.MoverV.StopMove();

        End();
    }
}
