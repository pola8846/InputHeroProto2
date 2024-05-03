using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1 : Enemy
{
    [SerializeField]
    private GameObject test;


    [SerializeField]
    private State state;

    private Mover moverV;
    private MoverByTransform moverT;
    private Rigidbody2D rb;
    private BulletShooter shooter;

    private float originGravity;

    //입력용 수치
    [Header("입력용")]
    [SerializeField]
    private float wait_MaxTime;//대기 최대 시간
    [SerializeField]
    private float move_MinDist;//이동 최소 거리
    [SerializeField]
    private float move_MaxTime;//이동 최대 시간
    [SerializeField]
    private float meleeAttack1CheckDistance;//근접공격을 위해 멈출 거리
    [SerializeField]
    private CollisionChecker meleeAttack1AreaChecker;

    [Header("공격 관련")]
    [SerializeField]
    private float anyAttackCooltime;

    [Header("근접공격")]
    [SerializeField]
    private GameObject meleeAttackObject;
    [SerializeField]
    private float meleeAttack1EWaitTime = .5f;
    [SerializeField]
    private float meleeAttack1Time = .1f;
    [SerializeField]
    private float meleeAttack1LWaitTime = .4f;

    [Header("원거리 점사")]
    [SerializeField]
    private float rangeAttack1EWaitTime = .3f;
    [SerializeField]
    private float rangeAttack1Time = .25f;
    [SerializeField]
    private int rangeAttack1RepeatCount = 4;
    [SerializeField]
    private float rangeAttack1LWaitTime = .3f;


    [Header("주변 탄 발사")]
    [SerializeField]
    private float barrageAttack2EWaitTime = 2f;
    [SerializeField]
    private float barrageAttack2AttackTime = 1.5f;
    [SerializeField]
    private int barrageAttack2BulletNum = 16;
    [SerializeField]
    private int barrageAttack2RepeatCount = 3;
    [SerializeField]
    private float barrageAttack2LWaitTime = 2f;
    [SerializeField]
    private float barrageAttack2Cooltime = 30f;



    //계산용 수치
    private GameObject meleeAttackGO;
    private int rangeAttack1Counter = 0;

    //맵 위치
    [Header("맵 위치")]
    [SerializeField]
    private Transform platformL;
    [SerializeField]
    private Transform platformR;
    private Transform targetPlatform;

    //색 변경
    private Renderer renderer;
    private Color originColor;

    //타이머
    private float lastAttackTime;
    private float stateTime;//현 상태에 진입한 시간
    private float tick;
    private float time_BAttack;


    //기본함수
    protected override void Start()
    {
        base.Start();
        moverV = gameObject.GetComponent<Mover>();
        moverT = gameObject.GetComponent<MoverByTransform>();
        renderer = gameObject.GetComponent<Renderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        shooter = gameObject.GetComponent<BulletShooter>();
        originGravity = rb.gravityScale;
        originColor = renderer.material.color;
        tick = Time.time;
    }

    protected override void Update()
    {
        PerformanceManager.StartTimer("TestEnemy_Boss_1.Update");

        base.Update();

        switch (state)
        {
            case State.Wait:

                if (TimeCheck(tick, 0.1f))
                {
                    tick = Time.time;

                    if (ChoiceAttack())
                    {
                        break;
                    }
                }
                if (TimeCheck(stateTime, wait_MaxTime))
                {
                    SetState(State.Move);
                }
                break;
            case State.Move:
                if (isLookLeft == (GameManager.Player.transform.position.x > transform.position.x + meleeAttack1CheckDistance * (isLookLeft ? -1 : 1)))
                {
                    SetState(State.Wait);
                }
                else if (TimeCheck(stateTime, move_MaxTime))
                {
                    SetState(State.Wait);
                }
                break;

            //근접
            case State.MeleeAttack1_EWait:
                if (TimeCheck(stateTime, meleeAttack1EWaitTime))
                {
                    SetState(State.MeleeAttack1_Attack);
                }
                break;
            case State.MeleeAttack1_Attack:
                if (TimeCheck(stateTime, meleeAttack1Time))
                {
                    SetState(State.MeleeAttack1_LWait);
                }
                break;
            case State.MeleeAttack1_LWait:
                if (TimeCheck(stateTime, meleeAttack1LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;

            //원거리
            case State.RangeAttack1_EWait:
                if (TimeCheck(stateTime, rangeAttack1EWaitTime))
                {
                    SetState(State.RangeAttack1_Attack);
                }
                break;
            case State.RangeAttack1_Attack:
                if (TimeCheck(stateTime, rangeAttack1Time))
                {
                    if (rangeAttack1Counter>=rangeAttack1RepeatCount)
                    {
                        SetState(State.RangeAttack1_LWait);
                    }
                    else
                    {
                        SetState(State.RangeAttack1_Attack);
                    }
                }
                break;
            case State.RangeAttack1_LWait:
                if (TimeCheck(stateTime, rangeAttack1LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;


            case State.BarrageAttack2_EWait:
                if (TimeCheck(stateTime, barrageAttack2EWaitTime))
                {
                    SetState(State.BarrageAttack2_Attack);
                }
                break;
            case State.BarrageAttack2_Attack:
                break;
            case State.BarrageAttack2_LWait:
                if (TimeCheck(stateTime, barrageAttack2LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;
        }
        PerformanceManager.StopTimer("TestEnemy_Boss_1.Update");
    }


    protected override void FixedUpdate()
    {
        PerformanceManager.StartTimer("TestEnemy_Boss_1.FixedUpdate");
        base.FixedUpdate();
        switch (state)
        {
            case State.Wait:
                break;
        }
        PerformanceManager.StopTimer("TestEnemy_Boss_1.FixedUpdate");

    }

    //상태 변경
    private void SetState(State st)
    {
        PerformanceManager.StartTimer("TestEnemy_Boss_1.SetState");
        ExitState(st);
        stateTime = Time.time;
        state = st;
        EnterState(st);
        PerformanceManager.StopTimer("TestEnemy_Boss_1.SetState");
    }

    private void EnterState(State st)
    {
        switch (st)
        {
            case State.Wait:
                SetColor(Color.clear);
                tick = Time.time;
                StopMove();
                break;
            case State.Move://플레이어랑 멀면 플레이어에게 이동
                MoveToPlayer();
                break;
                case State.MeleeAttack1_EWait:
                SetColor(Color.red * 0.8f);
                break;


            case State.MeleeAttack1_Attack:
                SetColor(Color.red);
                meleeAttackGO = Instantiate(meleeAttackObject, transform);
                meleeAttackGO.GetComponent<Attack>().Initialization(this, "Player", meleeAttackGO);
                Destroy(meleeAttackGO, meleeAttack1Time);
                break;


            case State.RangeAttack1_EWait:
                SetColor(Color.yellow*0.8f);
                rangeAttack1Counter = 0;
                shooter.shootType = BulletShootType.oneWay;
                shooter.BulletNum = 1;
                shooter.bulletSpeedMax = shooter.bulletSpeedMin = 8f;
                shooter.bulletDamageMax = shooter.bulletDamageMin = stats.attackPower; 
                break;

            case State.RangeAttack1_Attack:
                Vector2 temp = GetDist(GameManager.Player.transform.position + Vector3.up*0.5f);
                shooter.bulletAngleMax = shooter.bulletAngleMin =
                    Vector2.SignedAngle(Vector2.up, temp) * (isLookLeft? 1:-1);
                shooter.triger = true;
                rangeAttack1Counter++;
                break;

            case State.BarrageAttack2_EWait:
                shooter.shootType = BulletShootType.fan;
                shooter.BulletNum = barrageAttack2BulletNum;
                shooter.bulletAngleMax = 360;
                shooter.bulletAngleMin = 0;

                break;
            case State.BarrageAttack2_Attack:
                shooter.triger = true;
                break;
        }
    }

    private void ExitState(State st)
    {
        switch (st)
        {
            case State.Wait:
                break;
            case State.Move:
                StopMove();
                break;

            case State.MeleeAttack1_Attack:
                SetColor(Color.clear);
                break;
            case State.MeleeAttack1_LWait:
                lastAttackTime = Time.time;
                break;

            case State.RangeAttack1_EWait:
                SetColor(Color.yellow);
                break;
            case State.RangeAttack1_LWait:
                lastAttackTime = Time.time;
                break;
        }
    }

    //기타
    private void SetColor(Color color)
    {
        if (color == Color.clear)
        {
            renderer.material.color = originColor;

        }
        else
        {
            renderer.material.color = color;
        }
    }
    /// <summary>
    /// 공격 고르기. 안 했다면 false 반환
    /// </summary>
    /// <returns>공격 골랐는지</returns>
    private bool ChoiceAttack()
    {
        if (TimeCheck(lastAttackTime, anyAttackCooltime))
        {
            List<State> states = new();

            if (IsPlayerInMeleeAttack1Area())
            {
                states.Add(State.MeleeAttack1_EWait);
            }
            else
            {
                states.Add(State.RangeAttack1_EWait);
            }

            if (TimeCheck(time_BAttack, barrageAttack2Cooltime))
            {
                states.Add(State.BarrageAttack2_EWait);
            }

            if (states.Count >= 1)
            {
                int rand = Random.Range(0, states.Count);
                SetState(states[rand]);
                return true;
            }
        }

        return false;
    }

    //이동
    private void MoveToPlayer()
    {
        bool isPlayerLeft = (GameManager.Player.transform.position.x <= transform.position.x);
        if (isPlayerLeft != isLookLeft)
        {
            Turn();
        }

        //Vector2 targetPos = (Vector2)transform.position + move_MinDist * (isLookLeft ? Vector2.left : Vector2.right);
        float moveX = Stats.moveSpeed * (isLookLeft ? -1 : 1);
        moverV.SetVelocityX(moveX);
    }
    private void StopMove()
    {
        moverT.StopMove();
        moverV.StopMove();
        rb.gravityScale = originGravity;
    }
    private void MoveByTranceform(Vector2 target, float speedRate = 1f)
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        moverT.StartMove(MoverByTransform.moveType.LinearByPosWithSpeed, target, speedRate * Stats.moveSpeed);
    }


    //시간 체크
    private bool TimeCheck(float timer, float targetTime)
    {
        return timer + targetTime < Time.time;
    }

    //대상까지 방향벡터 가져오기
    private Vector2 GetDist(Vector2 target)
    {
        Vector2 temp = target - (Vector2)transform.position;
        return temp.normalized;
    }

    /// <summary>
    /// 플레이어가 근접공격 사거리 안에 있는지
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInMeleeAttack1Area()
    {
        var tempList = meleeAttack1AreaChecker.GetListOfClass<PlayerUnit>();
        if (tempList.Count >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private enum State
    {
        Wait,//그냥 대기
        Move,//플레이어에게 이동
        MeleeAttack1_EWait, MeleeAttack1_Attack, MeleeAttack1_LWait,//플레이어가 가까이 있으면 근접 공격
        RangeAttack1_EWait, RangeAttack1_Attack, RangeAttack1_LWait,//플레이어가 멀리 있으면 조준탄 발사
        AreaAttack1_EWait, AreaAttack1_Attack, AreaAttack1_LWait,//플레이어가 있는 발판에 광역 공격
        BarrageAttack1_EWait, BarrageAttack1_Attack, BarrageAttack1_LWait,//플랫폼 하나 정해서 이동 후 탄막 발사
        BarrageAttack2_EWait, BarrageAttack2_Attack, BarrageAttack2_LWait,//방사형 탄 발사
    }
}
