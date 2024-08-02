using UnityEngine;

[RequireComponent(typeof(Mover))]
public class TestRangeEnemy : Enemy
{
    public float engageDistance = 1;
    public float engageSpeedRate = 0.75f;
    public float aimTIme = 1;
    public float shootTime = 0.15f;
    public BulletShooter shooter;
    public Mover mover => GetComponent<Mover>();
    public GameObject armBox;
    public float angle;
    [Range(0f, 45f)]
    public float angleRange;

    public TestRangeEnemy_Animation_Top _Top;
    public TestRangeEnemy_Animation_Bottom _Bottom;

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
        if (GameManager.Player == null)
            return false;
        bool result = GameTools.IsAround
            (transform.position, GameManager.Player.transform.position, engageDistance);
        return result;
    }
}
