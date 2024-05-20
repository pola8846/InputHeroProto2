using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class PlayerUnit : Unit, IGroundChecker, IMoveReceiver
{
    //[Header("조작키")]
    protected Dictionary<InputType, bool> keyStay = new();

    [Header("기타")]
    [SerializeField]
    private Animator animator;
    private bool canMove = true;

    [SerializeField, Range(1f, 5f)]
    private float dashSpeedRate = 2f;
    [SerializeField]
    private float dashTime = 1f;
    private bool isDashing = false;

    [SerializeField]
    public GameObject areaAttackPrefab;

    [Header("점프")]
    [SerializeField]
    protected Transform groundCheckerLT;
    [SerializeField]
    protected Transform groundCheckerRD;
    [SerializeField]
    private GameObject groundChecker;
    private Collider2D groundCheckerCollider;
    [SerializeField]
    protected float groundCheckRadius = 0;
    [SerializeField]
    protected string groundLayer = "";
    [SerializeField]
    protected string halfGroundLayer = "";
    private bool isDownJumping = false;
    private PlatformEffector2D effector2D;

    private int canJumpCounter;
    private bool isJumping = false;


    [Header("원거리 공격")]
    [SerializeField]
    private BulletShooter shooter;
    [SerializeField]
    private BulletShooter shooter_Big;
    public BulletShooter Shooter_Big => shooter_Big;
    [SerializeField]
    private float shootCooltime;
    private bool canShoot = true;
    [SerializeField]
    private GameObject targetter;
    [SerializeField]
    private int maxBullet;
    public int MaxBullet => maxBullet;
    [SerializeField]
    private int nowBullet;
    public int NowBullet
    {
        get => nowBullet;
        set
        {
            nowBullet = Mathf.Clamp(value, 0, maxBullet);
            UIManager.SetBulletCounter(nowBullet);
        }
    }


    //스킬
    private List<PlayerSkill> skillList;
    public List<PlayerSkill> SkillList => skillList;


    protected override void Start()
    {
        base.Start();
        effector2D = GetComponent<PlatformEffector2D>();
        if (groundChecker != null)
        {
            groundCheckerCollider = groundChecker.GetComponent<Collider2D>();
        }
        GameManager.SetPlayer(this);
        skillList = new List<PlayerSkill>
        {
            new PSkill_TestAreaAtk(),
            new PSkill_TestDash(),
            new PSkill_TestRangeAtk()
        };
    }

    protected override void Update()
    {
        PerformanceManager.StartTimer("PlayerUnit.Update");
        RotateTargetter();
        JumpCheck();
        if (canMove && !TimeManager.IsSlowed)
        {
            if (IsKeyPushing(InputType.MoveLeft))
            {
                MoverV.SetVelocityX(Mathf.Max(0, Speed) * -1);
                if (!isLookLeft)
                {
                    Turn();
                }
            }
            else if (IsKeyPushing(InputType.MoveRight))
            {
                MoverV.SetVelocityX(Mathf.Max(0, Speed));
                if (isLookLeft)
                {
                    Turn();
                }
            }
            else
            {
                MoverV.StopMoveX();
            }
        }
        if (IsKeyPushing(InputType.Shoot) && canShoot)
        {
            if (!TimeManager.IsSlowed)
            {
                StartCoroutine(DoShoot());
            }
        }

        animator.SetFloat("MoveSpeedRate", Mathf.Abs(MoverV.Velocity.x) / stats.moveSpeed);
        PerformanceManager.StopTimer("PlayerUnit.Update");
    }


    public void KeyDown(InputType inputType)
    {
        //입력 검사
        if (keyStay.ContainsKey(inputType))
        {
            keyStay[inputType] = true;
        }
        else
        {
            keyStay.Add(inputType, true);
        }

        if (TimeManager.IsSlowed && !TimeManager.IsUsingSkills)
        {
            ComboManager.InputLog(inputType);
        }

        //대시
        if (inputType == InputType.Dash)
        {
            StartCoroutine(DoDash());
            return;
        }

        //점프
        if (!isDownJumping && ((inputType == InputType.Jump && IsKeyPushing(InputType.MoveDown)) || (inputType == InputType.MoveDown && IsKeyPushing(InputType.Jump))))
        {
            SetHalfDownJump(true);
        }
        else if (inputType == InputType.Jump)
        {
            if (canJumpCounter > 0 && canMove)
            {
                Jump();
            }
        }

        //급강하
        if (GroundCheck() == false && IsKeyPushing(InputType.MoveDown))
        {
            //급강하
            MoverV.SetVelocityY(-MoverV.MaxSpeedY, true);
        }

        //공격
        if (canMove && !animator.GetBool("IsJumping"))
        {
            if (inputType == InputType.MeleeAttack)
            {
                animator.Play("mixamo_com");
                canMove = false;
            }
        }
        if (inputType == InputType.Shoot && canShoot)
        {
            if (TimeManager.IsSlowed)
            {
                if (!TimeManager.IsUsingSkills)
                {
                    ShootToMouse();
                }
            }
            else
            {
                StartCoroutine(DoShoot());
            }
        }

        if (inputType == InputType.Reload && canMove && canShoot && !TimeManager.IsUsingSkills)
        {
            if (!TimeManager.IsSlowed)
            {
                StartCoroutine(Reloading());
            }
            else
            {
                Reload();
            }
        }

        //슬로우
        if (inputType == InputType.Slow && TimeManager.IsSlowed == false)
        {
            StopCoroutine(Reloading());
            TimeManager.StartSlow();
        }
    }


    public void KeyUp(InputType inputType)
    {
        //입력 검사
        if (keyStay.ContainsKey(inputType))
        {
            keyStay[inputType] = false;
        }
        else
        {
            keyStay.Add(inputType, false);
        }

        //점프
        if (isDownJumping && (inputType == InputType.Jump || inputType == InputType.MoveDown))
        {
            SetHalfDownJump(false);
        }
    }

    public void KeyReset()
    {
        foreach (var item in keyStay)
        {
            KeyUp(item.Key);
        }
        keyStay.Clear();
    }

    //마우스 방향 표시기 회전(임시)
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
        Vector2 dir = (worldMousePos - (transform.position + (Vector3)Vector2.up * .5f)).normalized * 2;
        targetter.transform.position = (Vector3)dir + transform.position + (Vector3)Vector2.up * .5f;
        PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
    }

    //마우스 방향으로 발사(임시)
    private void ShootToMouse()
    {
        PerformanceManager.StartTimer("PlayerUnit.ShootToMouse");

        if (NowBullet <= 0)
        {
            return;
        }
        else
        {
            NowBullet--;
        }

        //쏠 방향을 구해온다
        Vector2 dir = (Vector2)targetter.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up) * -1;


        //방향대로 쏜다
        shooter.BulletAngle = angle;
        shooter.Triger();
        PerformanceManager.StopTimer("PlayerUnit.ShootToMouse");
    }

    public void AttackEnd()
    {
        canMove = true;
    }

    //점프 가능 체크
    private void JumpCheck()
    {
        PerformanceManager.StartTimer("PlayerUnit.JumpCheck");
        if (isJumping && MoverV.Velocity.y <= -0.01f)
        {
            isJumping = false;
        }
        if (GroundCheck() && MoverV.Velocity.y <= 0.01f && !isJumping)
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

    //땅 체크
    public bool GroundCheck()
    {
        PerformanceManager.StartTimer("PlayerUnit.GroundCheck");

        if (groundChecker == null)
        {
            PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
            return false;
        }

        int layer = LayerMask.GetMask(groundLayer, halfGroundLayer);
        if (groundCheckerCollider.IsTouchingLayers(layer))
        {
            PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
            return true;
        }

        PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
        return false;
    }

    //키가 눌리고 있는지
    private bool IsKeyPushing(InputType inputType)
    {
        return keyStay.ContainsKey(inputType) && keyStay[inputType];
    }


    //아래 점프
    private void SetHalfDownJump(bool isDownJumping)
    {
        int layerIndex = LayerMask.NameToLayer(halfGroundLayer);
        this.isDownJumping = isDownJumping;
        if (isDownJumping)
        {
            effector2D.colliderMask &= ~(1 << layerIndex);
        }
        else
        {
            effector2D.colliderMask |= (1 << layerIndex);
        }
    }

    //점프
    private void Jump()
    {
        canJumpCounter--;
        MoverV.SetVelocityY(0, true);
        MoverV.AddForceY(JumpPower);
        isJumping = true;
        animator.SetBool("IsJumping", true);
    }


    //재장전
    public void Reload()
    {
        NowBullet = maxBullet;
    }

    private IEnumerator Reloading()
    {
        canShoot = false;
        Debug.Log("재장전");
        yield return new WaitForSeconds(3f);
        Reload();
        canShoot = true;
    }

    //대시
    private IEnumerator DoDash()
    {
        canMove = false;
        float speed = Mathf.Max(0, Speed) * dashSpeedRate * (isLookLeft ? -1 : 1);
        MoverV.SetVelocityX(speed);

        yield return new WaitForSeconds(dashTime);
        canMove = true;
    }


    //원거리 평타
    private IEnumerator DoShoot()
    {
        canShoot = false;
        ShootToMouse();
        yield return new WaitForSecondsRealtime(shootCooltime);
        canShoot = true;
    }

}
