using UnityEngine;

/// <summary>
/// ���� �ӵ��� ����ü. ���� Ŭ�������� ����ؼ� ���
/// </summary>
public class Shoot : Projectile
{

    private Vector2 previousPosition;
    private Vector2 currentPosition => transform.position;

    private TrailRenderer trail;


    /// <summary>
    /// �����. ���߿� ���� ���� �� �����̻� ���� �� ������ ��
    /// </summary>
    public float damage;

    [SerializeField]
    private bool isNotSlowed = false;

    [SerializeField]
    private GameObject parryParticle;



    //�ʱ�ȭ
    public override void Initialize(Vector2 dir, float speed, Unit sourceUnit, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, sourceUnit, lifeTime, lifeDistance);

        previousPosition = currentPosition;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if (isDestroyed) { return; }

        //�� �����ӿ� �̵��� �Ÿ�
        Vector2 moveDist = direction * speed * (isNotSlowed ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);

        //���� �߻�� ���� ����� ������ ��� ã��
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | 
            (1 << LayerMask.NameToLayer("Bullet")) | 
            (1 << LayerMask.NameToLayer("Ground"));
        var ray = Physics2D.LinecastAll(previousPosition, previousPosition + moveDist, layer);
        Collider2D target = null;
        Vector2 hitPos = Vector2.zero;

        //�浹�� ������ ��� ã��
        if (ray.Length > 0)
        {
            //�浹�� ��� ��� ����
            foreach (var hit in ray)
            {
                //Debug.Log(hit.transform.gameObject.name);
                //�����̰ų� ������ ������Ʈ�� ������ �ǳʶٱ�
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

                //���� ����� Ÿ�� ã��
                if (target == null ||
                    ((previousPosition - (Vector2)target.transform.position).sqrMagnitude < 
                    (previousPosition - (Vector2)hitGO.transform.position).sqrMagnitude))
                {
                    target = hit.collider;
                    hitPos = hit.point;
                }
            }

        }

        //���θ����°�?
        if (HitCheck(target))
        {
            Destroy();
            return;
        }

        //transform.position  = transform.position + (Vector3)moveDist;
        transform.Translate(moveDist, Space.World);
        previousPosition = currentPosition;
    }

    protected virtual bool HitCheck(Collider2D target)
    {
        if (target != null)
        {
            // �浹 ó��
            Debug.Log("Hit");


            // �浹�� ���� ó�� �߰�
            return true;
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
    /// �и��Ǿ��� �� �ı��ϰ� ����Ʈ ����. ����Ʈ ���� �߰��ؾ���
    /// </summary>
    public override void Parried()
    {
        base.Parried();
        GameObject go = Instantiate(parryParticle);
        go.transform.position = transform.position;
        FMODUnity.RuntimeManager.PlayOneShot("event:/BulletCrash");

        Destroy();
    }
}
