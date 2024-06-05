using UnityEngine;

/// <summary>
/// 빠른 속도의 투사체. 하위 클래스에서 상속해서 사용
/// </summary>
public class Shoot : Projectile
{
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

    private Vector2 previousPosition;
    private Vector2 currentPosition => transform.position;

    private bool isDestroying = false;
    private TrailRenderer trail;


    /// <summary>
    /// 대미지. 나중에 피해 공식 및 상태이상 만들 때 수정할 것
    /// </summary>
    public float damage;

    [SerializeField]
    private bool isNotSlowed = false;



    //초기화
    public override void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, lifeTime, lifeDistance);

        previousPosition = currentPosition;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        //한 프레임에 이동할 거리
        Vector2 moveDist = direction * speed * (isNotSlowed ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);

        //레이 발사로 가장 가까운 적법한 대상 찾기
        var ray = Physics2D.LinecastAll(previousPosition, previousPosition + moveDist);
        if (1 <= ray.Length)
        {
            //충돌한 적법한 대상
            Transform target = null;
            Vector2 hitPos = Vector2.zero;

            foreach (var hit in ray)
            {
                if (hit.transform == target || hit.transform.GetComponent<IBulletHitChecker>() == null)
                {
                    continue;
                }

                //가장 가까운 타겟 찾기
                if (target == null ||
                    ((previousPosition - (Vector2)target.position).sqrMagnitude < (previousPosition - (Vector2)hit.transform.position).sqrMagnitude))
                {
                    target = hit.transform;
                    hitPos = hit.point;
                }

            }

            //가로막혔는가?
            if (HitCheck(target))
            {
                transform.Translate(hitPos);
                Destroy();
                return;
            }

        }

        transform.Translate(moveDist);
    }

    protected virtual bool HitCheck(Transform target)
    {
        if (target != null)
        {
            // 충돌 처리
            Debug.Log("Hit");


            // 충돌에 대한 처리 추가
        }
        return false;
    }

    protected virtual void Hit(Unit targetUnit)
    {
        UnitManager.Instance.DamageUnitToUnit(targetUnit, AttackUnit, damage);
        return;
    }

    protected override void Destroy()
    {
        if (trail is not null)
        {
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, trail.time);
        }
        base.Destroy();
    }
}
