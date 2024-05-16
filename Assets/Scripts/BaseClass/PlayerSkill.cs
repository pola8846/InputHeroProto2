using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    protected PlayerUnit Player => GameManager.Player;
    protected List<InputType> neededCombo;
    public List<InputType> NeededCombo => neededCombo;

    //스킬 실행부
    public virtual void Invoke()
    {

    }
}
