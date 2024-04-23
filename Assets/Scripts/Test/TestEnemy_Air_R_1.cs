using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Air_R_1 : Enemy
{
    private Rigidbody2D rb;
    [SerializeField]
    private float patrolSpeedRate = 1f;
    [SerializeField]
    private float patrolDistance;
    [SerializeField]
    private float patrolTick = 1f;
    private Vector2 patrolDist;
    [SerializeField]
    private int state = 0;
    private Renderer renderer;
    private Color originColor;

    private float timer1;
    private float timer2;
    private float timer3;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
        originColor = renderer.material.color;
        timer1 = Time.time;
        timer2 = Time.time;
        timer3 = Time.time;
        patrolDist = transform.position;
    }
    protected override void Update()
    {
        base.Update();
        if (state >= 0 && state <= 2 && FindPlayer())
        {
            state = 3;
            return;
        }
        switch (state)
        {
            case 0://순찰 대기
                if (timer1 + patrolTick <= Time.time)
                {
                    state = 1;
                    patrolDist = new Vector2(transform.position.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y);
                }
                break;
            case 1://순찰 이동
                if (Vector3.Distance(transform.position, (Vector3)patrolDist) <= .3f)
                {
                    state = 0;
                    timer1 = Time.time;
                }
                break;
            case 2://순찰 복귀
                if (Vector3.Distance(transform.position, (Vector3)patrolDist) <= .3f)
                {
                    state = 0;
                    timer1 = Time.time;
                }
                break;
            case 3://플레이어에게 접근

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
            case 0:
                rb.velocity = new Vector2(0, 0);
                break;
            case 1:
                {
                    float speed = Mathf.Max(0, Speed) * patrolSpeedRate;
                    rb.velocity = GetDist(patrolDist) * speed;
                }
                break;
            case 2:
                {
                    float speed = Mathf.Max(0, Speed);
                    rb.velocity = GetDist(patrolDist) * speed;
                }
                break;
            default:
                break;
        }
    }

    private Vector2 GetDist(Vector2 target)
    {
        Vector2 temp = target - (Vector2)transform.position;
        return temp.normalized;
    }
}
