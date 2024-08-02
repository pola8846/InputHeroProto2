using UnityEngine;

[RequireComponent(typeof(Mover))]
public class TestEnemy_Gr_M_1 : Enemy
{
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private float attackWaitTime = 1f;
    [SerializeField]
    private float attackTime = 1f;
    [SerializeField]
    private float attackMoveSpeedRate = 2f;
    [SerializeField]
    private float attackCoolTime = 1f;
    [SerializeField]
    private int state = 0;
    private float timer1;
    private float timer2;
    private float timer3;
    private Renderer renderer;
    private Color originColor;

    public GameObject atk;
    private GameObject atkGO;
    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        originColor = renderer.material.color;
        timer1 = Time.time;
        timer2 = Time.time;
        timer3 = Time.time;
    }

    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case 0:
                break;
            case 1:
                if (timer1 + attackWaitTime <= Time.time)//���� ��� �ð� ������
                {
                    SetState(2);
                }
                break;
            case 2:
                if (timer2 + attackTime <= Time.time)
                {
                    SetState(3);
                }
                break;
            case 3:
                if (timer3 + attackCoolTime <= Time.time)
                {
                    SetState(0);
                }
                break;
            default:
                break;
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        bool isRight = GameManager.Player.transform.position.x >= transform.position.x;//�÷��̾ �����ʿ� �ִ°�?
        switch (state)
        {
            case 0://����
                if (FindPlayer())
                {
                    float movementX = Mathf.Max(0, Speed) * (isRight ? 1 : -1);

                    if (isRight == isLookLeft)
                    {
                        Turn();
                    }
                    MoverV.SetVelocityX(movementX);

                    if (AttackRangeCheck())//���� ��Ÿ� ���� ���� ������Ʈ 1��
                    {
                        MoverV.StopMoveX();
                        SetState(1);
                        timer1 = Time.time;
                        SetColor(Color.yellow);
                    }
                }
                else
                {
                    MoverV.StopMoveX();
                }
                break;
            case 1://�÷��̾ ���� ��Ÿ� ���� ���� ��
                break;
            case 2://���� ���� ��
                {
                    float movementX = Mathf.Max(0, stats.moveSpeed * attackMoveSpeedRate) * (isLookLeft ? -1 : 1);
                    MoverV.SetVelocityX(movementX);
                }
                break;
            case 3://���� ��Ÿ���� ��
                break;
            default:
                break;
        }

    }

    private void SetState(int num)
    {
        switch (state)
        {
            case 1:
                SetColor(Color.red);
                timer2 = Time.time;
                atkGO = Instantiate(atk, transform);
                atkGO.GetComponent<Attack>().Initialization(this, "Player", atkGO);
                break;
            case 2:
                timer3 = Time.time;
                MoverV.StopMoveX();
                Destroy(atkGO);
                break;
            case 3:
                SetColor(Color.clear);
                break;
            default:
                break;
        }

        state = num;

    }

    private bool AttackRangeCheck()
    {
        return Vector3.Distance(transform.position, GameManager.Player.transform.position) <= attackRange;
    }

    private void SetColor(Color color)
    {
        if (color == Color.clear)
        {
            renderer.material.color = originColor;
        }
        else
        {
            renderer.material.color = color;
        }
    }
}
