using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ܼ� ���� ����ü
/// </summary>
public class Bullet : Projectile
{
    private Rigidbody2D rb;

    [SerializeField]
    private bool isNotSlowed = false;

    public override void Initialize(Vector2 dir, float speed, Unit sourceUnit, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, sourceUnit, lifeTime, lifeDistance);

        rb = GetComponent<Rigidbody2D>();
        SetSpeed();
        if (isNotSlowed)
        {
            TimeManager.OnTimeScaleChanged += TimeScaleChanged;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        if (isNotSlowed)
        {
            TimeManager.OnTimeScaleChanged -= TimeScaleChanged;
        }
    }

    /// <summary>
    /// �ð� ��� �ٲ���� �� �̺�Ʈ���� �����ϴ� �Լ�
    /// </summary>
    private void TimeScaleChanged(object sender, float timeScale)
    {
        SetSpeed();
    }

    /// <summary>
    /// �ӵ� �缳��
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