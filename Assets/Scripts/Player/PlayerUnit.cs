using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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
    [SerializeField]
    private float attackRange = 50f;//��Ÿ�
    [SerializeField]
    private float autoAim_mouse1 = 0.25f;//�ڵ�����
    [SerializeField]
    private float autoAim_cornAngle1 = 1f;
    [SerializeField]
    private float autoAim_sqrLength1 = 10f;
    [SerializeField]
    private float autoAim_sqrWidth1 = 1f;
    [SerializeField]
    private LayerMask blockLayer;//���信���� ���θ��� ���̾�
    [SerializeField]
    private GameObject trail;
    [SerializeField]
    private GameObject particle;

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

    [SerializeField]
    private float reloadTime = 3f;

    public bool isDash;

    private Vector2 ShootStartPos
    {
        get
        {
            return (Vector2)transform.position + Vector2.up * .5f;//�Ѿ� ������
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

        //animator.SetFloat("MoveSpeedRate", Mathf.Abs(MoverV.Velocity.x) / stats.moveSpeed);
        PerformanceManager.StopTimer("PlayerUnit.Update");
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

        //����
        //if (canMove && !animator.GetBool("IsJumping"))
        //{
        //    if (inputType == InputType.MeleeAttack)
        //    {
        //        animator.Play("mixamo_com");
        //        canMove = false;
        //    }
        //}
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

        //���ο�
        if (inputType == InputType.Slow && TimeManager.IsSlowed == false)
        {
            StopCoroutine(Reloading());
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

    #region ���Ÿ� �⺻����
    //���콺 ���� ǥ�ñ� ȸ��(�ӽ�)
    private void RotateTargetter()
    {
        PerformanceManager.StartTimer("PlayerUnit.RotateTargetter");
        if (targetterGO is null)
        {
            PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
            return;
        }

        //�Ÿ� ���ϰ� ����
        Vector2 dir = (GameManager.MousePos - (Vector2)(transform.position + (Vector3)Vector2.up * .5f)).normalized * 2;
        targetterGO.transform.position = (Vector3)dir + transform.position + (Vector3)Vector2.up * .5f;
        PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
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

        target = GameTools.FindClosest(GameManager.MousePos, ProjectileManager.FindByFunc((Prj)=>
        {
            Vector2 pointB = (Vector2)transform.position + ((GameManager.MousePos - (Vector2)transform.position).normalized * autoAim_sqrLength1);
            return GameTools.IsPointInRhombus(Prj.transform.position, transform.position, pointB, autoAim_sqrWidth1);
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
            StartCoroutine(DrawHitGrapic(ShootStartPos, target.transform.position));
            return true;
        }
        return false;
    }

    //���콺 �������� �߻�(�ӽ�)
    private void ShootToMouse()
    {

        if (NowBullet <= 0)
        {
            return;
        }
        else
        {
            NowBullet--;
        }

        //�� ������ ���ؿ´�
        Vector2 dir = (Vector2)targetterGO.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up) * (IsLookLeft ? -1 : 1);


        //������ ���
        shooter.BulletAngle = angle;
        shooter.Triger();
    }

    private void ShootByRay()
    {
        //���̸� ���

        //�� ������ ���ؿ´�
        Vector2 dir = ((Vector2)targetterGO.transform.position - ShootStartPos).normalized;//�Ѿ� ����

        //������ ���
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | (1 << LayerMask.NameToLayer("Bullet")) | (1 << LayerMask.NameToLayer("Ground"));
        var ray = Physics2D.RaycastAll(ShootStartPos, dir, attackRange, layer);
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
            StartCoroutine(DrawHitGrapic(ShootStartPos, ShootStartPos + (dir * attackRange)));
            //DrawHitGrapic(ShootStartPos, ShootStartPos + (dir * attackRange));
            return;
        }

        //������ ��󿡰� �浹
        //���� ����
        var pgo = HitCheck(target, hitPos);
        //�׷��� ǥ��
        StartCoroutine(DrawHitGrapic(ShootStartPos, hitPos));
        DrawParticle(pgo, transform.position, hitPos, Vector2.Reflect(dir, normal));
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
        Debug.Log(target.gameObject.name);

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
        Debug.Log(particle);
        if (particle != null)
        {
            GameObject pgo = Instantiate(particle);
            pgo.transform.position = target + ((Vector2)transform.position - target).normalized * 0.2f;
            pgo.transform.LookAt(pgo.transform.position + (Vector3)dir);
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
    }

    private IEnumerator Reloading()
    {
        canShoot = false;
        Debug.Log("������");
        yield return new WaitForSeconds(reloadTime);
        Reload();
        canShoot = true;
    }

    

    //���
    private IEnumerator DoDash()
    {
        isDash = true;
        canMove = false;
        float speed = Mathf.Max(0, Speed) * dashSpeedRate * (isLookLeft ? -1 : 1);
        MoverV.SetVelocityX(speed);

        yield return new WaitForSeconds(dashTime);
        canMove = true;
        isDash = false;
    }


    //���Ÿ� ��Ÿ
    private IEnumerator DoShoot()
    {
        canShoot = false;
        Shoot();
        //ShootToMouse();
        yield return new WaitForSecondsRealtime(shootCooltime);
        canShoot = true;
    }

    public override void Turn()
    {
        isLookLeft = !isLookLeft;
    }
}
