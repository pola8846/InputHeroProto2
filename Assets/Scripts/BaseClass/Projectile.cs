using System.Collections;
using UnityEngine;

/// <summary>
/// 투사체의 기본 클래스. 등록/제거만 기능. 실제 움직임 및 충돌 시 처리는 하위 클래스에서 작성
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

    [SerializeField]
    protected bool canBeAutoAimed = true;


    /// <summary>
    /// 공격하는 유닛
    /// </summary>
    protected Unit attackUnit;
    /// <summary>
    /// 공격하는 유닛
    /// </summary>
    public Unit AttackUnit
    {
        get
        {
            return attackUnit;
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="dir">이동 방향(or 목적지)</param>
    /// <param name="speed">이동 속도</param>
    /// <param name="lifeTime">수명(초)</param>
    /// <param name="lifeDistance">수명(거리)</param>
    public virtual void Initialize(Vector2 dir, float speed, Unit sourceUnit, float lifeTime = -1f, float lifeDistance = -1f)
    {
        originPos = transform.position;
        direction = dir;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.lifeDistance = lifeDistance;
        isInitialized = true;
        attackUnit = sourceUnit;

        ProjectileManager.Enroll(this);

        if (lifeTime > 0)
        {
            StartCoroutine(DestroyDelay());
        }
    }

    protected virtual void Update()
    {
        if (lifeDistance > 0 && (originPos - (Vector2)transform.position).sqrMagnitude >= lifeDistance * lifeDistance)
        {
            Destroy();
            return;
        }
    }

    /// <summary>
    /// Destroy 대용
    /// </summary>
    protected virtual void Destroy()
    {
        if (isDestroyed)
        {
            return;
        }

        var da = GetComponent<DamageArea>();
        isDestroyed = true;
        ProjectileManager.Remove(this);
        if (da is not null)
        {
            da.Destroy();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    protected IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }

    public virtual void Parried()
    {

    }
}