using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class TestRangeEnemy : Enemy
{
    public float engageDistance = 1;
    public float aimTIme = 1;
    public float shootTime = 0.15f;
    public BulletShooter shooter;
    public Mover mover => GetComponent<Mover>();
    public GameObject armBox;
    public float angle;

    protected override void Start()
    {
        base.Start();

        stateMachine = new(this);
        stateMachine.ChangeState<TestRangeEnemy_idle>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
    }

    public bool FindPlayerInEngage()
    {
        bool result = GameTools.IsAround(transform.position, GameManager.Player.transform.position, engageDistance);
        return result;
    }
}
