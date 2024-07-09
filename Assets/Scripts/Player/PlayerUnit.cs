using FMODUnity;
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
    [SerializeField]
    private GameObject originPos;
    public Vector3 OriginPos => originPos.transform.position;

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
    public int CanJumpCounter => canJumpCounter;
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
    private bool canShootTemp = true;
    private bool CanShoot//공격할 수 있는가?
    {
        get
        {
            return canShootTemp && NowBullet >= 1 && shootTimer != null && shootTimer.Check(shootCooltime);
        }
    }
    [SerializeField]
    private float attackMinRange = 0.25f;//최소사거리(총알 표시 시작점)
    [SerializeField]
    private float attackRange = 50f;//사거리

    //마우스 각도
    private Vector2 nowMouseDir;
    private bool isNowMouseDirCashed = false;
    public Vector2 NowMouseDir
    {
        get
        {
            if (isNowMouseDirCashed)
            {
                return nowMouseDir;
            }
            else
            {
                //각도 계산
                nowMouseDir = (GameManager.MousePos - ShootStartPos).normalized;
                isNowMouseDirCashed = true;
                return nowMouseDir;
            }
        }
    }
    public float NowMouseAngle
    {
        get
        {
            return GameTools.GetDegreeAngleFormDirection(nowMouseDir);
        }
    }

    /// <summary>
    /// 마우스가 자기 기준 왼쪽에 있는가?
    /// </summary>
    public bool IsMouseLeft
    {
        get
        {
            return ShootStartPos.x < GameManager.MousePos.x;
        }
    }

    //자동조준
    [SerializeField]
    private float autoAim_mouse1 = 0.25f;
    [SerializeField]
    private float autoAim_cornAngle1 = 1f;
    [SerializeField]
    private float autoAim_sqrLength1 = 10f;
    [SerializeField]
    private float autoAim_sqrWidth1 = 1f;
    [SerializeField]
    private LayerMask blockLayer;//오토에임을 가로막을 레이어
    [SerializeField]
    private GameObject trail;//총알 트레일
    [SerializeField]
    private GameObject particle;//충돌 시 파티클

    //타겟 표시용 구체. 테스트용
    [SerializeField]
    private GameObject targetter;
    private GameObject targetterGO;

    //장탄
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

    //재장전 시간
    [SerializeField]
    private float reloadTime = 3f;
    private TickTimer reloadTimer;
    private TickTimer shootTimer;
    private bool reloading = false;
    public bool IsReloading => reloading;

    public bool isDash;

    public Vector2 ShootStartPos
    {
        get
        {
            //return (Vector2)transform.position + Vector2.up * .5f;//총알 시작점
            return OriginPos;
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
        reloadTimer = new();
        shootTimer = new(isTrigerInstant: true);
    }

    protected override void Update()
    {
        isNowMouseDirCashed = false;

        RotateTargetter();
        JumpCheck();
        ReloadCheck();
        InputMoveCheck();

        //Debug.Log($"{canShootTemp} && {NowBullet >= 1} && {shootTimer != null} && {shootTimer.Check(shootCooltime)}");
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

        if (inputType == InputType.Shoot && CanShoot)//공격
        {
            InputShoot();
        }

        //재장전
        if (inputType == InputType.Reload && canMove && canShootTemp && !TimeManager.IsUsingSkills)
        {
            if (!TimeManager.IsSlowed)
            {
                Reloading();
                //StartCoroutine(ReloadingTemp());
            }
            else
            {
                Reload();
            }
        }

        //슬로우
        if (inputType == InputType.Slow && TimeManager.IsSlowed == false)
        {
            //Reloading();
            //StopCoroutine(ReloadingTemp());
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

    private void InputMoveCheck()
    {
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
        if (IsKeyPushing(InputType.Shoot) && CanShoot && !TimeManager.IsSlowed)//총알 발사
        {
            InputShoot();
        }
    }

    #region 원거리 기본공격
    //마우스 방향 표시기 회전(임시)
    private void RotateTargetter()
    {
        if (targetterGO is null)
        {
            return;
        }

        //거리 구하고 적용
        targetterGO.transform.position = (NowMouseDir * 2) + ShootStartPos;
    }

    private void InputShoot()
    {
        if (!TimeManager.IsSlowed)
        {
            shootTimer.Reset();
        }
        Shoot();
    }

    private void Shoot()
    {

        if (NowBullet <= 0)
        {
            return;
        }
        else
        {
            NowBullet--;
        }
        RuntimeManager.PlayOneShot("event:/Bullet");
        RuntimeManager.PlayOneShot("event:/Bulletdrop");

        Projectile target = null;

        //마우스 주변 작은 원을 그려 쏠 수 있는 탄이 있으면 가까운 것 잡는다
        target = GameTools.FindClosest(GameManager.MousePos, ProjectileManager.FindByDistance(GameManager.MousePos, autoAim_mouse1));
        //쏠 수 있으면 쏜다
        if (ShootToTarget(target))
        {
            return;
        }

        /*
        //플레이어로부터 마우스 방향으로 작은 부채꼴을 그려 가까운 것 잡는다
        Vector2 dir = (Vector2)targetterGO.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up);//마우스 방향

        target = GameTools.FindClosest(transform.position, ProjectileManager.FindInCorn(transform.position, angle, autoAim_cornAngle1, attackRange));
        //쏠 수 있으면 쏜다
        if(ShootToTarget(target))
        {
            return;
        }
        */

        //찾은 탄 중에서 마우스에 가장 가까운 것을 찾는다
        target = GameTools.FindClosest(GameManager.MousePos, ProjectileManager.FindByFunc((Prj) =>
        {
            //사각형 내에 있는 탄을 찾는다
            Vector2 pointB = ShootStartPos + (NowMouseDir * autoAim_sqrLength1);
            return GameTools.IsPointInRhombus(Prj.transform.position, ShootStartPos, pointB, autoAim_sqrWidth1);
        }));
        if (ShootToTarget(target))
        {
            return;
        }

        //추가로 오토에임 건다

        //오토에임 안 걸리면 직선으로 쏴서 닿는 것을 목표로 잡는다
        ShootByRay();
    }

    //대상에게 탄 충돌 시도, 대상을 쏠 수 있으면 true
    private bool ShootToTarget(Projectile target)
    {
        if (target != null && BulletPathCheck(target.transform.position))
        {
            //해당 탄을 찍고 공격한다
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
            DrawHitGrapicAtMinRange(target.transform.position);
            return true;
        }
        return false;
    }

    private void ShootByRay()
    {
        //방향대로 쏜다
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | (1 << LayerMask.NameToLayer("Bullet")) | (1 << LayerMask.NameToLayer("Ground"));
        var ray = Physics2D.RaycastAll(ShootStartPos, NowMouseDir, attackRange, layer);
        Collider2D target = null;//충돌한 적법하고 가장 가까운 대상
        Vector2 hitPos = Vector2.zero;//충돌한 지점
        Vector2 normal = Vector2.zero;//법선

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
                normal = hit.normal;
                minDistance = distance;
            }
        }

        //가로막혔는가?
        if (target == null)
        {
            //최대 사거리까지 발사
            //그래픽 표시
            DrawHitGrapicAtMinRange(ShootStartPos + (NowMouseDir * attackRange));
            //DrawHitGrapic(ShootStartPos, ShootStartPos + (dir * attackRange));
            return;
        }

        //적법한 대상에게 충돌
        //피해 적용
        var pgo = HitCheck(target, hitPos);
        //그래픽 표시
        DrawHitGrapicAtMinRange(hitPos);
        DrawParticle(pgo, ShootStartPos, hitPos, Vector2.Reflect(NowMouseDir, normal));
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
    /// <returns>발생할 이펙트 게임오브젝트</returns>
    private GameObject HitCheck(Collider2D target, Vector2 hitPos)
    {
        if (target == null)
        {
            return null;
        }

        // 충돌 처리

        //적 히트박스면
        HitBox hitBox = target.GetComponent<HitBox>();
        if (hitBox != null && target.CompareTag("Enemy"))
        {
            hitBox.Damage(1);
            //Hit(hitBox.Unit);
            return null;
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
                    return null;
                }

                var prj = target.GetComponentInChildren<Projectile>();
                if (prj != null)
                {
                    prj.Parried();
                }
                return particle;
            }
        }

        //기타 장애물이면
        if (true)
        {

        }

        // 충돌에 대한 처리 추가
        return particle;
    }

    private void DrawHitGrapicAtMinRange(Vector2 target)
    {
        var dist = target - ShootStartPos;
        dist.Normalize();

        StartCoroutine(DrawHitGrapic(ShootStartPos + dist * attackMinRange, target));
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

    private void DrawParticle(GameObject particle, Vector2 start, Vector2 target, Vector2 dir)
    {
        if (particle != null)
        {
            GameObject pgo = Instantiate(particle);
            pgo.transform.position = target + (start - target).normalized * 0.2f;
            pgo.transform.LookAt(pgo.transform.position + (Vector3)dir);
            RuntimeManager.PlayOneShot("event:/BulletBlocked");
            Destroy(pgo, pgo.GetComponent<ParticleSystem>().main.duration);
        }
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
            //animator.SetBool("IsJumping", false);
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
        //animator.SetBool("IsJumping", true);
    }


    //재장전
    public void Reload()
    {
        NowBullet = maxBullet;
        RuntimeManager.PlayOneShot("event:/Realod_End");
    }

    private void Reloading()
    {
        canShootTemp = false;
        reloading = true;
        reloadTimer.Reset();
        RuntimeManager.PlayOneShot("event:/Reload_start");

        //Debug.Log("재장전");
    }

    private void ReloadCheck()
    {
        if (reloading && reloadTimer.Check(reloadTime))
        {
            reloading = false;
            Reload();
            reloadTimer.Reset();
            canShootTemp = true;
            //Debug.Log("재장전완료");
        }
    }

    private void CancelReload()
    {
        //Reloading();

        //StopCoroutine(ReloadingTemp());
        Debug.Log("dd");
        reloading = false;
        reloadTimer.Reset();
        canShootTemp = true;
    }

    //대시
    private IEnumerator DoDash()
    {
        Debug.Log("dash");
        canShootTemp = false;
        isDash = true;
        canMove = false;
        float speed = Mathf.Max(0, Speed) * dashSpeedRate * (isLookLeft ? -1 : 1);
        MoverV.SetVelocityX(speed);
        if (reloading)
        {
            CancelReload();
        }

        yield return new WaitForSeconds(dashTime);
        canMove = true;
        isDash = false;
        canShootTemp = true;
    }

    public override void Turn()
    {
        isLookLeft = !isLookLeft;
    }
}
