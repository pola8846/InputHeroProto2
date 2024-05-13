using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 단순 직선 투사체
/// </summary>
public class Bullet : Projectile
{
    private Rigidbody2D rb;

    [SerializeField]
    private bool isNotSlowed = false;

    public override void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        PerformanceManager.StartTimer("Bullet.Initialize");
        base.Initialize(dir, speed, lifeTime, lifeDistance);

        rb = GetComponent<Rigidbody2D>();
        SetSpeed();
        if (isNotSlowed)
        {
            GameManager.OnTimeScaleChanged += TimeScaleChanged;
        }
        PerformanceManager.StopTimer("Bullet.Initialize");
    }

    protected override void Update()
    {
        PerformanceManager.StartTimer("Bullet.Update");
        base.Update();

        PerformanceManager.StopTimer("Bullet.Update");
    }

    private void OnDestroy()
    {
        if (isNotSlowed)
        {
            GameManager.OnTimeScaleChanged -= TimeScaleChanged;
        }
    }

    /// <summary>
    /// 시간 배속 바뀌었을 때 이벤트에서 실행하는 함수
    /// </summary>
    private void TimeScaleChanged(object sender, float timeScale)
    {
        SetSpeed();
    }

    /// <summary>
    /// 속도 재설정
    /// </summary>
    private void SetSpeed()
    {
        if (isNotSlowed)
        {
            if (Time.timeScale == 0)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.velocity = direction * speed / Time.timeScale;
            }
        }
        else
        {
            rb.velocity = direction * speed;
        }
    }
}