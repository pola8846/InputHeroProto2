using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 유닛 공용으로 사용하는 부모 클래스
/// 능력치 및 대미지 적용 등을 관리
/// </summary>
public class Unit : MonoBehaviour, IMoveReceiver
{
    [SerializeField]
    protected int unitID = -1;
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

    protected float movementX = 0;
    [SerializeField]
    protected Transform groundCheckerLT;
    [SerializeField]
    protected Transform groundCheckerRD;
    [SerializeField]
    protected float groundCheckRadius = 0;
    [SerializeField]
    protected string groundLayer = "";

    protected Dictionary<KeyCode, bool> keyStay = new();
    [SerializeField]
    protected bool isLookLeft = true;

    protected virtual void Start()
    {
        unitID = UnitManager.Instance.EnrollUnit(this);
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public void Kill()
    {
        UnitManager.Instance.RemoveUnit(this);
        Destroy(gameObject);
    }

    public bool Damage(float damage)
    {
        //예외
        if (damage <= 0f)
        {
            return true;
        }

        //피해적용
        stats.health -= damage;

        //사망처리
        if (stats.health > 0f)
        {
            return true;
        }
        Kill();
        return false;
    }

    public virtual void KeyDown(KeyCode keyCode)
    {
        if (keyStay.ContainsKey(keyCode))
        {
            keyStay[keyCode] = true;
        }
        else
        {
            keyStay.Add(keyCode, true);
        }
    }

    public virtual void KeyUp(KeyCode keyCode)
    {
        if (keyStay.ContainsKey(keyCode))
        {
            keyStay[keyCode] = false;
        }
        else
        {
            keyStay.Add(keyCode, false);
        }
    }

    public void KeyReset(KeyCode keyCode)
    {
        foreach (var item in keyStay)
        {
            KeyUp(item.Key);
        }
        keyStay.Clear();
    }

    public void Turn()
    {
        isLookLeft = !isLookLeft;
        transform.Rotate(new(0, 180, 0));
    }

    /// <summary>
    /// 땅에 있는지 체크
    /// </summary>
    /// <returns>땅에 있는가?</returns>
    protected bool GroundCheck()
    {
        if (groundCheckerLT == null || groundCheckerRD == null)
        {
            return false;
        }
        return Physics2D.OverlapArea(groundCheckerLT.position, groundCheckerRD.position, LayerMask.GetMask(groundLayer));
        //return Physics2D.OverlapCircle(groundChecker.transform.position, groundCheckRadius, LayerMask.GetMask("Ground")) ||
        //Physics2D.OverlapCircle(groundChecker2.transform.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmos()
    {
        if (groundCheckerLT != null && groundCheckerRD != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(groundCheckerLT.position.x, groundCheckerLT.position.y, 0), new Vector3(groundCheckerRD.position.x, groundCheckerLT.position.y, 0));
            Gizmos.DrawLine(new Vector3(groundCheckerLT.position.x, groundCheckerLT.position.y, 0), new Vector3(groundCheckerLT.position.x, groundCheckerRD.position.y, 0));
            Gizmos.DrawLine(new Vector3(groundCheckerRD.position.x, groundCheckerLT.position.y, 0), new Vector3(groundCheckerRD.position.x, groundCheckerRD.position.y, 0));
            Gizmos.DrawLine(new Vector3(groundCheckerLT.position.x, groundCheckerRD.position.y, 0), new Vector3(groundCheckerRD.position.x, groundCheckerRD.position.y, 0));
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
    /// 현재 마나
    /// </summary>
    public float magicPoint;
    /// <summary>
    /// 최대 마나
    /// </summary>
    public float maxMagicPoint;
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
        /// 현재 마나
        /// </summary>
        magicPoint,
        /// <summary>
        /// 최대 마나
        /// </summary>
        maxMagicPoint,
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
        jumpCount,
    }
}