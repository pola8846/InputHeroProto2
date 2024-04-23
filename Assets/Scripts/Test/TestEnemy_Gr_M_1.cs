using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Gr_M_1 : Enemy
{
    private Rigidbody2D rb;
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
        rb = GetComponent<Rigidbody2D>();
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
                    state = 2;
                    renderer.material.color = Color.red;
                    timer2 = Time.time;
                    atkGO = Instantiate(atk, transform);
                    atkGO.GetComponent<Attack>().Initialization(this, "Player", atkGO);
                    //if (!isLookLeft)
                    //{
                    //    atkGO.transform.localPosition = new Vector3(-atkGO.transform.localPosition.x, atkGO.transform.localPosition.y);
                    //}
                }
                break;
            case 2:
                if (timer2 + attackTime <= Time.time)
                {
                    state = 3;
                    timer3 = Time.time;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    Destroy(atkGO);
                }
                break;
            case 3:
                if (timer3 + attackCoolTime <= Time.time)
                {
                    state = 0;
                    renderer.material.color = originColor;
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

                    rb.velocity = new Vector2(movementX, rb.velocity.y);

                    if (AttackRangeCheck())//공격 사거리 내에 오면 스테이트 1로
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        state = 1;
                        timer1 = Time.time;
                        renderer.material.color = Color.yellow;
                    }
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                break;
            case 1://플레이어가 공격 사거리 내에 있을 때
                break;
            case 2://공격 중일 때
                {
                    float movementX = Mathf.Max(0, stats.moveSpeed * attackMoveSpeedRate) * (isLookLeft ? -1 : 1);
                    rb.velocity = new Vector2(movementX, rb.velocity.y);
                }
                break;
            case 3://공격 쿨타임일 때
                break;
            default:
                break;
        }

    }

    private bool AttackRangeCheck()
    {
        return Vector3.Distance(transform.position, GameManager.Player.transform.position) <= attackRange;
    }
}
