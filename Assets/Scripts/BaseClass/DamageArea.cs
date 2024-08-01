using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 피해+부가효과를 나타내는 Damage를 입히기 위한 영역(게임 오브젝트)
/// 자체적으로는 충돌 중인 히트박스만 감지, 실제 트리거는 Attack에서 함
/// </summary>
public class DamageArea : CollisionChecker
{
    /// <summary>
    /// 공격자
    /// </summary>
    private Attack source;
    /// <summary>
    /// 공격자
    /// </summary>
    public Attack Source
    {
        get { return source; }
        set { source = value; }
    }
    /// <summary>
    /// 대미지. 나중에 피해 공식 및 상태이상 만들 때 수정할 것
    /// </summary>
    public float damage;

    /// <summary>
    /// 우선도. 높은 것을 우선함.
    /// </summary>
    [SerializeField]
    private int priority = 0;
    /// <summary>
    /// 우선도. 높은 것을 우선함.
    /// </summary>
    public int Priority
    {
        get
        { return priority; }
    }

    /// <summary>
    /// 1 이상이라면 해당 횟수 충돌 이후 소멸
    /// </summary>
    public int destroyHitCounter = -1;

    private List<HitBox> cachedList = new();
    private bool isCached_HitBoxList = false;
    public List<HitBox> HitBoxList
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
    /// 해당 DamageArea로 인해 피해를 주도록 만듬
    /// </summary>
    /// <param name="target">공격받는 대상</param>
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
    /// 등록 해제 및 삭제. 유니티의 Destroy말고 이걸 쓸 것
    /// </summary>
    public void Destroy()
    {
        source.WithdrawDamage(this);
        Destroy(gameObject);
    }
}
