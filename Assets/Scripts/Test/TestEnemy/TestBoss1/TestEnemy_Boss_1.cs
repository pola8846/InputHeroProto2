using System;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1 : Enemy
{
    [SerializeField]
    private State state;//�ӽ�

    private BulletShooter shooter;//�Ѿ� �߻��
    public BulletShooter Shooter => shooter;

    [Header("�Է¿�")]
    [SerializeField]
    private float wait_MaxTime;//��� �ִ� �ð�
    public float Wait_MaxTime => wait_MaxTime;
    [SerializeField]
    private float move_MinDist;//�̵� �ּ� �Ÿ�
    [SerializeField]
    private float move_MaxTime;//�̵� �ִ� �ð�
    public float Move_MaxTime => move_MaxTime;
    [SerializeField]
    private float meleeAttack1CheckDistance;//���������� ���� ���� �Ÿ�
    public float MeleeAttack1CheckDistance => meleeAttack1CheckDistance;
    [SerializeField]
    private CollisionChecker meleeAttack1AreaChecker;//���� ���� ���� üũ�� ������

    [Header("���� ����")]
    [SerializeField]
    private float anyAttackCooltime;//��� ���� ���� ��Ÿ��

    [Header("��������")]
    [SerializeField]
    private AttackState melee;
    [SerializeField]
    private GameObject meleeAttackObject;
    public GameObject MeleeAttackObject => meleeAttackObject;

    [Header("���Ÿ� ����")]
    [SerializeField]
    private AttackState range;
    [SerializeField]
    private int rangeAttack1RepeatCount = 4;

    [Header("���� ����")]
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


    [Header("�ֺ� ź �߻�")]
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


    [Header("�ֺ� ź �߻�")]
    [SerializeField]
    private AttackState barrage2;
    [SerializeField]
    private int barrageAttack2BulletNum = 16;
    public int BarrageAttack2BulletNum => barrageAttack2BulletNum;
    [SerializeField]
    private float barrageAttack2Cooltime = 30f;



    //���� ��ġ
    private GameObject meleeAttackGO;
    private GameObject areaAttackGO;
    private int rangeAttack1Counter = 0;
    private int barrageAttack1Counter = 0;
    private int barrageAttack2Counter = 0;

    //�� ��ġ
    [Header("�� ��ġ")]
    [SerializeField]
    private Transform platformL;
    public Transform PlatformL => platformL;//���� ����
    [SerializeField]
    private CollisionChecker collisionCheckerL;
    [SerializeField]
    private Transform platformR;
    public Transform PlatformR => platformR;//������ ����
    [SerializeField]
    private CollisionChecker collisionCheckerR;
    [SerializeField]
    private Transform platformD;
    public Transform PlatformD => platformD;//�Ʒ� ����
    [SerializeField]
    private CollisionChecker collisionCheckerD;
    public Transform targetPlatform;

    //�� ����
    private new Renderer renderer;
    private Color originColor;

    //Ÿ�̸�
    private TickTimer lastAttackTime;
    private TickTimer stateTime;//�� ���¿� ������ �ð�
    private TickTimer tick;
    private TickTimer time_BAttack;
    private TickTimer time_BAttack2;
    private TickTimer time_AreaAttack;


    //�⺻�Լ�
    protected override void Start()
    {
        base.Start();
        renderer = gameObject.GetComponent<Renderer>();
        shooter = gameObject.GetComponent<BulletShooter>();
        originColor = renderer.material.color;

        lastAttackTime = new();
        stateTime = new();//�� ���¿� ������ �ð�
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

            //����
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

            //���Ÿ�
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

            //����
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

            //ź��1
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

            //ź��2
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

    //���� ����
    private void SetState(State st)
    {
        //Debug.Log($"{state.ToString()} ����");
        ExitState(state);
        stateTime.Reset();
        state = st;
        //Debug.Log($"{state.ToString()} ����");
        EnterState(state);
    }

    //���� ���� �� ó��
    private void EnterState(State st)
    {
        switch (st)
        {
            case State.Wait:
                SetColor(Color.clear);
                tick.Reset();
                StopMove();
                break;
            case State.Move://�÷��̾�� �ָ� �÷��̾�� �̵�
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
                shooter.shootType = ShootType.OneWay;
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
                shooter.shootType = ShootType.Fan;
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
                    shooter.shootType = ShootType.Fan;
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

    //���� ���� �� ó��
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

    //��Ÿ
    //������(�ִϸ��̼� ���)
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
    /// ������ ���� �߿��� �������� ����. �� �ߴٸ� false ��ȯ
    /// </summary>
    /// <returns>���� �������</returns>
    private bool ChoiceAttack()
    {
        if (lastAttackTime.Check(anyAttackCooltime))
        {
            List<State> states = new();

            if (IsPlayerInMeleeAttack1Area())
            {
                //�ٰŸ�
                states.Add(State.MeleeAttack1_EWait);
            }
            else
            {
                //���Ÿ�
                states.Add(State.RangeAttack1_EWait);
            }

            //���� �̵� �� ��ä�� ź
            if (time_BAttack2.Check(barrageAttack1Cooltime))
            {
                states.Add(State.BarrageAttack1_EWait);
            }

            //������ ź
            if (time_BAttack.Check(barrageAttack2Cooltime))
            {
                states.Add(State.BarrageAttack2_EWait);
            }

            //���ִ� ���� ����
            if (time_AreaAttack.Check(areaAttack1Cooltime))
            {
                if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                {
                    states.Add(State.AreaAttack1_EWait);
                }
            }


            //������ ����
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

    //State �ۼ� ���� ������� �ӽ� �ڵ�
    public Type ChoiceAttackState()
    {
        if (lastAttackTime.Check(anyAttackCooltime))
        {
            List<Type> states = new();

            if (IsPlayerInMeleeAttack1Area())
            {
                //�ٰŸ�
                states.Add(typeof(TestEnemy_Boss_1_AtkM1));
            }
            else
            {
                //���Ÿ�
                states.Add(typeof(TestEnemy_Boss_1_AtkR1));
            }

            //���� �̵� �� ��ä�� ź
            if (time_BAttack2.Check(barrageAttack1Cooltime))
            {
                states.Add(typeof(TestEnemy_Boss_1_AtkB1));
            }

            //������ ź
            if (time_BAttack.Check(barrageAttack2Cooltime))
            {
                states.Add(typeof(TestEnemy_Boss_1_AtkB2));
            }

            //���ִ� ���� ����
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



    //�̵�
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
    //����
    public void StopMove()
    {
        moverT.StopMove();
        moverV.StopMove();
    }
    //�÷��̾� �������� �߻�
    public void ShootToPlayer(float angleRange = 0f)
    {
        float temp2 = GameTools.GetDegreeAngleFormDirection
            (GameManager.Player.transform.position + Vector3.up * 0.5f - transform.position);
        shooter.bulletAngleMax = temp2 + angleRange;
        shooter.bulletAngleMin = temp2 - angleRange;
        shooter.Triger();
    }

    /// <summary>
    /// �÷��̾ �������� ��Ÿ� �ȿ� �ִ���
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
        Wait,//�׳� ���
        Move,//�÷��̾�� �̵�
        MeleeAttack1_EWait, MeleeAttack1_Attack, MeleeAttack1_LWait,//�÷��̾ ������ ������ ���� ����
        RangeAttack1_EWait, RangeAttack1_Attack, RangeAttack1_LWait,//�÷��̾ �ָ� ������ ����ź �߻�
        AreaAttack1_EWait, AreaAttack1_Attack, AreaAttack1_LWait,//�÷��̾ �ִ� ���ǿ� ���� ����
        BarrageAttack1_EWait, BarrageAttack1_Attack, BarrageAttack1_LWait,//�÷��� �ϳ� ���ؼ� �̵� �� ź�� �߻�
        BarrageAttack2_EWait, BarrageAttack2_Attack, BarrageAttack2_LWait,//����� ź �߻�
    }
}
