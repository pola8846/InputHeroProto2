using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private Rigidbody2D rb;
    private float originGravity;

    //��ǥ �ӵ�
    private float targetSpeedX = 0;
    public float TargetSpeedX => targetSpeedX;
    private float targetSpeedY = 0;
    public float TargetSpeedY => targetSpeedY;

    //�ִ� �ӵ�
    [SerializeField]
    private float maxSpeedX = -1;
    public float MaxSpeedX
    { get { return maxSpeedX; } set { maxSpeedX = value; } }

    [SerializeField]
    private float maxSpeedY = -1;
    public float MaxSpeedY
    { get { return maxSpeedY; } set { maxSpeedY = value; } }

    //�ӵ� ����
    [SerializeField]
    private bool fixSpeedX = false;
    [SerializeField]
    private bool fixSpeedY = false;

    //�ִ� �ӵ� ����. true�� �ִ� �ӵ� �̻����� ���� ����
    public bool speedCap = true;

    //�� ��踦 ��� �� �ִ°�?
    [SerializeField]
    private bool canMoveOverMapLimit = true;

    //�� ��� ������
    [SerializeField]
    private float MapLimitExtra = 0;

    /// <summary>
    /// �ӵ�
    /// </summary>
    public Vector2 Velocity
    {
        get { return rb.velocity; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originGravity = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        //�̵�
        if (fixSpeedX)
        {
            SetVelocityX();

        }
        if (fixSpeedY)
        {
            SetVelocityY();
        }

        //�ִ� �ӵ� üũ
        if (speedCap)
        {
            MaxSpeedCheck();
        }

        //�� ��� üũ
        if (!canMoveOverMapLimit)
        {
            transform.position = GameTools.ClampToRect(transform.position, GameManager.MapLimit, MapLimitExtra);
        }
    }

    /// <summary>
    /// �ִ� �ӵ� üũ
    /// </summary>
    private void MaxSpeedCheck()
    {
        if (maxSpeedX >= 0)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeedX, maxSpeedX), rb.velocity.y);
        }
        if (maxSpeedY >= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeedY, maxSpeedY));
        }
    }

    /// <summary>
    /// �ӵ� �缳��(X�ุ)
    /// </summary>
    private void SetVelocityX()
    {
        rb.velocity = new Vector2(targetSpeedX, rb.velocity.y);
        if (speedCap)
        {
            MaxSpeedCheck();
        }
    }

    /// <summary>
    /// �ӵ� �缳��(X�ุ)
    /// </summary>
    /// <param name="speed">������ �ӵ�</param>
    public void SetVelocityX(float speed, bool once = false)
    {
        targetSpeedX = speed;
        SetVelocityX();
        fixSpeedX = !once;
    }

    /// <summary>
    /// �̵� ����(X�ุ)
    /// </summary>
    public void StopMoveX(bool instant = true)
    {
        fixSpeedX = false;
        targetSpeedX = 0;
        if (instant)
        {
            SetVelocityX();
        }
    }

    /// <summary>
    /// �ӵ� �缳��(Y�ุ)
    /// </summary>
    private void SetVelocityY()
    {
        rb.velocity = new Vector2(rb.velocity.x, targetSpeedY);
        if (speedCap)
        {
            MaxSpeedCheck();
        }
    }

    /// <summary>
    /// �ӵ� �缳��(Y�ุ)
    /// </summary>
    /// <param name="speed">������ �ӵ�</param>
    public void SetVelocityY(float speed, bool once = false)
    {
        targetSpeedY = speed;
        SetVelocityY();
        fixSpeedY = !once;
    }

    /// <summary>
    /// �̵� ����(Y�ุ)
    /// </summary>
    public void StopMoveY(bool instant = true)
    {
        fixSpeedY = false;
        targetSpeedY = 0;
        if (instant)
        {
            SetVelocityY();
        }
    }

    /// <summary>
    /// �ӵ� �缳��
    /// </summary>
    /// <param name="velocity">�缳���� �ӵ�</param>
    public void SetVelocity(Vector2 velocity, bool once = false)
    {
        SetVelocityX(velocity.x, once);
        SetVelocityY(velocity.y, once);
    }

    /// <summary>
    /// �ӵ� �缳��
    /// </summary>
    /// <param name="velocity">�缳���� �ӵ�</param>
    public void SetVelocity(Vector3 velocity)
    {
        SetVelocity((Vector2)velocity);
    }

    /// <summary>
    /// �̵� ����
    /// </summary>
    public void StopMove(bool instant = true)
    {
        fixSpeedX = false;
        fixSpeedY = false;
        targetSpeedX = 0;
        targetSpeedY = 0;
        if (instant)
        {
            SetVelocity(Vector2.zero, true);
        }
    }

    /// <summary>
    /// AddForce ��ü
    /// </summary>
    /// <param name="force">���� ��(�� ����)</param>
    public void AddForceX(float force)
    {
        fixSpeedX = true;
        rb.velocity = new Vector2(rb.velocity.x + force, rb.velocity.y);
    }

    /// <summary>
    /// AddForce ��ü
    /// </summary>
    /// <param name="force">���� ��(������ ����)</param>
    public void AddForceY(float force)
    {
        fixSpeedY = false;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + force);
    }

    /// <summary>
    /// AddForce ��ü
    /// </summary>
    /// <param name="force">���� ��</param>
    public void AddForce(Vector2 force)
    {
        fixSpeedX = false;
        fixSpeedY = false;
        rb.velocity = new Vector2(rb.velocity.x + force.x, rb.velocity.y + force.y);
    }

    /// <summary>
    /// �߷� ��ȯ
    /// </summary>
    /// <param name="isOn">�߷��� �� ���ΰ�?</param>
    public void TurnGravity(bool isOn)
    {
        if (isOn)
        {
            rb.gravityScale = originGravity;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }
}