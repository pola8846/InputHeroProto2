using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� �������� ����ϴ� �θ� Ŭ����
/// �ɷ�ġ �� ����� ���� ���� ����
/// </summary>
public class Unit : MonoBehaviour
{
    /// <summary>
    /// ���� ���̵�
    /// </summary>
    [SerializeField]
    protected int unitID = -1;
    /// <summary>
    /// ���� ���̵�
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
    /// ���ظ� ���� �� �ִ°�?
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
        UnitManager.Instance.RemoveUnit(this);
        Destroy(gameObject);
    }

    public bool Damage(float damage)
    {
        //����
        if (damage <= 0f || !canDamaged)
        {
            return true;
        }

        //��������
        stats.health -= damage;

        //���ó��
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
    /// ���� ü��
    /// </summary>
    public float health;
    /// <summary>
    /// �ִ� ü��
    /// </summary>
    public float maxHealth;
    /// <summary>
    /// ���ݷ�
    /// </summary>
    public float attackPower;
    /// <summary>
    /// ����
    /// </summary>
    public float defencePower;
    /// <summary>
    /// �����
    /// </summary>
    public float defenceRate;
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// ������
    /// </summary>
    public float jumpPower;
    /// <summary>
    /// ��Ÿ�� ����
    /// </summary>
    public float cooldownRate;
    /// <summary>
    /// ���� Ƚ��
    /// </summary>
    public int jumpCount;


    /// <summary>
    /// ���� �������� ����ϴ� �ɷ�ġ ����
    /// </summary>
    public enum StatType
    {
        /// <summary>
        /// ���� ü��
        /// </summary>
        health,
        /// <summary>
        /// �ִ� ü��
        /// </summary>
        maxHealth,
        /// <summary>
        /// ���ݷ�
        /// </summary>
        attackPower,
        /// <summary>
        /// ����
        /// </summary>
        defencePower,
        /// <summary>
        /// �����
        /// </summary>
        defenceRate,
        /// <summary>
        /// �̵��ӵ�
        /// </summary>
        moveSpeed,
        /// <summary>
        /// ������
        /// </summary>
        jumpPower,
        /// <summary>
        /// ��Ÿ�� ����
        /// </summary>
        cooldownRate,
        /// <summary>
        /// ���� Ƚ��
        /// </summary>
        jumpCount,
    }
}