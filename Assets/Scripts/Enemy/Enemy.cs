using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected bool isFindPlayer = false;
    [SerializeField]
    protected float findDistance = 5f;

    protected bool FindPlayer()
    {
        return Vector3.Distance(transform.position, GameManager.Player.transform.position) <= findDistance;
    }
}