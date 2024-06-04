using System.Collections;
using UnityEngine;

/// <summary>
/// 투사체의 기본 클래스. 등록/제거만 기능
/// </summary>
public class Projectile : MonoBehaviour
{
    protected Vector2 originPos;
    protected Vector2 direction;//진행 방향
    protected float speed;
    protected float lifeTime = -1f;
    protected float lifeDistance = -1f;
    protected bool isInitialized = false;
    protected bool isDestroyed = false;

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="dir">이동 방향(or 목적지)</param>
    /// <param name="speed">이동 속도</param>
    /// <param name="lifeTime">수명(초)</param>
    /// <param name="lifeDistance">수명(거리)</param>
    public virtual void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        PerformanceManager.StartTimer("Projectile.Initialize");

        originPos = transform.position;
        direction = dir;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.lifeDistance = lifeDistance;
        isInitialized = true;

        if (lifeTime > 0)
        {
            TryDestroy();
        }

        PerformanceManager.StopTimer("Projectile.Initialize");
    }

    protected virtual void Update()
    {
        PerformanceManager.StartTimer("Projectile.Update");
        if (lifeDistance > 0 && (originPos - (Vector2)transform.position).sqrMagnitude >= lifeDistance * lifeDistance)
        {
            TryDestroy();
            PerformanceManager.StopTimer("Projectile.Update");
            return;
        }

        PerformanceManager.StopTimer("Projectile.Update");
    }

    /// <summary>
    /// Destroy 시도. DamageArea가 있으면 DamageArea의 것을 대신 사용
    /// </summary>
    protected void TryDestroy()
    {
        if (lifeTime > 0)
        {
            StartCoroutine(DestroyDelay());
        }
        else
        {
            var da = GetComponent<DamageArea>();
            if (da is not null)
            {
                isDestroyed = true;
                da.Destroy();
            }
            else
            {
                isDestroyed = true;
                Destroy(gameObject);
            }
        }

    }

    /// <summary>
    /// Destroy 대용
    /// </summary>
    protected void Destroy()
    {
        var da = GetComponent<DamageArea>();
        if (da is not null)
        {
            isDestroyed = true;
            da.Destroy();
        }
        else
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }


    protected IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }
}