using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitMovement : MonoBehaviour
{
    [Header("�̵� Ÿ��")]
    /// <summary>
    /// ������ Ÿ�� ����
    /// </summary>
    private MovementType movementType;
    public MovementType MovementType
    {
        get
        {
            return movementType;
        }
        set
        {
            if (movementType != value)
            {

                movementType = value;

            }
        }
    }

    /// <summary>
    /// ���� �̵� Ÿ�� ����
    /// </summary>
    [SerializeField]
    private UnitMoveType moveType;

    [Header("�ӵ�")]
    private Rigidbody2D rigidBody;

    /// <summary>
    /// �ʱ� �߷°�
    /// </summary>
    private float originGravity;



    [Header("�� ���")]
    /// <summary>
    /// �� ��� �ٱ����� �̵� ��������
    /// </summary>
    [SerializeField]
    private bool canMoveOverMapLimit = true;
    [SerializeField]
    private float MapLimitExtra = 0;

    private void Start()
    {
        rigidBody= GetComponent<Rigidbody2D>();
        originGravity = rigidBody.gravityScale;
    }
}

public enum MovementType
{
    BY_VELOCITY,//������ �ӵ��� �̵�(���� ���)
    BY_SPEED,//������ �ӵ��� �̵�(Ʈ������ ���)
    TO_TARGET_POS,//Ư�� ��ġ�� �̵�
}

public enum UnitMoveType
{
    LAND,//����
    FLY,//����
}