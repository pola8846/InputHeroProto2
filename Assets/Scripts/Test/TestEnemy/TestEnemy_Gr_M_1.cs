using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class TestEnemy_Gr_M_1 : Enemy
{
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private float attackWaitTime = 1f;
    [SerializeField]
    private float attackTime = 1f;
    [SerializeField]
    private float attackMoveSpeedRate = 2f;
    [SerializeField]
    private float attackCoolTime = 1f;
    [SerializeField]
    private int state = 0;
    private float timer1;
    private float timer2;
    private float timer3;
    private Renderer renderer;
    private Color originColor;

    public GameObject atk;
    private GameObject atkGO;
    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        originColor = renderer.material.color;
        timer1 = Time.time;
        timer2 = Time.time;
        timer3 = Time.time;
    }

    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case 0:
                break;
            case 1:
                if (timer1 + attackWaitTime <= Time.time)//공격 대기 시간 지나면
                {
                    SetState(2);
                }
                break;
            case 2:
                if (timer2 + attackTime <= Time.time)
                {
                    SetState(3);
                }
                break;
            case 3:
                if (timer3 + attackCoolTime <= Time.time)
                {
                    SetState(0);
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
        bool isRight = player.transform.position.x >= transform.position.x;//플레이어가 오른쪽에 있는가?
        switch (state)
        {
            case 0://평상시
                if (FindPlayer())
                {
                    float movementX = Mathf.Max(0, Speed) * (isRight ? 1 : -1);

                    if (isRight == isLookLeft)
                    {
                        Turn();
                    }
                    //Debug.Log(movementX);
                    MoverV.SetVelocityX(movementX);

                    if (AttackRangeCheck())//공격 사거리 내에 오면 스테이트 1로
                    {
                        MoverV.StopMoveX();
                        SetState(1);
                        timer1 = Time.time;
                        SetColor(Color.yellow);
                    }
                }
                else
                {
                    //Debug.Log("stop");
                    MoverV.StopMoveX();
                }
                break;
            case 1://플레이어가 공격 사거리 내에 있을 때
                break;
            case 2://공격 중일 때
                {
                    float movementX = Mathf.Max(0, stats.moveSpeed * attackMoveSpeedRate) * (isLookLeft ? -1 : 1);
                    MoverV.SetVelocityX(movementX);
                }
                break;
            case 3://공격 쿨타임일 때
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
                SetColor(Color.red);
                timer2 = Time.time;
                atkGO = Instantiate(atk, transform);
                atkGO.GetComponent<Attack>().Initialization(this, "Player", atkGO);
                break;
            case 2:
                timer3 = Time.time;
                MoverV.StopMoveX();
                Destroy(atkGO);
                break;
            case 3:
                SetColor(Color.clear);
                break;
            default:
                break;
        }

        state = num;

        switch (state)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    private bool AttackRangeCheck()
    {
        return Vector3.Distance(transform.position, GameManager.Player.transform.position) <= attackRange;
    }

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
}
