using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUnit : Unit, IGroundChecker
{
    [Header("조작키")]
    [SerializeField]
    private KeyCode MoveL = KeyCode.A;
    [SerializeField]
    private KeyCode MoveR = KeyCode.D;
    [SerializeField]
    private KeyCode MoveU = KeyCode.W;
    [SerializeField]
    private KeyCode MoveD = KeyCode.S;
    [SerializeField]
    private KeyCode Jump = KeyCode.Space;
    [SerializeField]
    private KeyCode Attack = KeyCode.Z;
    [SerializeField]
    private KeyCode Attack2 = KeyCode.Mouse0;
    [SerializeField]
    private KeyCode Dash = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode Slow = KeyCode.LeftShift;
    [Header("기타")]
    [SerializeField]
    private Animator animator;
    private int canJumpCounter;
    private bool isJumping = false;
    private bool canMove = true;
    [SerializeField, Range(1f,5f)]
    private float dashSpeedRate = 2f;
    [SerializeField]
    private float dashTime = 1f;
    private bool isDashing = false;
    [SerializeField]
    private float slowRate = .5f;
    [SerializeField]
    private float slowTime = 4f;

    private bool isSlowed = false;


    [SerializeField]
    protected Transform groundCheckerLT;
    [SerializeField]
    protected Transform groundCheckerRD;
    [SerializeField]
    protected float groundCheckRadius = 0;
    [SerializeField]
    protected string groundLayer = "";
    [SerializeField]
    private GameObject targetter;

    private Mover mover;
    [SerializeField]
    private BulletShooter shooter;
    [SerializeField]
    private float shootCooltime;
    private bool canShoot = true;


    protected override void Start()
    {
        base.Start();
        mover = GetComponent<Mover>();
        GameManager.SetPlayer(this);
    }

    protected override void Update()
    {
        RotateTargetter();
        JumpCheck();
        if (canMove)
        {
            if (keyStay.ContainsKey(MoveL) && keyStay[MoveL])
            {
                mover.SetVelocityX(Mathf.Max(0, Speed) * -1);
                if (!isLookLeft)
                {
                    Turn();
                }
            }
            else if (keyStay.ContainsKey(MoveR) && keyStay[MoveR])
            {
                mover.SetVelocityX(Mathf.Max(0, Speed));
                if (isLookLeft)
                {
                    Turn();
                }
            }
            else
            {
                mover.StopMoveX();
            }
        }
        if (keyStay.ContainsKey(Attack2) && keyStay[Attack2] && canShoot)
        {
            StartCoroutine(DoShoot());
        }

        animator.SetFloat("MoveSpeedRate", Mathf.Abs(mover.Velocity.x) / stats.moveSpeed);
    }


    public override void KeyDown(KeyCode keyCode)
    {
        base.KeyDown(keyCode);

        if (keyCode == Dash)
        {
            StartCoroutine(DoDash());
            return;
        }

        if (keyCode == Jump)
        {
            if (canJumpCounter > 0 && canMove)
            {
                canJumpCounter--;
                mover.SetVelocityY(0);
                mover.AddForceY(JumpPower);
                isJumping = true;
                animator.SetBool("IsJumping", true);
            }
        }

        if (GroundCheck() == false && keyStay.ContainsKey(MoveD) && keyStay[MoveD])
        {
            //급강하
            mover.SetVelocityY(-mover.MaxSpeedY, true);
        }


        if (canMove && !animator.GetBool("IsJumping"))
        {
            if (keyCode == Attack)
            {
                animator.Play("mixamo_com");
                canMove = false;
            }
        }
        if (keyCode == Attack2 && canShoot)
        {
            StartCoroutine(DoShoot());
        }
        if (keyCode == Slow && isSlowed == false)
        {
            StartCoroutine(DoSlow());
        }
    }

    private void RotateTargetter()
    {
        PerformanceManager.StartTimer("PlayerUnit.RotateTargetter");
        //타게터 회전

        //마우스 위치 읽어오기
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        //거리 구하고 적용
        Vector2 dir = (worldMousePos - (transform.position + (Vector3)Vector2.up*.5f)).normalized * 2;
        targetter.transform.position = (Vector3)dir + transform.position + (Vector3)Vector2.up * .5f;
        PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
    }

    private void ShootToMouse()
    {
        PerformanceManager.StartTimer("PlayerUnit.ShootToMouse");

        //쏠 방향을 구해온다
        Vector2 dir = (Vector2)targetter.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up);

        if (isLookLeft)
        {
            angle *= -1;
        }

        //방향대로 쏜다
        shooter.bulletAngleMax = angle;
        shooter.bulletAngleMin = angle;
        shooter.triger = true;
        PerformanceManager.StopTimer("PlayerUnit.ShootToMouse");
    }

    public void AttackEnd()
    {
        canMove = true;
    }

    private void JumpCheck()
    {
        PerformanceManager.StartTimer("PlayerUnit.JumpCheck");
        if (isJumping && mover.Velocity.y <= -0.01f)
        {
            isJumping = false;
        }
        if (GroundCheck() && mover.Velocity.y <= 0.01f && !isJumping)
        {
            canJumpCounter = stats.jumpCount;
            animator.SetBool("IsJumping", false);   
        }
        else if (!GroundCheck() && canJumpCounter == stats.jumpCount)
        {
            canJumpCounter = stats.jumpCount - 1;
        }
        PerformanceManager.StopTimer("PlayerUnit.JumpCheck");
    }

    public bool GroundCheck()
    {
        PerformanceManager.StartTimer("PlayerUnit.GroundCheck");
        if (groundCheckerLT == null || groundCheckerRD == null)
        {
            return false;
        }
        bool result = Physics2D.OverlapArea(groundCheckerLT.position, groundCheckerRD.position, LayerMask.GetMask(groundLayer));
        PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
        return result;
    }

    private IEnumerator DoDash()
    {
        canMove = false;
        float speed = Mathf.Max(0,Speed) * dashSpeedRate * (isLookLeft ? -1 : 1);
        mover.SetVelocityX(speed);

        yield return new WaitForSeconds(dashTime);
        canMove = true;
    }

    private IEnumerator DoSlow()
    {
        isSlowed = true;
        Time.timeScale = slowRate;
        Time.fixedDeltaTime = Time.timeScale / 50;
        yield return new WaitForSecondsRealtime(slowTime);
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale / 50;
        isSlowed = false;
    }
    private IEnumerator DoShoot()
    {
        canShoot = false;
        ShootToMouse();
        yield return new WaitForSecondsRealtime(shootCooltime);
        canShoot = true;
    }
}
