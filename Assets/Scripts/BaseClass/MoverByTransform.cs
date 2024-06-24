using System;
using UnityEngine;

public class MoverByTransform : MonoBehaviour
{
    /// <summary>
    /// 로컬 좌표를 사용하는가?
    /// </summary>
    [SerializeField]
    private bool isSetByLocal = true;
    private Vector2 Position
    {
        get
        {
            if (isSetByLocal)
            {
                return transform.localPosition;
            }
            else
            {
                return transform.position;
            }
        }
        set
        {
            if (isSetByLocal)
            {
                transform.localPosition = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }

    /// <summary>
    /// 이동 타입
    /// </summary>
    public enum moveType
    {
        LinearByPosWithTime,
        LinearByPosWithSpeed,
        LinearBySpeed,
        ByFunction,
    }

    private moveType type;

    //위치 기반 이동 시
    //목표 위치
    private Vector2 targetPos;
    private float targetMoveTime = 0;
    private Vector2 posOrigin = Vector2.zero;

    //속도 기반 이동 시
    //목표 속도
    private Vector2 targetSpeed;
    private float targetSpeedF = 0;

    //함수 사용 시
    Func<float, Vector2> MoveFunction;

    [SerializeField]
    private float moveTimer;
    [SerializeField]
    private bool isMoving = false;
    public bool IsMoving => isMoving;

    [SerializeField]
    private bool canMoveOverMapLimit = true;
    [SerializeField]
    private float MapLimitExtra = 0;

    private void Update()
    {
        PerformanceManager.StartTimer("MoverByTransform.Update");

        if (isMoving)
        {
            switch (type)
            {
                case moveType.LinearByPosWithTime:
                    MoveLinearByPosWithTime();
                    break;

                case moveType.LinearByPosWithSpeed:
                    MoveLinearByPosWithSpeed();
                    break;

                case moveType.LinearBySpeed:
                    MoveLinearBySpeed();
                    break;

                case moveType.ByFunction:
                    MoveByFunction();
                    break;

                default:
                    break;
            }
        }


        if (!canMoveOverMapLimit)
        {
            transform.position = GameTools.ClampToRect(transform.position, GameManager.MapLimit, MapLimitExtra);
        }
        PerformanceManager.StopTimer("MoverByTransform.Update");
    }

    /// <summary>
    /// 움직임 시작
    /// </summary>
    public void StartMove(moveType type, Vector2 target, params float[] options)
    {
        PerformanceManager.StartTimer("MoverByTransform.StartMove");
        moveTimer = 0;
        this.type = type;
        posOrigin = Position;
        isMoving = true;

        switch (type)
        {
            case moveType.LinearByPosWithTime:
                targetPos = target;
                targetMoveTime = Mathf.Max(options[0], 0);

                break;

            case moveType.LinearByPosWithSpeed:
                targetPos = target;
                targetSpeedF = options[0];
                break;

            case moveType.LinearBySpeed:
                targetSpeed = target;
                targetMoveTime = options[0];

                break;
            default:
                Debug.LogError("알 수 없는 이동 유형");
                break;
        }
        PerformanceManager.StopTimer("MoverByTransform.StartMove");
    }

    public void StartMove(moveType type, float targetTime, Func<float, Vector2> function)
    {
        moveTimer = 0;
        this.type = type;
        posOrigin = Position;
        targetMoveTime = targetTime;
        isMoving = true;

        switch (type)
        {
            case moveType.ByFunction:
                MoveFunction = function;
                break;
            default:
                Debug.LogError("알 수 없는 이동 유형");
                break;
        }

    }

    public void StopMove()
    {
        moveTimer = 0;
        isMoving = false;
    }

    private void MoveLinearByPosWithTime()
    {
        PerformanceManager.StartTimer("MoverByTransform.MoveLinearByPosWithTime");
        //타이머 체크
        moveTimer += Time.deltaTime;
        if (moveTimer >= targetMoveTime)
        {
            moveTimer = targetMoveTime;
            isMoving = false;
        }

        //시간 비율 계산
        float Ilerp = Mathf.InverseLerp(0, targetMoveTime, moveTimer);

        //시간 비율에 따른 좌표 계산
        float moveX = Mathf.Lerp(posOrigin.x, targetPos.x, Ilerp);
        float moveY = Mathf.Lerp(posOrigin.y, targetPos.y, Ilerp);

        //이동
        Position = new Vector2(moveX, moveY);
        PerformanceManager.StopTimer("MoverByTransform.MoveLinearByPosWithTime");
    }
    private void MoveLinearByPosWithSpeed()
    {
        PerformanceManager.StartTimer("MoverByTransform.MoveLinearByPosWithSpeed");
        //타이머 체크
        moveTimer += Time.deltaTime;
        float targetDist = (targetPos - posOrigin).magnitude;//이동해야 하는 거리
        float targetTime = targetDist / targetSpeedF;//이동에 걸리는 시간


        if (moveTimer >= targetTime)
        {
            Position = targetPos;
            isMoving = false;
            return;
        }

        //시간 비율 계산
        float Ilerp = Mathf.InverseLerp(0, targetTime, moveTimer);

        //시간 비율에 따른 좌표 계산
        float moveX = Mathf.Lerp(posOrigin.x, targetPos.x, Ilerp);
        float moveY = Mathf.Lerp(posOrigin.y, targetPos.y, Ilerp);

        //이동
        Position = new Vector2(moveX, moveY);
        PerformanceManager.StopTimer("MoverByTransform.MoveLinearByPosWithSpeed");
    }
    private void MoveLinearBySpeed()
    {
        PerformanceManager.StartTimer("MoverByTransform.MoveLinearBySpeed");
        float deltaTime = Time.deltaTime;
        if (targetMoveTime > 0)
        {
            moveTimer += deltaTime;
            if (moveTimer >= targetMoveTime)
            {
                moveTimer = targetMoveTime;
                isMoving = false;
            }
        }

        Vector2 temp = Position;
        temp.x += deltaTime * targetSpeed.x;
        temp.y += deltaTime * targetSpeed.y;
        Position = temp;
        PerformanceManager.StopTimer("MoverByTransform.MoveLinearBySpeed");
    }

    private void MoveByFunction()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= targetMoveTime)
        {
            moveTimer = targetMoveTime;
            isMoving = false;
        }

        Position = MoveFunction(moveTimer);
    }
}
