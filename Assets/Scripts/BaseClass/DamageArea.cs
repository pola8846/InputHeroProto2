using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����+�ΰ�ȿ���� ��Ÿ���� Damage�� ������ ���� ����(���� ������Ʈ)
/// ��ü�����δ� �浹 ���� ��Ʈ�ڽ��� ����, ���� Ʈ���Ŵ� Attack���� ��
/// </summary>
public class DamageArea : CollisionChecker
{
    /// <summary>
    /// ������
    /// </summary>
    private Attack source;
    /// <summary>
    /// ������
    /// </summary>
    public Attack Source
    {
        get { return source; }
        set { source = value; }
    }
    /// <summary>
    /// �����. ���߿� ���� ���� �� �����̻� ���� �� ������ ��
    /// </summary>
    public float damage;

    /// <summary>
    /// �켱��. ���� ���� �켱��.
    /// </summary>
    [SerializeField]
    private int priority = 0;
    /// <summary>
    /// �켱��. ���� ���� �켱��.
    /// </summary>
    public int Priority
    {
        get
        { return priority; }
    }

    /// <summary>
    /// 1 �̻��̶�� �ش� Ƚ�� �浹 ���� �Ҹ�
    /// </summary>
    public int destroyHitCounter = -1;

    private List<HitBox> cachedList = new();
    private bool isCached_HitBoxList = false;
    public List<HitBox> HitBoxList//�� �˻�� ã���� ���� ���ϰ� �ɰ��Ͽ� ĳ���Ͽ� ���
    {
        get
        {
            if (isCached_HitBoxList)
            {
                return cachedList;
            }

            cachedList.Clear();
            foreach (var collider in EnteredColliders)
            {
                HitBox hitBox = collider.GetComponent<HitBox>();
                if (hitBox is not null)
                {
                    cachedList.Add(hitBox);
                }
            }
            isCached_HitBoxList = true;
            return cachedList;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        isCached_HitBoxList = false;
        Source.DamageEnter();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        isCached_HitBoxList = false;
        Source.DamageExit();
    }

    /// <summary>
    /// �ش� DamageArea�� ���� ���ֿ��� ���� ���ظ� �ֵ��� ����
    /// </summary>
    /// <param name="target">���ݹ޴� ���</param>
    public void DealDamage(Unit target)
    {
        UnitManager.Instance.DamageUnitToUnit(target, source.AttackUnit, this);

        if (destroyHitCounter != -1)
        {
            destroyHitCounter--;
            if (destroyHitCounter == 0)
            {
                Destroy();
            }
        }
    }

    /// <summary>
    /// �ش� DamageArea�� ���� Hitbox�� ���� ���ظ� �ֵ��� ����
    /// Hitbox�� ���� �ٸ� ó���� �ؾ� �ϴ� ��쿡 ���
    /// </summary>
    /// <param name="hitBox">���ݹ޴� ����� ��Ʈ�ڽ�</param>
    public void DealDamage(HitBox hitBox)
    {
        UnitManager.Instance.DamageUnitToHitbox(hitBox, source.AttackUnit, this);

        if (destroyHitCounter != -1)
        {
            destroyHitCounter--;
            if (destroyHitCounter == 0)
            {
                Destroy();
            }
        }
    }


    /// <summary>
    /// ��� ���� �� ����. ����Ƽ�� Destroy���� �̰� �� ��
    /// </summary>
    public void Destroy()
    {
        source.WithdrawDamage(this);
        Destroy(gameObject);
    }
}
