using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private Rigidbody2D rb;
    private float originGravity;

    //목표 속도
    private float targetSpeedX = 0;
    public float TargetSpeedX => targetSpeedX;
    private float targetSpeedY = 0;
    public float TargetSpeedY => targetSpeedY;

    //최대 속도
    [SerializeField]
    private float maxSpeedX = -1;
    public float MaxSpeedX
    { get { return maxSpeedX; } set { maxSpeedX = value; } }

    [SerializeField]
    private float maxSpeedY = -1;
    public float MaxSpeedY
    { get { return maxSpeedY; } set { maxSpeedY = value; } }

    //속도 고정
    [SerializeField]
    private bool fixSpeedX = false;
    [SerializeField]
    private bool fixSpeedY = false;

    public bool speedCap = true;

    [SerializeField]
    private bool canMoveOverMapLimit = true;

    [SerializeField]
    private float MapLimitExtra = 0;

    public Vector2 Velocity
    {
        get { return rb.velocity; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originGravity = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        PerformanceManager.StartTimer("Mover.FixedUpdate");

        if (fixSpeedX)
        {
            SetVelocityX();

        }
        if (fixSpeedY)
        {
            SetVelocityY();
        }
        if (speedCap)
        {
            MaxSpeedCheck();
        }

        if (!canMoveOverMapLimit)
        {
            transform.position = GameTools.ClampToRect(transform.position, GameManager.MapLimit, MapLimitExtra);
        }

        PerformanceManager.StopTimer("Mover.FixedUpdate");
    }

    private void MaxSpeedCheck()
    {
        PerformanceManager.StartTimer("Mover.MaxSpeedCheck");
        if (maxSpeedX >= 0)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeedX, maxSpeedX), rb.velocity.y);
        }
        if (maxSpeedY >= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeedY, maxSpeedY));
        }
        PerformanceManager.StopTimer("Mover.MaxSpeedCheck");
    }

    /// <summary>
    /// 속도 재설정(X축만)
    /// </summary>
    private void SetVelocityX()
    {
        PerformanceManager.StartTimer("Mover.SetVelocityX");
        rb.velocity = new Vector2(targetSpeedX, rb.velocity.y);
        if (speedCap)
        {
            MaxSpeedCheck();
        }
        PerformanceManager.StopTimer("Mover.SetVelocityX");
    }

    /// <summary>
    /// 속도 재설정(X축만)
    /// </summary>
    /// <param name="speed">설정할 속도</param>
    public void SetVelocityX(float speed, bool once = false)
    {
        targetSpeedX = speed;
        SetVelocityX();
        fixSpeedX = !once;
    }

    /// <summary>
    /// 이동 정지(X축만)
    /// </summary>
    public void StopMoveX(bool instant = true)
    {
        PerformanceManager.StartTimer("Mover.StopMoveX");
        fixSpeedX = false;
        targetSpeedX = 0;
        if (instant)
        {
            SetVelocityX();
        }
        PerformanceManager.StopTimer("Mover.StopMoveX");
    }

    /// <summary>
    /// 속도 재설정(Y축만)
    /// </summary>
    private void SetVelocityY()
    {
        PerformanceManager.StartTimer("Mover.SetVelocityY");
        rb.velocity = new Vector2(rb.velocity.x, targetSpeedY);
        if (speedCap)
        {
            MaxSpeedCheck();
        }
        PerformanceManager.StopTimer("Mover.SetVelocityY");
    }

    /// <summary>
    /// 속도 재설정(Y축만)
    /// </summary>
    /// <param name="speed">설정할 속도</param>
    public void SetVelocityY(float speed, bool once = false)
    {
        targetSpeedY = speed;
        SetVelocityY();
        fixSpeedY = !once;
    }

    /// <summary>
    /// 이동 정지(Y축만)
    /// </summary>
    public void StopMoveY(bool instant = true)
    {
        PerformanceManager.StartTimer("Mover.StopMoveY");
        fixSpeedY = false;
        targetSpeedY = 0;
        if (instant)
        {
            SetVelocityY();
        }
        PerformanceManager.StopTimer("Mover.StopMoveY");
    }

    /// <summary>
    /// 속도 재설정
    /// </summary>
    /// <param name="velocity">재설정할 속도</param>
    public void SetVelocity(Vector2 velocity, bool once = false)
    {
        SetVelocityX(velocity.x, once);
        SetVelocityY(velocity.y, once);
    }

    /// <summary>
    /// 속도 재설정
    /// </summary>
    /// <param name="velocity">재설정할 속도</param>
    public void SetVelocity(Vector3 velocity)
    {
        SetVelocity((Vector2)velocity);
    }

    /// <summary>
    /// 이동 정지
    /// </summary>
    public void StopMove(bool instant = true)
    {
        PerformanceManager.StartTimer("Mover.StopMove");
        fixSpeedX = false;
        fixSpeedY = false;
        targetSpeedX = 0;
        targetSpeedY = 0;
        if (instant)
        {
            SetVelocity(Vector2.zero, true);
        }
        PerformanceManager.StopTimer("Mover.StopMove");
    }

    /// <summary>
    /// AddForce 대체
    /// </summary>
    /// <param name="force">가할 힘(위 방향)</param>
    public void AddForceX(float force)
    {
        PerformanceManager.StartTimer("Mover.AddForceX");
        fixSpeedX = true;
        rb.velocity = new Vector2(rb.velocity.x + force, rb.velocity.y);
        PerformanceManager.StopTimer("Mover.AddForceX");
    }

    /// <summary>
    /// AddForce 대체
    /// </summary>
    /// <param name="force">가할 힘(오른쪽 방향)</param>
    public void AddForceY(float force)
    {
        PerformanceManager.StartTimer("Mover.AddForceY");
        fixSpeedY = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + force);
        PerformanceManager.StopTimer("Mover.AddForceY");
    }

    /// <summary>
    /// AddForce 대체
    /// </summary>
    /// <param name="force">가할 힘</param>
    public void AddForce(Vector2 force)
    {
        PerformanceManager.StartTimer("Mover.AddForce");
        fixSpeedX = false;
        fixSpeedY = false;
        rb.velocity = new Vector2(rb.velocity.x + force.x, rb.velocity.y + force.y);
        PerformanceManager.StopTimer("Mover.AddForce");
    }

    /// <summary>
    /// 중력 전환
    /// </summary>
    /// <param name="isOn">중력을 켤 것인가?</param>
    public void TurnGravity(bool isOn)
    {
        if (isOn)
        {
            rb.gravityScale = originGravity;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }
}

public struct Move
{
    public float x;
    public bool isMoveX;
    public float y;
    public bool isMoveY;
    public float lifeTime;
    public bool isAdded;

    public Move(Vector2 move, bool isMoveX = true, bool isMoveY = true, float lifeTime = -1, bool isAdded = false)
    {
        x = move.x;
        y = move.y;
        this.isMoveX = isMoveX;
        this.isMoveY = isMoveY;
        this.isAdded = isAdded;

        this.lifeTime = lifeTime;
    }
}