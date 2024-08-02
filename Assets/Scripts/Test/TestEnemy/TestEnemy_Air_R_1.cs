using UnityEngine;

[RequireComponent(typeof(MoverByTransform))]
public class TestEnemy_Air_R_1 : Enemy
{
    [SerializeField]
    private float patrolSpeedRate = 1f;
    [SerializeField]
    private float patrolDistance;
    [SerializeField]
    private float patrolTick = 1f;
    [SerializeField]
    private float patrolRange = 5f;//첫위치에서 얼마나 순찰할건지
    private Vector2 patrolDist;
    private Vector2 patrolOrigin;
    [SerializeField]
    private float attackRange2 = 5f;
    [SerializeField]
    private float attackRange3 = 1f;
    [SerializeField, Range(0f, 90f)]
    private float attackRangeAngle = 45f;
    [SerializeField, Range(1, 10)]
    private int attackMoveCount = 3;
    private int attackMoveCounter = 0;
    [SerializeField, Range(1, 20)]
    private int attackCount = 2;
    private int attackCounter = 0;
    [SerializeField]
    private float attackMoveSpeedRate = 4f;
    [SerializeField]
    private float attackMoveWaitTime = 0.2f;
    [SerializeField]
    private float attackDelay = .35f;
    [SerializeField]
    private float attackWaitTimeBeforeAttack = 1f;
    [SerializeField]
    private float attackWaitTimeAfterAttack = 2f;
    private Vector2 attackPosOrigin;
    private Vector2 attackPos;
    private BulletShooter shooter;
    [SerializeField]
    private int state = 0;
    private Renderer renderer;
    private Color originColor;

    private float timer1;
    private float timer2;
    private float timer3;
    private float timer4;

    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        shooter = GetComponent<BulletShooter>();
        originColor = renderer.material.color;
        timer1 = Time.time;
        timer2 = Time.time;
        timer3 = Time.time;
        timer4 = Time.time;
        patrolDist = transform.position;
        patrolOrigin = transform.position;
    }
    protected override void Update()
    {
        base.Update();
        if (state >= 0 && state <= 2 && FindPlayer())//플레이어 발견 시
        {
            SetState(3);
            return;
        }
        switch (state)
        {
            case 0://순찰 대기
                if (timer1 + patrolTick <= Time.time)
                {
                    SetState(1);
                }
                break;
            case 1://순찰 이동
                //if (Vector2.Distance(transform.position, (Vector3)patrolDist) <= .1f)
                if (!moverT.IsMoving)
                {
                    SetState(0);
                }
                break;
            case 2://순찰 복귀
                if (!moverT.IsMoving)
                //if (Vector2.Distance(transform.position, (Vector3)patrolDist) <= .1f)
                {
                    SetState(0);
                }
                break;
            case 3://플레이어에게 접근
                if (!moverT.IsMoving)
                //if (Vector2.Distance(transform.position, (Vector3)attackPos) <= .1f)
                {
                    SetState(4);
                }
                break;
            case 4://공격 영역 내에서 재이동
                if (!moverT.IsMoving)
                //if (Vector2.Distance(transform.position, (Vector3)attackPos) <= .1f)
                {
                    if (attackMoveCounter >= attackMoveCount)
                    {
                        SetState(6);
                    }
                    else
                    {
                        SetState(5);
                    }
                }
                break;
            case 5://이동 도중 대기
                if (timer2 + attackMoveWaitTime <= Time.time)
                {
                    SetState(4);
                }
                break;
            case 6://공격 이전 대기
                if (timer3 + attackWaitTimeBeforeAttack <= Time.time)
                {
                    SetState(7);
                }
                break;
            case 7:
                if (attackCounter >= attackCount)
                {
                    if (timer4 + attackWaitTimeAfterAttack <= Time.time)
                    {
                        if (FindPlayer())
                        {
                            SetState(3);
                        }
                        else
                        {
                            SetState(2);
                        }
                    }
                }
                else
                {
                    if (timer4 + attackDelay <= Time.time)
                    {
                        SetState(7);
                    }
                }
                break;
            default:
                break;
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();


        var player = GameManager.Player;

        switch (state)
        {
            case 1:
                {

                }
                break;
            case 2:
                {
                }
                break;
            case 3:
                if (Vector2.Distance(GameManager.Player.transform.position, transform.position) <= attackRange2)
                {
                    moverT.StopMove();
                }
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private void SetState(int num)
    {
        switch (state)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                moverT.StopMove();
                break;
            case 4:
                attackMoveCounter++;
                moverT.StopMove();
                break;
            case 6:
                attackCounter = 0;
                break;
            case 7:
                attackCounter++;
                break;
            default:
                break;
        }

        state = num;

        switch (state)
        {
            case 0:
                moverT.StopMove();
                timer1 = Time.time;
                break;
            case 1:
                {
                    SetRandomPatrolPosition();
                    float speed = Mathf.Max(0, Speed) * patrolSpeedRate;
                    MoveTo(patrolDist, speed);
                }
                break;
            case 2:
                {
                    patrolDist = patrolOrigin;
                    float speed = Mathf.Max(0, Speed);
                    MoveTo(patrolDist, speed);
                }
                break;
            case 3:
                {
                    SetAttackPosOrigin();
                    SetAttackPos();
                    attackMoveCounter = 0;
                    float speed = Mathf.Max(0, Speed);
                    MoveTo(patrolDist, speed);
                }
                break;
            case 4:
                {
                    SetAttackPos();
                    float speed = Mathf.Max(0, Speed) * attackMoveSpeedRate;
                    MoveTo(attackPos, speed);
                }
                break;
            case 5:
                moverT.StopMove();
                timer2 = Time.time;
                break;
            case 6:
                timer3 = Time.time;
                break;
            case 7:
                timer4 = Time.time;
                //공격
                Vector2 temp = GetDist(GameManager.Player.transform.position);
                shooter.BulletAngle = Vector2.SignedAngle(Vector2.up, temp);
                shooter.Triger();
                break;
            default:
                break;
        }
    }

    private Vector2 GetDist(Vector2 target)//대상까지 방향벡터 가져오기
    {
        Vector2 temp = target - (Vector2)transform.position;
        return temp.normalized;
    }

    private void SetRandomPatrolPosition()
    {
        float randomVelue = Random.Range(-patrolRange, patrolRange);
        float targetX = patrolOrigin.x + randomVelue;
        patrolDist = new Vector2(targetX, transform.position.y);
    }

    private void SetAttackPosOrigin()
    {
        Vector3 playerPos = GameManager.Player.transform.position;
        Vector3 direction = Quaternion.AngleAxis(Random.Range(-attackRangeAngle, attackRangeAngle), Vector3.forward) * Vector3.up;
        attackPosOrigin = playerPos + (direction * attackRange2);
    }
    private void SetAttackPos()
    {
        float x = Random.Range(-attackRange3, attackRange3);
        float y = Random.Range(-attackRange3, attackRange3);
        attackPos = new Vector2(attackPosOrigin.x + x, attackPosOrigin.y + y);
    }

    private void MoveTo(Vector2 targetPos, float speed)
    {
        moverT.StartMove(MoverByTransform.moveType.LinearByPosWithSpeed, targetPos, speed);
    }
}
