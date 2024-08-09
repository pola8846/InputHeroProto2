using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class PlayerUnit : Unit, IGroundChecker, IMoveReceiver
{
    [Header("기타")]
    private bool canMove = true; //이동 가능한가?
    public bool CanMove
    {
        get { return canMove; }
    }

    [SerializeField]
    private GameObject originPos;//좌표 계산 기준 위치
    public Vector3 OriginPos => originPos.transform.position;

    [SerializeField]
    private float glitchTime = 0.5f;//피격 시 글리치 이펙트 유지 시간
    [SerializeField]
    private GameObject gameoverText;//사망 시 활성화할 오브젝트(게임 오버 이미지)

    [Header("대시")]
    [SerializeField, Range(1f, 5f)]
    private float dashSpeedRate = 2f;//대시가 일반 속도의 몇 배인지
    [SerializeField]
    private float dashTime = 1f;//대시유지시간
    private TickTimer dashTimer;
    [SerializeField, Range(0f, 1f)]
    private float dashSlowRate = 1f;//대시가 슬로우의 영향 받는 비율

    [Header("점프")]
    //바닥 체크 용
    [SerializeField]
    private Collider2D groundCheckerCollider;
    [SerializeField]
    protected string groundLayer = "";//바닥 레이어
    [SerializeField]
    protected string halfGroundLayer = "";//반벽 레이어
    private bool isDownJumping = false;//아래 점프 도중인가?
    private PlatformEffector2D effector2D;

    private int canJumpCounter;//남은 점프 회수 체크용(2단 이상 점프 시)
    private bool isJumping = false;//점프 중인가?
    public bool IsJumping => isJumping;


    [Header("원거리 공격")]
    [SerializeField]
    private float shootCooltime;//공격 쿨타임
    [SerializeField, Range(0, 1)]
    private float shootSlowRate = 1;//공격이 슬로우에 영향 받는 비율
    private TickTimer shootTimer;//공격용 타이머
    private bool canShootTemp = true;//일시적으로 공격 불가능하게 만들 때 쓰는 플래그
    private bool CanShoot//공격할 수 있는가?
    {
        get
        {
            return canShootTemp && NowBullet >= 1 && shootTimer != null && shootTimer.Check(shootCooltime, shootSlowRate);
        }
    }
    [SerializeField]
    private float attackMinRange = 0.25f;//최소사거리(총알 표시 시작점)
    [SerializeField]
    private float attackRange = 50f;//사거리

    //마우스 각도
    private Vector2 nowMouseDir;//플레이어 기준으로 마우스까지의 방향 벡터
    private bool isNowMouseDirCashed = false;//캐시 체크용 플래그
    public Vector2 NowMouseDir//플레이어 기준으로 마우스까지의 방향 벡터
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
    public float NowMouseAngle//마우스까지의 방향을 각도로 받아옴
    {
        get
        {
            return GameTools.GetDegreeAngleFormDirection(NowMouseDir);
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
    private float autoAim_mouse1 = 0.25f;//마우스 주변에서 찾을 거리
    [SerializeField]
    private float autoAim_cornAngle1 = 1f;//마우스 방향의 찾을 범위 부채꼴 범위
    [SerializeField]
    private float autoAim_sqrLength1 = 10f;//마우스 방향의 찾을 범위 사각형 길이
    [SerializeField]
    private float autoAim_sqrWidth1 = 1f;//마우스 방향의 찾을 범위 사각형 두께
    [SerializeField]
    private LayerMask blockLayer;//오토에임을 가로막을 레이어
    [SerializeField]
    private GameObject trail;//총알 트레일
    [SerializeField]
    private GameObject particle;//충돌 시 파티클


    //장탄
    [SerializeField]
    private int maxBullet;//최대 탄환 수
    public int MaxBullet => maxBullet;
    [SerializeField]
    private int nowBullet;//현재 탄환 수
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
    private float reloadTime = 3f;//재장전에 걸리는 시간
    public float ReloadTime => reloadTime;
    public float RemainReloadTime => reloadTimer.GetRemain(ReloadTime);//재장전 완료까지 남은 시간
    private TickTimer reloadTimer;//재장전용 타이머
    [SerializeField, Range(0f,1f)]
    private float reloadSlowRate = 1;//재장전이 슬로우의 영향을 얼마나 받을지?
    private bool reloading = false;//재장전 중인가?
    public bool IsReloading => reloading;

    public bool isDash;//대시 중인가?

    public Vector2 ShootStartPos//총알 발사 시작점
    {
        get
        {
            return OriginPos;
        }
    }

    // FSM
    private StateMachine mainStateMachine;
    private StateMachine downStateMachine;
    public string currentMainState;
    public string currentDownState;

    [Header("애니메이션")]
    //애니메이션
    [SerializeField]
    private Upper_Animator upperAni;//상체
    [SerializeField]
    private Down_Animator downAni;//하체


    protected override void Start()
    {
        base.Start();
        effector2D = GetComponent<PlatformEffector2D>();
        GameManager.SetPlayer(this);
        InputManager.EnrollReciver(this);
        reloadTimer = new();
        dashTimer = new();
        shootTimer = new(isTrigerInstant: true);

        upperAni = GetComponentInChildren<Upper_Animator>();
        downAni = GetComponentInChildren<Down_Animator>();

        // FSM
        mainStateMachine = new(this);
        mainStateMachine.ChangeState<PlayerMain_aim>();

        downStateMachine = new(this);
        downStateMachine.ChangeState<PlayerDown_idle>();
    }

    protected override void Update()
    {
        isNowMouseDirCashed = false;

        JumpCheck();
        ReloadCheck();
        //InputMoveCheck();
        Animate();

        // FSM
        mainStateMachine.Update();
        currentMainState = mainStateMachine.GetCurrentState();
        downStateMachine.Update();
        currentDownState = downStateMachine.GetCurrentState();

        Debug.Log(TickTimer.GetConvertedTimeRate(shootSlowRate));
    }


    /// <summary>
    /// 키 눌렸을 때의 처리
    /// </summary>
    /// <param name="inputType">눌린 키</param>
    public void KeyDown(InputType inputType)
    {
        //입력 검사
        if (TimeManager.IsSlowing)
        {
            return;
        }

        switch (inputType)
        {
            case InputType.MoveLeft:
                break;
            case InputType.MoveRight:
                break;
            case InputType.MoveUp:
                break;
            case InputType.MoveDown:
                if (InputManager.IsKeyPushing(InputType.Jump))//아래점프
                    SetHalfDownJump(true);

                if (GroundCheck() == false)//급강하
                    MoverV.SetVelocityY(-MoverV.MaxSpeedY, true);
                break;

            case InputType.Jump://점프
                if (!isDownJumping && InputManager.IsKeyPushing(InputType.MoveDown))//아래 점프
                {
                    SetHalfDownJump(true);
                }
                else//점프
                {
                    if (canJumpCounter > 0 && canMove)
                    {
                        //Jump();
                    }
                }
                break;

            case InputType.Dash:
                //대시
                StartCoroutine(DoDash());

                break;
            case InputType.Shoot:
                if (CanShoot)//공격
                    InputShoot();
                else if (NowBullet <= 0)//총알 부족 시
                    UIManager.Instance.OnBulletUseFailed?.Invoke();//부족 알림 표시
                break;

            case InputType.Reload:
                //재장전
                if (canMove && !TimeManager.IsUsingSkills)
                {
                    //if (!TimeManager.IsSlowed)
                        Reloading();
                    //else
                    //    Reload();
                }
                break;

            case InputType.Slow:
                //슬로우
                if (TimeManager.IsSlowed == false)
                    TimeManager.StartSlow();
                else if(TimeManager.IsSlowing == false)
                    TimeManager.EndSlow();
                break;

            case InputType.MeleeAttack:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 키 뗄 때 처리
    /// </summary>
    /// <param name="inputType">뗀 키</param>
    public void KeyUp(InputType inputType)
    {

        //점프
        if (isDownJumping && (inputType == InputType.Jump || inputType == InputType.MoveDown))
        {
            SetHalfDownJump(false);
        }
    }

    /// <summary>
    /// Update마다 이동 체크
    /// </summary>
    //private void InputMoveCheck()
    //{
    //    if (canMove)//움직일 수 있고
    //    {
    //        if (InputManager.IsKeyPushing(InputType.MoveLeft))//왼쪽 누르고 있다면
    //        {
    //            MoverV.SetVelocityX(Mathf.Max(0, Speed) * -1);//이동
    //            if (!isLookLeft)//방향 전환
    //            {
    //                Turn();
    //            }
    //        }
    //        else if (InputManager.IsKeyPushing(InputType.MoveRight))//오른쪽 누르고 있다면
    //        {
    //            MoverV.SetVelocityX(Mathf.Max(0, Speed));//이동
    //            if (isLookLeft)//방향 전환
    //            {
    //                Turn();
    //            }
    //        }
    //        else//이동 키 조작 없으면
    //        {
    //            MoverV.StopMoveX();//정지
    //        }
    //    }
    //    if (InputManager.IsKeyPushing(InputType.Shoot) && CanShoot)//총알 발사
    //    {
    //        InputShoot();
    //    }
    //}

    /// <summary>
    /// 애니메이션 갱신
    /// </summary>
    private void Animate()
    {
        if (true)
        {
            upperAni.FlipCheck();
            downAni.FlipCheck();
        }
    }

    #region 원거리 기본공격
    /// <summary>
    /// 공격 키 입력
    /// </summary>
    private void InputShoot()
    {
        //if (!TimeManager.IsSlowed)
        {
            shootTimer.Reset(); 
        }
        Shoot();
    }

    /// <summary>
    /// 공격
    /// </summary>
    private void Shoot()
    {
        NowBullet--;
        UIManager.Instance.OnBulletNumUpdated?.Invoke();
        CancelReload();

        RuntimeManager.PlayOneShot("event:/Bullet");
        RuntimeManager.PlayOneShot("event:/Bulletdrop");

        //자동조준
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

        //오토에임 안 걸리면 직선으로 쏴서 닿는 것을 목표로 잡는다
        ShootByRay();
    }

    //목표에게 탄 충돌 시도, 대상을 쏠 수 있으면 true
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

    /// <summary>
    /// 자동 조준되지 않았을 때 레이를 통해 피격 처리
    /// </summary>
    private void ShootByRay()
    {
        //방향대로 쏜다
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | 
            (1 << LayerMask.NameToLayer("Bullet")) | 
            (1 << LayerMask.NameToLayer("Ground"));
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
            return;
        }

        //적법한 대상에게 충돌
        //피해 적용
        var pgo = HitCheck(target, hitPos);
        //그래픽 표시
        DrawHitGrapicAtMinRange(hitPos);
        DrawParticle(pgo, ShootStartPos, hitPos, Vector2.Reflect(NowMouseDir, normal));
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
    /// <param name="target">공격받은 대상</param>
    /// <param name="hitPos">충돌 지점</param>
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
            hitBox.Damage(1);//피해 입힌다
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
                    da.Destroy();//파괴
                    return null;
                }

                var prj = target.GetComponentInChildren<Projectile>();
                if (prj != null)
                {
                    prj.Parried();//패리
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

    /// <summary>
    /// 플레이어로부터 착탄 지점까지 발사 효과 그려주기
    /// </summary>
    /// <param name="target">착탄 지점</param>
    private void DrawHitGrapicAtMinRange(Vector2 target)
    {
        var dist = target - ShootStartPos;
        dist.Normalize();

        StartCoroutine(DrawHitGrapic(ShootStartPos + dist * attackMinRange, target));
    }

    //총알 발사 그래픽 그리기. 표시되긴 하는데 안 이쁨
    private IEnumerator DrawHitGrapic(Vector2 start, Vector2 target)
    {
        GameObject go = Instantiate(trail);//트레일을 생성한다
        go.transform.position = start;
        go.GetComponent<TrailRenderer>().emitting = true;

        yield return null;//트레일 처리를 위해 1프레임 대기

        //트레일 이동 후 제거
        go.transform.position = target;
        Destroy(go, go.GetComponent<TrailRenderer>().time);
    }

    /// <summary>
    /// 착탄 지점에 파티클 표시(지형일 경우)
    /// </summary>
    /// <param name="particle">생성할 파티클</param>
    /// <param name="start">발사 위치</param>
    /// <param name="target">착탄 위치</param>
    /// <param name="dir">파티클 방향</param>
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

    /// <summary>
    /// 공격 종료
    /// </summary>
    public void AttackEnd()
    {
        canMove = true;
    }
    #endregion

    //점프 가능 체크
    private void JumpCheck()
    {
        if (isJumping && MoverV.Velocity.y <= -0.01f)
        {
            //낙하 시작 시
            isJumping = false;
        }
        if (GroundCheck() && MoverV.Velocity.y <= 0.01f && !isJumping)
        {
            //땅에 있을 때
            canJumpCounter = stats.jumpCount;
        }
        else if (!GroundCheck() && canJumpCounter == stats.jumpCount)
        {
            //점프하지 않고 공중에 있을 때
            canJumpCounter = stats.jumpCount - 1;
        }
    }

    //땅 체크
    public bool GroundCheck()
    {
        if (groundCheckerCollider == null)
        {
            return false;
        }

        int layer = LayerMask.GetMask(groundLayer, halfGroundLayer);
        if (groundCheckerCollider.IsTouchingLayers(layer))//콜라이더가 해당 레이어에 닿아 있는가?
        {
            return true;
        }

        return false;
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

    ////점프
    //private void Jump()
    //{
    //    canJumpCounter--;
    //    MoverV.SetVelocityY(0, true);
    //    MoverV.AddForceY(JumpPower);
    //    isJumping = true;
    //}


    //재장전
    public void Reload()
    {
        NowBullet = maxBullet;
        UIManager.Instance.OnBulletNumUpdated?.Invoke();
        RuntimeManager.PlayOneShot("event:/Realod_End");
    }

    //재장전 시작
    private void Reloading()
    {
        reloading = true;
        UIManager.Instance.OnReload?.Invoke();
        reloadTimer.Reset();
        RuntimeManager.PlayOneShot("event:/Reload_start");
    }

    //재장전 남은 시간 검사
    private void ReloadCheck()
    {
        if (reloading && reloadTimer.Check(reloadTime, reloadSlowRate))
        {
            reloading = false;
            Reload();
            reloadTimer.Reset();
        }
    }

    //장전 취소
    private void CancelReload()
    {
        reloading = false;
        UIManager.Instance.OnCancelReload?.Invoke();
        reloadTimer.Reset();
    }

    //대시. 이후 제거할 것
    private IEnumerator DoDash()
    {
        canShootTemp = false;
        isDash = true;
        canMove = false;
        if (reloading)
        {
            CancelReload();
        }

        dashTimer.Reset();

        while (!dashTimer.Check(dashTime, dashSlowRate))
        {
            //float temp = TickTimer.GetConvertedTimeRate(slowRate_Dash);
            float speed = (Mathf.Max(0, Speed)) * dashSpeedRate * (isLookLeft ? -1 : 1);
            MoverV.SetVelocityX(speed);
            //Debug.Log(dashTimer.GetRemain(dashTime));
            yield return null;
        }

        //yield return new WaitForSeconds(dashTime);
        canMove = true;
        isDash = false;
        canShootTemp = true;
    }

    //반대 방향 회전. 그래픽은 마우스 방향 기준이므로 처리해주지 않음
    public override void Turn()
    {
        isLookLeft = !isLookLeft;
    }

    //죽을 때
    protected override void OnKilled()
    {
        base.OnKilled();
        GameManager.TestGlitch.currentGlitch = TestGlitch.GlitchType.Death;
        gameoverText.SetActive(true);
        //죽을 때 처리 추가
    }
    
    //피해 입을 때
    protected override void OnDamaged()
    {
        base.OnDamaged();
        StopCoroutine(DamageEffect());
        StartCoroutine(DamageEffect());
    }

    //피격 시 이펙트 표시
    private IEnumerator DamageEffect()
    {
        GameManager.TestGlitch.currentGlitch = TestGlitch.GlitchType.Hurt;
        yield return new WaitForSeconds(glitchTime);
        GameManager.TestGlitch.currentGlitch = TestGlitch.GlitchType.NONE;
    }
}
