using UnityEngine;

/// <summary>
/// 단순 직선 투사체
/// </summary>
public class Bullet : Projectile
{
    private Rigidbody2D rb;

    [SerializeField]
    private bool isNotSlowed = false;//슬로우 영향을 받는가?

    //초기화
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