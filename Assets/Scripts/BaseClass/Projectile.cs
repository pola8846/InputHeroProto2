using System.Collections;
using UnityEngine;

/// <summary>
/// ����ü�� �⺻ Ŭ����. ���/���Ÿ� ���. ���� ������ �� �浹 �� ó���� ���� Ŭ�������� �ۼ�
/// </summary>
public class Projectile : MonoBehaviour
{
    protected Vector2 originPos;//���� ��ġ
    protected Vector2 direction;//���� ����
    protected float speed;//�ӵ�
    protected float lifeTime = -1f;//����(�ð�)
    protected float lifeDistance = -1f;//����(�Ÿ�)
    protected bool isInitialized = false;//�ʱ�ȭ �Ǿ��°�?
    protected bool isDestroyed = false;//�ı� ���ΰ�?(�ߺ� �ı� ������ �÷���)

    [SerializeField]
    protected bool canBeAutoAimed = true;//�ڵ� ������ ����� �Ǵ°�?


    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    protected Unit attackUnit;
    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    public Unit AttackUnit
    {
        get
        {
            return attackUnit;
        }
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="dir">�̵� ����(or ������)</param>
    /// <param name="speed">�̵� �ӵ�</param>
    /// <param name="lifeTime">����(��)</param>
    /// <param name="lifeDistance">����(�Ÿ�)</param>
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
    /// Destroy ���
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