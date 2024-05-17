using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    protected PlayerUnit Player => GameManager.Player;
    protected List<InputType> neededCombo;
    public List<InputType> NeededCombo => neededCombo;

    private static bool isUsing = false;
    public static bool IsUsing => isUsing;

    //스킬 실행부
    public virtual void Invoke()
    {
        isUsing = true;
    }

    //종료 알림
    public virtual void End()
    {
        isUsing = false;
    }
}
