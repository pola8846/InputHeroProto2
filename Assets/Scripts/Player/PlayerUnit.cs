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
    private float attackRange = 50f;
    [SerializeField]
    private float autoAim_mouse1 = 0.25f;
    [SerializeField]
    private LayerMask blockLayer;//오토에임을 가로막을 레이어
    [SerializeField]
    private GameObject trail;

    [SerializeField]
    private GameObject targetter;
    private GameObject targetterGO;
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
    private Vector2 ShootStartPos
    {
        get
        {
            return (Vector2)transform.position + Vector2.up * .5f;//총알 시작점
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
        targetterGO = Instantiate(targetter);
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
        if (TimeManager.IsSlowing)
        {
            return;
        }


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
                    //ShootToMouse();
                    Shoot();
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

    #region 원거리 기본공격
    //마우스 방향 표시기 회전(임시)
    private void RotateTargetter()
    {
        PerformanceManager.StartTimer("PlayerUnit.RotateTargetter");
        if (targetterGO is null)
        {
            PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
            return;
        }

        //마우스 위치 읽어오기
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        //거리 구하고 적용
        Vector2 dir = (worldMousePos - (transform.position + (Vector3)Vector2.up * .5f)).normalized * 2;
        targetterGO.transform.position = (Vector3)dir + transform.position + (Vector3)Vector2.up * .5f;
        PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
    }

    private void Shoot()
    {

        Projectile target = null;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        //마우스 주변 작은 원을 그려 쏠 수 있는 탄이 있으면 가까운 것 잡는다
        target = ProjectileManager.FindByDistance(worldMousePos, autoAim_mouse1);
        //경로가 확보된 탄이 있으면
        if (target != null && BulletPathCheck(target.transform.position))
        {
            //해당 탄을 찍고 공격한다
            Debug.Log(1);

            var temp = target.GetComponent<TestBulletChecker>();
            if (temp != null)
            {
                Debug.Log("Hit");
                var da = target.GetComponentInChildren<DamageArea>();
                if (da != null)
                {
                    da.Destroy();
                }
                else
                {
                    var prj = target.GetComponentInChildren<Projectile>();
                    if (prj != null)
                    {
                        prj.Parried();
                    }
                }
            }
            StartCoroutine(DrawHitGrapic(ShootStartPos, target.transform.position));
            return;
        }

        //추가로 오토에임 건다

        //오토에임 안 걸리면 직선으로 쏴서 닿는 것을 목표로 잡는다
        ShootByRay();
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
        Vector2 dir = (Vector2)targetterGO.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up) * (IsLookLeft ? -1 : 1);


        //방향대로 쏜다
        shooter.BulletAngle = angle;
        shooter.Triger();
        PerformanceManager.StopTimer("PlayerUnit.ShootToMouse");
    }

    private void ShootByRay()
    {
        //레이를 쏜다

        //쏠 방향을 구해온다
        Vector2 dir = ((Vector2)targetterGO.transform.position - ShootStartPos).normalized;//총알 방향

        //방향대로 쏜다
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | (1 << LayerMask.NameToLayer("Bullet"));
        var ray = Physics2D.RaycastAll(ShootStartPos, dir, attackRange, layer);
        Collider2D target = null;//충돌한 적법하고 가장 가까운 대상
        Vector2 hitPos = Vector2.zero;//충돌한 지점

        //유효하며 가장 가까운 것을 찾는다
        float minDistance = float.MaxValue;
        foreach (var hit in ray)
        {
            //적법하지 않은 대상 넘기기
            if (hit.collider.GetComponent<IBulletHitChecker>() == null &&
                hit.collider.GetComponent<Unit>() == null &&
                hit.collider.GetComponent<Projectile>() == null)
            {
                continue;
            }

            if (hit.transform == transform ||
                hit.transform.parent == transform)
            {
                continue;
            }

            //가장 가까운 것 찾기
            float distance = Vector2.Distance(ShootStartPos, hit.point);
            if (distance < minDistance)
            {
                target = hit.collider;
                hitPos = hit.point;
                minDistance = distance;
            }
        }

        //가로막혔는가?
        if (target == null)
        {
            //최대 사거리까지 발사
            //그래픽 표시
            StartCoroutine(DrawHitGrapic(ShootStartPos, ShootStartPos + (dir * attackRange)));
            //DrawHitGrapic(ShootStartPos, ShootStartPos + (dir * attackRange));
            return;
        }

        //적법한 대상에게 충돌
        //피해 적용
        HitCheck(target, hitPos);
        //그래픽 표시
        StartCoroutine(DrawHitGrapic(ShootStartPos, hitPos));
        //DrawHitGrapic(ShootStartPos, hitPos);
    }

    /// <summary>
    /// 해당 위치까지 총알이 갈 수 있는지(중간에 장애물이 없는지)
    /// </summary>
    /// <param name="targetPos">목표 위치</param>
    /// <returns>갈 수 있는가?</returns>
    private bool BulletPathCheck(Vector2 targetPos)
    {
        //레이를 쏜다
        var ray = Physics2D.LinecastAll(ShootStartPos, targetPos, blockLayer);
        //플레이어가 아닌 것에 닿으면 막힌다
        foreach (var hit in ray)
        {
            if (!hit.transform.CompareTag("Player"))
                return false;
        }

        //안 닿으면 안 막힌다
        return true;
    }

    /// <summary>
    /// 피격 처리
    /// </summary>
    /// <param name="target"></param>
    /// <param name="hitPos"></param>
    /// <returns></returns>
    private bool HitCheck(Collider2D target, Vector2 hitPos)
    {
        if (target == null)
        {
            return false;
        }

        // 충돌 처리
        Debug.Log(target.gameObject.name);

        //적 히트박스면
        HitBox hitBox = target.GetComponent<HitBox>();
        if (hitBox != null && target.CompareTag("Enemy"))
        {
            hitBox.Damage(1);
            //Hit(hitBox.Unit);
            return true;
        }

        //총알이면
        //if (Time.timeScale < 1)
        {
            var temp = target.GetComponent<TestBulletChecker>();
            if (temp != null)
            {
                Debug.Log("Hit");
                var da = target.GetComponentInChildren<DamageArea>();
                if (da != null)
                {
                    da.Destroy();
                    return true;
                }

                var prj = target.GetComponentInChildren<Projectile>();
                if (prj != null)
                {
                    prj.Parried();
                }
                return true;
            }
        }

        //기타 장애물이면
        if (true)
        {

        }

        // 충돌에 대한 처리 추가
        return false;
    }

    //총알 발사 그래픽 그리기. 표시되긴 하는데 안 이쁨
    private IEnumerator DrawHitGrapic(Vector2 start, Vector2 target)
    {
        GameObject go = Instantiate(trail);
        go.transform.position = start;
        go.GetComponent<TrailRenderer>().emitting = true;

        yield return null;

        go.transform.position = target;
        Destroy(go, go.GetComponent<TrailRenderer>().time);

        //Debug.DrawLine(start, target);
    }

    public void AttackEnd()
    {
        canMove = true;
    }
    #endregion

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
        Shoot();
        //ShootToMouse();
        yield return new WaitForSecondsRealtime(shootCooltime);
        canShoot = true;
    }
}
