using UnityEngine;

/// <summary>
/// 빠른 속도의 투사체. 하위 클래스에서 상속해서 사용
/// </summary>
public class Shoot : Projectile
{

    private Vector2 previousPosition;
    private Vector2 currentPosition => transform.position;

    private TrailRenderer trail;


    /// <summary>
    /// 대미지. 나중에 피해 공식 및 상태이상 만들 때 수정할 것
    /// </summary>
    public float damage;

    [SerializeField]
    private bool isNotSlowed = false;



    //초기화
    public override void Initialize(Vector2 dir, float speed, Unit sourceUnit, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, sourceUnit, lifeTime, lifeDistance);

        previousPosition = currentPosition;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if (isDestroyed) { return; }

        //한 프레임에 이동할 거리
        Vector2 moveDist = direction * speed * (isNotSlowed ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);

        //레이 발사로 가장 가까운 적법한 대상 찾기
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | (1 << LayerMask.NameToLayer("Bullet"));
        var ray = Physics2D.LinecastAll(previousPosition, previousPosition + moveDist, layer);
        Collider2D target = null;
        Vector2 hitPos = Vector2.zero;

        //충돌한 적법한 대상 찾기
        if (ray.Length > 0)
        {
            //충돌한 모든 대상 상대로
            foreach (var hit in ray)
            {
                //본인이거나 적법한 컴포넌트가 없으면 건너뛰기
                if (hit.collider == target ||
                    (
                    hit.collider.GetComponent<IBulletHitChecker>() == null &&
                    hit.collider.GetComponent<Unit>() == null &&
                    hit.collider.GetComponent<Projectile>() == null
                    ))
                {
                    continue;
                }

                GameObject hitGO = hit.collider.gameObject;

                //가장 가까운 타겟 찾기
                if (target == null ||
                    ((previousPosition - (Vector2)target.transform.position).sqrMagnitude < (previousPosition - (Vector2)hitGO.transform.position).sqrMagnitude))
                {
                    target = hit.collider;
                    hitPos = hit.point;
                }
            }

        }

        //가로막혔는가?
        if (HitCheck(target))
        {
            transform.position = hitPos;
            Destroy();
            return;
        }

        transform.Translate(moveDist);
        previousPosition = currentPosition;
    }

    protected virtual bool HitCheck(Collider2D target)
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

    /// <summary>
    /// 패리되었을 때 파괴하고 이펙트 생성. 이펙트 관련 추가해야함
    /// </summary>
    public override void Parried()
    {
        base.Parried();
        Destroy();
    }
}
