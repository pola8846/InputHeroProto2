using System;
using UnityEngine;

public class MoverByTransform : MonoBehaviour
{
    /// <summary>
    /// ���� ��ǥ�� ����ϴ°�?
    /// </summary>
    [SerializeField]
    private bool isSetByLocal = true;
    /// <summary>
    /// gameobject.position ��ü. isSetByLocal�� true�� ���� ��ǥ�� ���
    /// </summary>
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
    /// �̵� Ÿ��
    /// </summary>
    public enum moveType
    {
        LinearByPosWithTime,
        LinearByPosWithSpeed,
        LinearBySpeed,
        ByFunction,
    }
    private moveType type;

    //��ġ ��� �̵� ��
    //��ǥ ��ġ
    private Vector2 targetPos;
    private float targetMoveTime = 0;
    private Vector2 posOrigin = Vector2.zero;

    //�ӵ� ��� �̵� ��
    //��ǥ �ӵ�
    private Vector2 targetSpeed;
    private float targetSpeedF = 0;

    //�Լ� ��� ��
    Func<float, Vector2> MoveFunction;

    [SerializeField]
    private float moveTimer;//�̵� Ȯ�ο� Ÿ�̸�. ticktimer ����� ���� ���� float�� ��� ��
    [SerializeField]
    private bool isMoving = false;//�̵� ���ΰ�?
    public bool IsMoving => isMoving;

    //�� ��踦 ��� �� �ִ°�?
    [SerializeField]
    private bool canMoveOverMapLimit = true;
    //�� ��� ������
    [SerializeField]
    private float MapLimitExtra = 0;

    private void Update()
    {

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
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public void StartMove(moveType type, Vector2 target, params float[] options)
    {
        moveTimer = 0;
        this.type = type;
        posOrigin = Position;
        isMoving = true;

        switch (type)
        {
            case moveType.LinearByPosWithTime:
                targetPos = target;//�̵��� ��ġ
                targetMoveTime = Mathf.Max(options[0], 0);//�ɸ��� �ð�

                break;

            case moveType.LinearByPosWithSpeed:
                targetPos = target;//�̵��� ��ġ
                targetSpeedF = options[0];//�̵� �ӵ�
                break;

            case moveType.LinearBySpeed:
                targetSpeed = target;//�̵��� ���� ����
                targetMoveTime = options[0];//�̵� �ӵ�

                break;
            default:
                Debug.LogError("�� �� ���� �̵� ����");
                break;
        }
    }

    /// <summary>
    /// ���޹��� �Լ��� ���� ������ �˵��� �̵�.
    /// ���� ���� � ����� �����̾����� ��ȹ �������� �̻��
    /// </summary>
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
                Debug.LogError("�� �� ���� �̵� ����");
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
        //Ÿ�̸� üũ
        moveTimer += Time.deltaTime;
        if (moveTimer >= targetMoveTime)
        {
            moveTimer = targetMoveTime;
            isMoving = false;
        }

        //�ð� ���� ���
        float Ilerp = Mathf.InverseLerp(0, targetMoveTime, moveTimer);

        //�ð� ������ ���� ��ǥ ���
        float moveX = Mathf.Lerp(posOrigin.x, targetPos.x, Ilerp);
        float moveY = Mathf.Lerp(posOrigin.y, targetPos.y, Ilerp);

        //�̵�
        Position = new Vector2(moveX, moveY);
    }
    private void MoveLinearByPosWithSpeed()
    {
        //Ÿ�̸� üũ
        moveTimer += Time.deltaTime;
        float targetDist = (targetPos - posOrigin).magnitude;//�̵��ؾ� �ϴ� �Ÿ�
        float targetTime = targetDist / targetSpeedF;//�̵��� �ɸ��� �ð�


        if (moveTimer >= targetTime)
        {
            Position = targetPos;
            isMoving = false;
            return;
        }

        //�ð� ���� ���
        float Ilerp = Mathf.InverseLerp(0, targetTime, moveTimer);

        //�ð� ������ ���� ��ǥ ���
        float moveX = Mathf.Lerp(posOrigin.x, targetPos.x, Ilerp);
        float moveY = Mathf.Lerp(posOrigin.y, targetPos.y, Ilerp);

        //�̵�
        Position = new Vector2(moveX, moveY);
    }
    private void MoveLinearBySpeed()
    {
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
