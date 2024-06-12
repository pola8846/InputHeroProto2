using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Unit
{
    protected bool isFindPlayer = false;
    [SerializeField]
    protected float findDistance = 5f;

    protected bool FindPlayer()
    {
        bool result = GameTools.IsAround(transform.position, GameManager.Player.transform.position, findDistance);
        return result;
    }
}

[Serializable]
public struct AttackState
{
    public string name;
    public float waitTime_Early;
    public float waitTime_Attack;
    public float waitTime_Late;
    public int repeatCount;
    public int counter;
    private AttackStateEnum nowState;

    public AttackState(string name, float waitTime_Early = 1f, float waitTime_Attack = 1f, float waitTime_Late = 1f, int repeatCount = 1)
    {
        this.name = name;
        this.waitTime_Early = waitTime_Early;
        this.waitTime_Attack = waitTime_Attack;
        this.waitTime_Late = waitTime_Late;
        this.repeatCount = repeatCount;
        counter = 0;
        nowState = AttackStateEnum.NotCalled;
    }

    public string StateName_Early
    {
        get
        {
            return name + "_Early";
        }
    }

    public string StateName_Attack
    {
        get
        {
            return name + "_Attack";
        }
    }

    public string StateName_Late
    {
        get
        {
            return name + "_Late";
        }
    }

    public string NowState
    {
        get
        {
            string result;

            switch (nowState)
            {
                case AttackStateEnum.EarlyWait:
                    result = StateName_Early;
                    break;
                case AttackStateEnum.Attack:
                    result = StateName_Attack;
                    break;
                case AttackStateEnum.LateWait:
                    result = StateName_Late;
                    break;
                default:
                    result = "default";
                    break;
            }
            return result;
        }
    }

    private enum AttackStateEnum
    {
        NotCalled,
        EarlyWait,
        Attack,
        LateWait
    }
}