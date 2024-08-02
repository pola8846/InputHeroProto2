using System;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1 : Enemy
{
    [SerializeField]
    private State state;//임시

    private BulletShooter shooter;//총알 발사기
    public BulletShooter Shooter => shooter;

    [Header("입력용")]
    [SerializeField]
    private float wait_MaxTime;//대기 최대 시간
    public float Wait_MaxTime => wait_MaxTime;
    [SerializeField]
    private float move_MinDist;//이동 최소 거리
    [SerializeField]
    private float move_MaxTime;//이동 최대 시간
    public float Move_MaxTime => move_MaxTime;
    [SerializeField]
    private float meleeAttack1CheckDistance;//근접공격을 위해 멈출 거리
    public float MeleeAttack1CheckDistance => meleeAttack1CheckDistance;
    [SerializeField]
    private CollisionChecker meleeAttack1AreaChecker;//근접 공격 범위 체크용 에리어

    [Header("공격 관련")]
    [SerializeField]
    private float anyAttackCooltime;//모든 공격 공용 쿨타임

    [Header("근접공격")]
    [SerializeField]
    private AttackState melee;
    [SerializeField]
    private GameObject meleeAttackObject;
    public GameObject MeleeAttackObject => meleeAttackObject;

    [Header("원거리 점사")]
    [SerializeField]
    private AttackState range;
    [SerializeField]
    private int rangeAttack1RepeatCount = 4;

    [Header("광역 공격")]
    [SerializeField]
    private AttackState area;
    [SerializeField]
    private float areaAttack1Cooltime = 45f;
    [SerializeField]
    private GameObject areaAttackObjectL;
    public GameObject AreaAttackObjectL => areaAttackObjectL;
    [SerializeField]
    private GameObject areaAttackObjectR;
    public GameObject AreaAttackObjectR => areaAttackObjectR;
    [SerializeField]
    private GameObject areaAttackObjectD;
    public GameObject AreaAttackObjectD => areaAttackObjectD;


    [Header("주변 탄 발사")]
    [SerializeField]
    private AttackState barrage1;
    [SerializeField]
    private int barrageAttack1BulletNum = 8;
    public int BarrageAttack1BulletNum => barrageAttack1BulletNum;
    [SerializeField]
    private float barrageAttack1Angle = 45f;
    public float BarrageAttack1Angle => barrageAttack1Angle;
    [SerializeField]
    private float barrageAttack1Cooltime = 60f;


    [Header("주변 탄 발사")]
    [SerializeField]
    private AttackState barrage2;
    [SerializeField]
    private int barrageAttack2BulletNum = 16;
    public int BarrageAttack2BulletNum => barrageAttack2BulletNum;
    [SerializeField]
    private float barrageAttack2Cooltime = 30f;



    //계산용 수치
    private GameObject meleeAttackGO;
    private GameObject areaAttackGO;
    private int rangeAttack1Counter = 0;
    private int barrageAttack1Counter = 0;
    private int barrageAttack2Counter = 0;

    //맵 위치
    [Header("맵 위치")]
    [SerializeField]
    private Transform platformL;
    public Transform PlatformL => platformL;//왼쪽 발판
    [SerializeField]
    private CollisionChecker collisionCheckerL;
    [SerializeField]
    private Transform platformR;
    public Transform PlatformR => platformR;//오른쪽 발판
    [SerializeField]
    private CollisionChecker collisionCheckerR;
    [SerializeField]
    private Transform platformD;
    public Transform PlatformD => platformD;//아래 발판
    [SerializeField]
    private CollisionChecker collisionCheckerD;
    public Transform targetPlatform;

    //색 변경
    private new Renderer renderer;
    private Color originColor;

    //타이머
    private TickTimer lastAttackTime;
    private TickTimer stateTime;//현 상태에 진입한 시간
    private TickTimer tick;
    private TickTimer time_BAttack;
    private TickTimer time_BAttack2;
    private TickTimer time_AreaAttack;


    //기본함수
    protected override void Start()
    {
        base.Start();
        renderer = gameObject.GetComponent<Renderer>();
        shooter = gameObject.GetComponent<BulletShooter>();
        originColor = renderer.material.color;

        lastAttackTime = new();
        stateTime = new();//현 상태에 진입한 시간
        tick = new(checkTime: 0.1f, autoReset: true);
        time_BAttack = new(isTrigerInstant: true);
        time_BAttack2 = new(isTrigerInstant: true);
        time_AreaAttack = new(isTrigerInstant: true);

        stateMachine.ChangeState<TestEnemy_Boss_1_Idle>();
    }

    protected override void Update()
    {

        base.Update();

        switch (state)
        {
            case State.Wait:

                if (tick.Check())
                {
                    if (ChoiceAttack())
                    {
                        break;
                    }
                }
                if (stateTime.Check(wait_MaxTime))
                {
                    SetState(State.Move);
                }
                break;
            case State.Move:
                if (IsPlayerInMeleeAttack1Area())
                {
                    SetState(State.MeleeAttack1_EWait);
                }
                else if (isLookLeft == (GameManager.Player.transform.position.x > 
                    transform.position.x + meleeAttack1CheckDistance * (isLookLeft ? -1 : 1)))
                {
                    SetState(State.Wait);
                }
                else if (stateTime.Check(move_MaxTime))
                {
                    SetState(State.Wait);
                }
                break;

            //근접
            case State.MeleeAttack1_EWait:
                if (stateTime.Check(melee.waitTime_Early))
                {
                    SetState(State.MeleeAttack1_Attack);
                }
                break;
            case State.MeleeAttack1_Attack:
                if (stateTime.Check(melee.waitTime_Attack))
                {
                    SetState(State.MeleeAttack1_LWait);
                }
                break;
            case State.MeleeAttack1_LWait:
                if (stateTime.Check(melee.waitTime_Late))
                {
                    SetState(State.Wait);
                }
                break;

            //원거리
            case State.RangeAttack1_EWait:
                if (stateTime.Check(range.waitTime_Early))
                {
                    SetState(State.RangeAttack1_Attack);
                }
                break;
            case State.RangeAttack1_Attack:
                if (stateTime.Check(range.waitTime_Attack))
                {
                    if (rangeAttack1Counter >= rangeAttack1RepeatCount)
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
                if (stateTime.Check(range.waitTime_Late))
                {
                    SetState(State.Wait);
                }
                break;

            //광역
            case State.AreaAttack1_EWait:
                if (stateTime.Check(area.waitTime_Early))
                {
                    SetState(State.AreaAttack1_Attack);
                }
                break;
            case State.AreaAttack1_Attack:
                if (stateTime.Check(area.waitTime_Attack))
                {
                    SetState(State.AreaAttack1_LWait);
                }
                break;
            case State.AreaAttack1_LWait:
                if (stateTime.Check(area.waitTime_Late))
                {
                    SetState(State.Wait);
                }
                break;

            //탄막1
            case State.BarrageAttack1_EWait:
                if (stateTime.Check(barrage1.waitTime_Early))
                {
                    SetState(State.BarrageAttack1_Attack);
                }
                break;
            case State.BarrageAttack1_Attack:
                if (stateTime.Check(barrage1.waitTime_Attack))
                {
                    if (barrageAttack1Counter >= barrage1.repeatCount)
                    {
                        SetState(State.BarrageAttack1_LWait);
                    }
                    else
                    {
                        SetState(State.BarrageAttack1_Attack);
                    }
                }
                break;
            case State.BarrageAttack1_LWait:
                if (stateTime.Check(barrage1.waitTime_Late))
                {
                    SetState(State.Wait);
                }
                break;

            //탄막2
            case State.BarrageAttack2_EWait:
                if (stateTime.Check(barrage2.waitTime_Early))
                {
                    SetState(State.BarrageAttack2_Attack);
                }
                break;
            case State.BarrageAttack2_Attack:
                if (stateTime.Check(barrage2.waitTime_Attack))
                {
                    if (barrageAttack2Counter >= barrage2.repeatCount)
                    {
                        SetState(State.BarrageAttack2_LWait);
                    }
                    else
                    {
                        SetState(State.BarrageAttack2_Attack);
                    }
                }
                break;
            case State.BarrageAttack2_LWait:
                if (stateTime.Check(barrage2.waitTime_Late))
                {
                    SetState(State.Wait);
                }
                break;
        }
    }

    //상태 변경
    private void SetState(State st)
    {
        //Debug.Log($"{state.ToString()} 퇴장");
        ExitState(state);
        stateTime.Reset();
        state = st;
        //Debug.Log($"{state.ToString()} 진입");
        EnterState(state);
    }

    //상태 진입 시 처리
    private void EnterState(State st)
    {
        switch (st)
        {
            case State.Wait:
                SetColor(Color.clear);
                tick.Reset();
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
                Destroy(meleeAttackGO, melee.waitTime_Attack);
                break;

            case State.RangeAttack1_EWait:
                SetColor(Color.yellow * 0.8f);
                rangeAttack1Counter = 0;
                shooter.shootType = ShootType.oneWay;
                shooter.BulletNum = 1;
                shooter.BulletSpeed = 25f;
                //shooter.bulletDamageMax = shooter.bulletDamageMin = stats.attackPower;
                break;

            case State.RangeAttack1_Attack:
                {
                    ShootToPlayer();
                    rangeAttack1Counter++;
                }
                break;


            case State.AreaAttack1_EWait:
                {
                    SetColor(Color.red);
                }
                break;

            case State.AreaAttack1_Attack:
                {
                    if (ReferenceEquals(targetPlatform, platformD))
                    {
                        areaAttackGO = Instantiate(areaAttackObjectD);
                        areaAttackGO.GetComponent<Attack>().Initialization(this, "Player", areaAttackGO);
                    }
                    else if (ReferenceEquals(targetPlatform, platformL))
                    {
                        areaAttackGO = Instantiate(areaAttackObjectL);
                        areaAttackGO.GetComponent<Attack>().Initialization(this, "Player", areaAttackGO);
                    }
                    else if (ReferenceEquals(targetPlatform, platformR))
                    {
                        areaAttackGO = Instantiate(areaAttackObjectR);
                        areaAttackGO.GetComponent<Attack>().Initialization(this, "Player", areaAttackGO);
                    }
                    Destroy(areaAttackGO, area.waitTime_Attack);

                    time_AreaAttack.Reset();
                }
                break;

            case State.BarrageAttack1_EWait:
                //Debug.Log($"{targetPlatform.transform.position}, {targetPlatform}");
                transform.position = targetPlatform.transform.position + 
                    (Vector3.up * targetPlatform.lossyScale.y * 0.5f) + (Vector3.up * 2f);
                shooter.shootType = ShootType.fan;
                shooter.BulletNum = barrageAttack1BulletNum;
                shooter.BulletSpeed = 6f;
                moverV.SetVelocity(new Vector2(0, 0.2f));
                break;

            case State.BarrageAttack1_Attack:
                ShootToPlayer(barrageAttack1Angle);
                barrageAttack1Counter++;
                time_BAttack2.Reset();
                break;
            case State.BarrageAttack1_LWait:
                StopMove();
                break;

            case State.BarrageAttack2_EWait:
                {
                    SetColor(Color.cyan);
                    shooter.shootType = ShootType.fan;
                    shooter.BulletNum = barrageAttack2BulletNum;
                    shooter.bulletAngleMax = 360;
                    shooter.bulletAngleMin = 0;
                    shooter.BulletSpeed = 3f;
                    barrageAttack2Counter = 0;
                }
                break;
            case State.BarrageAttack2_Attack:
                {
                    float temp = (360 / barrageAttack2BulletNum) / 2;
                    shooter.bulletAngleMax += temp;
                    shooter.bulletAngleMin += temp;
                    time_BAttack.Reset();
                    barrageAttack2Counter++;
                    shooter.Triger();
                }
                break;
        }
    }

    //상태 종료 시 처리
    private void ExitState(State st)
    {
        switch (st)
        {
            case State.Move:
                StopMove();
                break;

            case State.MeleeAttack1_Attack:
                SetColor(Color.clear);
                break;
            case State.MeleeAttack1_LWait:
                lastAttackTime.Reset();
                break;

            case State.RangeAttack1_EWait:
                SetColor(Color.yellow);
                break;
            case State.RangeAttack1_LWait:
                lastAttackTime.Reset();
                break;
            case State.AreaAttack1_EWait:
                {
                    SetColor(Color.clear);
                }
                break;

            case State.BarrageAttack2_LWait:
                lastAttackTime.Reset();
                break;
        }
    }

    //기타
    //색깔설정(애니메이션 대신)
    public void SetColor(Color color)
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
    /// 가능한 공격 중에서 랜덤으로 고르기. 안 했다면 false 반환
    /// </summary>
    /// <returns>공격 골랐는지</returns>
    private bool ChoiceAttack()
    {
        if (lastAttackTime.Check(anyAttackCooltime))
        {
            List<State> states = new();

            if (IsPlayerInMeleeAttack1Area())
            {
                //근거리
                states.Add(State.MeleeAttack1_EWait);
            }
            else
            {
                //원거리
                states.Add(State.RangeAttack1_EWait);
            }

            //발판 이동 후 부채꼴 탄
            if (time_BAttack2.Check(barrageAttack1Cooltime))
            {
                states.Add(State.BarrageAttack1_EWait);
            }

            //전방위 탄
            if (time_BAttack.Check(barrageAttack2Cooltime))
            {
                states.Add(State.BarrageAttack2_EWait);
            }

            //서있는 발판 광역
            if (time_AreaAttack.Check(areaAttack1Cooltime))
            {
                if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                {
                    states.Add(State.AreaAttack1_EWait);
                }
            }


            //무작위 선정
            if (states.Count >= 1)
            {
                int rand = UnityEngine.Random.Range(0, states.Count);
                var result = states[rand];

                switch (result)
                {
                    case State.BarrageAttack1_EWait:
                        targetPlatform = UnityEngine.Random.Range(0, 2) == 0 ? platformL : platformR;
                        break;

                    case State.AreaAttack1_EWait:
                        if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1)
                        {
                            targetPlatform = platformD;
                        }
                        else if (collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1)
                        {
                            targetPlatform = platformL;
                        }
                        else if (collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                        {
                            targetPlatform = platformR;
                        }
                        break;
                }
                SetState(states[rand]);
                return true;
            }
        }
        return false;
    }

    //State 작성 도중 만들었던 임시 코드
    public Type ChoiceAttackState()
    {
        if (lastAttackTime.Check(anyAttackCooltime))
        {
            List<Type> states = new();

            if (IsPlayerInMeleeAttack1Area())
            {
                //근거리
                states.Add(typeof(TestEnemy_Boss_1_AtkM1));
            }
            else
            {
                //원거리
                states.Add(typeof(TestEnemy_Boss_1_AtkR1));
            }

            //발판 이동 후 부채꼴 탄
            if (time_BAttack2.Check(barrageAttack1Cooltime))
            {
                states.Add(typeof(TestEnemy_Boss_1_AtkB1));
            }

            //전방위 탄
            if (time_BAttack.Check(barrageAttack2Cooltime))
            {
                states.Add(typeof(TestEnemy_Boss_1_AtkB2));
            }

            //서있는 발판 광역
            if (time_AreaAttack.Check(areaAttack1Cooltime))
            {
                if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                {
                    states.Add(typeof(TestEnemy_Boss_1_AtkA1));
                }
            }



            if (states.Count >= 1)
            {
                int rand = UnityEngine.Random.Range(0, states.Count);
                var result = states[rand];

                if (result == typeof(TestEnemy_Boss_1_AtkB1))
                {
                    targetPlatform = UnityEngine.Random.Range(0, 2) == 0 ? platformL : platformR;
                }
                else if (result == typeof(TestEnemy_Boss_1_AtkA1))
                {
                    if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1)
                    {
                        targetPlatform = platformD;
                    }
                    else if (collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1)
                    {
                        targetPlatform = platformL;
                    }
                    else if (collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                    {
                        targetPlatform = platformR;
                    }
                }
                return result;
            }
        }
        return null;
    }



    //이동
    public void MoveToPlayer()
    {
        bool isPlayerLeft = (GameManager.Player.transform.position.x <= transform.position.x);
        if (isPlayerLeft != isLookLeft)
        {
            Turn();
        }

        float moveX = Stats.moveSpeed * (isLookLeft ? -1 : 1);
        moverV.SetVelocityX(moveX);
    }
    //정지
    public void StopMove()
    {
        moverT.StopMove();
        moverV.StopMove();
    }
    //플레이어 방향으로 발사
    public void ShootToPlayer(float angleRange = 0f)
    {
        float temp2 = GameTools.GetDegreeAngleFormDirection
            (GameManager.Player.transform.position + Vector3.up * 0.5f - transform.position);
        shooter.bulletAngleMax = temp2 + angleRange;
        shooter.bulletAngleMin = temp2 - angleRange;
        shooter.Triger();
    }

    /// <summary>
    /// 플레이어가 근접공격 사거리 안에 있는지
    /// </summary>
    private bool IsPlayerInMeleeAttack1Area()
    {
        var tempList = meleeAttack1AreaChecker.GetListOfClass<HitBox>();
        foreach (HitBox hitBox in tempList)
        {
            if (hitBox.Unit == GameManager.Player)
            {
                return true;
            }
        }
        return false;
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
