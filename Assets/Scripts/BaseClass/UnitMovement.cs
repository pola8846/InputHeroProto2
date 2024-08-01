using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitMovement : MonoBehaviour
{
    [Header("이동 타입")]
    /// <summary>
    /// 움직임 타입 구분
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
    /// 유닛 이동 타입 구분
    /// </summary>
    [SerializeField]
    private UnitMoveType moveType;

    [Header("속도")]
    private Rigidbody2D rigidBody;

    /// <summary>
    /// 초기 중력값
    /// </summary>
    private float originGravity;



    [Header("맵 경계")]
    /// <summary>
    /// 맵 경계 바깥으로 이동 가능한지
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
    BY_VELOCITY,//정해진 속도로 이동(물리 기반)
    BY_SPEED,//정해진 속도로 이동(트랜스폼 기반)
    TO_TARGET_POS,//특정 위치로 이동
}

public enum UnitMoveType
{
    LAND,//지상
    FLY,//공중
}