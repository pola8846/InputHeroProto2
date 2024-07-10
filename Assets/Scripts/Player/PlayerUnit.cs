using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class PlayerUnit : Unit, IGroundChecker, IMoveReceiver
{
    //[Header("����Ű")]
    protected Dictionary<InputType, bool> keyStay = new();

    [Header("��Ÿ")]
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

    [Header("����")]
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


    [Header("���Ÿ� ����")]
    [SerializeField]
    private BulletShooter shooter;
    [SerializeField]
    private BulletShooter shooter_Big;
    public BulletShooter Shooter_Big => shooter_Big;
    [SerializeField]
    private float shootCooltime;
    private bool canShoot = true;
    private bool canShootTemp = true;
    private bool CanShoot//������ �� �ִ°�?
    {
        get
        {
            return canShootTemp && NowBullet >= 1 && shootTimer != null && shootTimer.Check(shootCooltime);
        }
    }
    [SerializeField]
    private float attackMinRange = 0.25f;//�ּһ�Ÿ�(�Ѿ� ǥ�� ������)
    [SerializeField]
    private float attackRange = 50f;//��Ÿ�

    //���콺 ����
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
                //���� ���
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
    /// ���콺�� �ڱ� ���� ���ʿ� �ִ°�?
    /// </summary>
    public bool IsMouseLeft
    {
        get
        {
            return ShootStartPos.x < GameManager.MousePos.x;
        }
    }

    //�ڵ�����
    [SerializeField]
    private float autoAim_mouse1 = 0.25f;
    [SerializeField]
    private float autoAim_cornAngle1 = 1f;
    [SerializeField]
    private float autoAim_sqrLength1 = 10f;
    [SerializeField]
    private float autoAim_sqrWidth1 = 1f;
    [SerializeField]
    private LayerMask blockLayer;//���信���� ���θ��� ���̾�
    [SerializeField]
    private GameObject trail;//�Ѿ� Ʈ����
    [SerializeField]
    private GameObject particle;//�浹 �� ��ƼŬ

    //Ÿ�� ǥ�ÿ� ��ü. �׽�Ʈ��
    [SerializeField]
    private GameObject targetter;
    private GameObject targetterGO;

    //��ź
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

    //������ �ð�
    [SerializeField]
    private float reloadTime = 3f;
    public float ReloadTime => reloadTime;
    private TickTimer reloadTimer;
    private TickTimer shootTimer;
    private bool reloading = false;
    public bool IsReloading => reloading;

    public bool isDash;

    public Vector2 ShootStartPos
    {
        get
        {
            //return (Vector2)transform.position + Vector2.up * .5f;//�Ѿ� ������
            return OriginPos;
        }
    }


    //��ų
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
        //�Է� �˻�
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

        //���
        if (inputType == InputType.Dash)
        {
            StartCoroutine(DoDash());
            return;
        }

        //����
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

        //�ް���
        if (GroundCheck() == false && IsKeyPushing(InputType.MoveDown))
        {
            //�ް���
            MoverV.SetVelocityY(-MoverV.MaxSpeedY, true);
        }

        if (inputType == InputType.Shoot)//����
        {
            if (CanShoot)
            {
                InputShoot();
            }
            else if(NowBullet<=0)
            {
                UIManager.Instance.OnBulletUseFailed?.Invoke();
            }
        }

        //������
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

        //���ο�
        if (inputType == InputType.Slow && TimeManager.IsSlowed == false)
        {
            //Reloading();
            //StopCoroutine(ReloadingTemp());
            TimeManager.StartSlow();
        }
    }


    public void KeyUp(InputType inputType)
    {
        //�Է� �˻�
        if (keyStay.ContainsKey(inputType))
        {
            keyStay[inputType] = false;
        }
        else
        {
            keyStay.Add(inputType, false);
        }

        //����
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
        if (IsKeyPushing(InputType.Shoot) && CanShoot && !TimeManager.IsSlowed)//�Ѿ� �߻�
        {
            InputShoot();
        }
    }

    #region ���Ÿ� �⺻����
    //���콺 ���� ǥ�ñ� ȸ��(�ӽ�)
    private void RotateTargetter()
    {
        if (targetterGO is null)
        {
            return;
        }

        //�Ÿ� ���ϰ� ����
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
            UIManager.Instance.OnBulletNumUpdated?.Invoke();
        }
        RuntimeManager.PlayOneShot("event:/Bullet");
        RuntimeManager.PlayOneShot("event:/Bulletdrop");

        Projectile target = null;

        //���콺 �ֺ� ���� ���� �׷� �� �� �ִ� ź�� ������ ����� �� ��´�
        target = GameTools.FindClosest(GameManager.MousePos, ProjectileManager.FindByDistance(GameManager.MousePos, autoAim_mouse1));
        //�� �� ������ ���
        if (ShootToTarget(target))
        {
            return;
        }

        /*
        //�÷��̾�κ��� ���콺 �������� ���� ��ä���� �׷� ����� �� ��´�
        Vector2 dir = (Vector2)targetterGO.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up);//���콺 ����

        target = GameTools.FindClosest(transform.position, ProjectileManager.FindInCorn(transform.position, angle, autoAim_cornAngle1, attackRange));
        //�� �� ������ ���
        if(ShootToTarget(target))
        {
            return;
        }
        */

        //ã�� ź �߿��� ���콺�� ���� ����� ���� ã�´�
        target = GameTools.FindClosest(GameManager.MousePos, ProjectileManager.FindByFunc((Prj) =>
        {
            //�簢�� ���� �ִ� ź�� ã�´�
            Vector2 pointB = ShootStartPos + (NowMouseDir * autoAim_sqrLength1);
            return GameTools.IsPointInRhombus(Prj.transform.position, ShootStartPos, pointB, autoAim_sqrWidth1);
        }));
        if (ShootToTarget(target))
        {
            return;
        }

        //�߰��� ���信�� �Ǵ�

        //���信�� �� �ɸ��� �������� ���� ��� ���� ��ǥ�� ��´�
        ShootByRay();
    }

    //��󿡰� ź �浹 �õ�, ����� �� �� ������ true
    private bool ShootToTarget(Projectile target)
    {
        if (target != null && BulletPathCheck(target.transform.position))
        {
            //�ش� ź�� ��� �����Ѵ�
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
        //������ ���
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | (1 << LayerMask.NameToLayer("Bullet")) | (1 << LayerMask.NameToLayer("Ground"));
        var ray = Physics2D.RaycastAll(ShootStartPos, NowMouseDir, attackRange, layer);
        Collider2D target = null;//�浹�� �����ϰ� ���� ����� ���
        Vector2 hitPos = Vector2.zero;//�浹�� ����
        Vector2 normal = Vector2.zero;//����

        //��ȿ�ϸ� ���� ����� ���� ã�´�
        float minDistance = float.MaxValue;
        foreach (var hit in ray)
        {
            //�������� ���� ��� �ѱ��
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

            //���� ����� �� ã��
            float distance = Vector2.Distance(ShootStartPos, hit.point);
            if (distance < minDistance)
            {
                target = hit.collider;
                hitPos = hit.point;
                normal = hit.normal;
                minDistance = distance;
            }
        }

        //���θ����°�?
        if (target == null)
        {
            //�ִ� ��Ÿ����� �߻�
            //�׷��� ǥ��
            DrawHitGrapicAtMinRange(ShootStartPos + (NowMouseDir * attackRange));
            //DrawHitGrapic(ShootStartPos, ShootStartPos + (dir * attackRange));
            return;
        }

        //������ ��󿡰� �浹
        //���� ����
        var pgo = HitCheck(target, hitPos);
        //�׷��� ǥ��
        DrawHitGrapicAtMinRange(hitPos);
        DrawParticle(pgo, ShootStartPos, hitPos, Vector2.Reflect(NowMouseDir, normal));
        //DrawHitGrapic(ShootStartPos, hitPos);
    }

    /// <summary>
    /// �ش� ��ġ���� �Ѿ��� �� �� �ִ���(�߰��� ��ֹ��� ������)
    /// </summary>
    /// <param name="targetPos">��ǥ ��ġ</param>
    /// <returns>�� �� �ִ°�?</returns>
    private bool BulletPathCheck(Vector2 targetPos)
    {
        //���̸� ���
        var ray = Physics2D.LinecastAll(ShootStartPos, targetPos, blockLayer);
        //�÷��̾ �ƴ� �Ϳ� ������ ������
        foreach (var hit in ray)
        {
            if (!hit.transform.CompareTag("Player"))
                return false;
        }

        //�� ������ �� ������
        return true;
    }

    /// <summary>
    /// �ǰ� ó��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="hitPos"></param>
    /// <returns>�߻��� ����Ʈ ���ӿ�����Ʈ</returns>
    private GameObject HitCheck(Collider2D target, Vector2 hitPos)
    {
        if (target == null)
        {
            return null;
        }

        // �浹 ó��

        //�� ��Ʈ�ڽ���
        HitBox hitBox = target.GetComponent<HitBox>();
        if (hitBox != null && target.CompareTag("Enemy"))
        {
            hitBox.Damage(1);
            //Hit(hitBox.Unit);
            return null;
        }

        //�Ѿ��̸�
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

        //��Ÿ ��ֹ��̸�
        if (true)
        {

        }

        // �浹�� ���� ó�� �߰�
        return particle;
    }

    private void DrawHitGrapicAtMinRange(Vector2 target)
    {
        var dist = target - ShootStartPos;
        dist.Normalize();

        StartCoroutine(DrawHitGrapic(ShootStartPos + dist * attackMinRange, target));
    }

    //�Ѿ� �߻� �׷��� �׸���. ǥ�õǱ� �ϴµ� �� �̻�
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

    //���� ���� üũ
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

    //�� üũ
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

    //Ű�� ������ �ִ���
    private bool IsKeyPushing(InputType inputType)
    {
        return keyStay.ContainsKey(inputType) && keyStay[inputType];
    }


    //�Ʒ� ����
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

    //����
    private void Jump()
    {
        canJumpCounter--;
        MoverV.SetVelocityY(0, true);
        MoverV.AddForceY(JumpPower);
        isJumping = true;
        //animator.SetBool("IsJumping", true);
    }


    //������
    public void Reload()
    {
        NowBullet = maxBullet;
        UIManager.Instance.OnBulletNumUpdated?.Invoke();
        RuntimeManager.PlayOneShot("event:/Realod_End");
    }

    private void Reloading()
    {
        canShootTemp = false;
        reloading = true;
        reloadTimer.Reset();
        UIManager.Instance.OnReload?.Invoke();
        RuntimeManager.PlayOneShot("event:/Reload_start");

        //Debug.Log("������");
    }

    private void ReloadCheck()
    {
        if (reloading && reloadTimer.Check(reloadTime))
        {
            reloading = false;
            Reload();
            reloadTimer.Reset();
            canShootTemp = true;
            //Debug.Log("�������Ϸ�");
        }
    }

    private void CancelReload()
    {
        //Reloading();

        //StopCoroutine(ReloadingTemp());
        Debug.Log("dd");
        UIManager.Instance.OnCancelReload?.Invoke();
        reloading = false;
        reloadTimer.Reset();
        canShootTemp = true;
    }

    //���
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

    protected override void OnKilled()
    {
        base.OnKilled();

        //���� �� ó�� �߰�
    }
}
