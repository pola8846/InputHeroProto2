using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 유닛 공용으로 사용하는 부모 클래스
/// 능력치 및 대미지 적용 등을 관리
/// </summary>
public class Unit : MonoBehaviour
{
    /// <summary>
    /// 유닛 아이디
    /// </summary>
    [SerializeField]
    protected int unitID = -1;
    /// <summary>
    /// 유닛 아이디
    /// </summary>
    public int UnitID
    {
        get
        {
            return unitID;
        }
        set
        {
            unitID = value;
        }
    }

    [SerializeField]
    protected Stats stats;
    public Stats Stats
    {
        get
        {
            return stats;
        }
        set
        {
            stats = value;
        }
    }


    public float Speed
    {
        get
        {
            return stats.moveSpeed;
        }
    }

    public float JumpPower
    {
        get
        {
            return stats.jumpPower;
        }
    }


    protected Mover moverV;
    public Mover MoverV => moverV;
    protected MoverByTransform moverT;
    public MoverByTransform MoverT => moverT;

    [SerializeField]
    protected bool isLookLeft = true;
    public bool IsLookLeft => isLookLeft;

    /// <summary>
    /// 피해를 받을 수 있는가?
    /// </summary>
    public bool canDamaged = true;

    protected virtual void Start()
    {
        unitID = UnitManager.Instance.EnrollUnit(this);
        moverV = gameObject.GetComponent<Mover>();
        moverT = gameObject.GetComponent<MoverByTransform>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public void Kill()
    {
        OnKilled();
        UnitManager.Instance.RemoveUnit(this);
        Destroy(gameObject);
    }

    protected virtual void OnKilled()
    {

    }

    protected virtual void OnDamaged()
    {

    }
    public bool Damage(float damage)
    {
        //예외
        if (damage <= 0f || !canDamaged)
        {
            return true;
        }

        //피해적용
        stats.health -= damage;
        OnDamaged();

        //사망처리
        if (stats.health > 0f)
        {
            return true;
        }
        Kill();
        return false;
    }


    public virtual void Turn()
    {
        isLookLeft = !isLookLeft;
        //transform.Rotate(new(0, 180, 0));
    }

    public virtual void Turn(bool lookLeft)
    {
        if (isLookLeft!=lookLeft)
        {
            Turn();
        }
    }
}


[Serializable]
public struct Stats
{
    /// <summary>
    /// 현재 체력
    /// </summary>
    public float health;
    /// <summary>
    /// 최대 체력
    /// </summary>
    public float maxHealth;
    /// <summary>
    /// 공격력
    /// </summary>
    public float attackPower;
    /// <summary>
    /// 방어력
    /// </summary>
    public float defencePower;
    /// <summary>
    /// 방어율
    /// </summary>
    public float defenceRate;
    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpPower;
    /// <summary>
    /// 쿨타임 배율
    /// </summary>
    public float cooldownRate;
    /// <summary>
    /// 점프 횟수
    /// </summary>
    public int jumpCount;


    /// <summary>
    /// 유닛 공용으로 사용하는 능력치 종류
    /// </summary>
    public enum StatType
    {
        /// <summary>
        /// 현재 체력
        /// </summary>
        health,
        /// <summary>
        /// 최대 체력
        /// </summary>
        maxHealth,
        /// <summary>
        /// 공격력
        /// </summary>
        attackPower,
        /// <summary>
        /// 방어력
        /// </summary>
        defencePower,
        /// <summary>
        /// 방어율
        /// </summary>
        defenceRate,
        /// <summary>
        /// 이동속도
        /// </summary>
        moveSpeed,
        /// <summary>
        /// 점프력
        /// </summary>
        jumpPower,
        /// <summary>
        /// 쿨타임 배율
        /// </summary>
        cooldownRate,
        /// <summary>
        /// 점프 횟수
        /// </summary>
        jumpCount,
    }
}