using UnityEngine;

/// <summary>
/// 적 공격, 빠른 속도
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

    [SerializeField]
    private bool isNotSlowed = false;


    public override void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, lifeTime, lifeDistance);

        previousPosition = currentPosition;
        PerformanceManager.StopTimer("Bullet.Initialize");
    }

    private void FixedUpdate()
    {
        //한 프레임에 이동할 거리
        Vector2 moveDist = direction * speed * (isNotSlowed ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);

        var ray = Physics2D.RaycastAll(previousPosition, moveDist.normalized, moveDist.magnitude);
        if (1 <= ray.Length)
        {
            foreach (var hit in ray)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                var hitTarget = hit.transform.GetComponent<IBulletHitChecker>();
                if (hitTarget == null)
                {
                    continue;
                }


            }
            // 충돌 처리
            Debug.Log("Hit: ");
            // 필요한 경우, 충돌 지점에 대한 처리 추가
        }

        transform.Translate(moveDist);
    }
}
