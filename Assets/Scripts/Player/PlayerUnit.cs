using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUnit : Unit
{
    [SerializeField]
    private KeyCode MoveL = KeyCode.A;
    [SerializeField]
    private KeyCode MoveR = KeyCode.D;
    [SerializeField]
    private KeyCode Jump = KeyCode.Space;
    [SerializeField]
    private KeyCode Attack = KeyCode.Z;
    [SerializeField]
    private KeyCode Attack2 = KeyCode.X;
    [SerializeField]
    private Animator animator;
    private int canJumpCounter;
    private bool isJumping = false;
    private bool canMove = true;
    
    private Rigidbody2D rb;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        GameManager.SetPlayer(this);
    }

    protected override void FixedUpdate()
    {
        rb.velocity = new Vector2(movementX, rb.velocity.y);
    }

    protected override void Update()
    {
        JumpCheck();
        if (keyStay.ContainsKey(MoveL) && keyStay[MoveL] && canMove)
        {
            movementX = Mathf.Max(0, Speed) * -1;
            if (!isLookLeft)
            {
                Turn();
            }
        }
        else if (keyStay.ContainsKey(MoveR) && keyStay[MoveR] && canMove)
        {
            movementX = Mathf.Max(0, Speed);
            if (isLookLeft)
            {
                Turn();
            }
        }
        else
        {
            movementX = 0;
        }

        animator.SetFloat("MoveSpeedRate", Mathf.Abs(rb.velocity.x) / stats.moveSpeed);
    }


    public override void KeyDown(KeyCode keyCode)
    {
        base.KeyDown(keyCode);
        if (keyCode==Jump && canJumpCounter>0)
        {
            canJumpCounter--;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * JumpPower);
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }
        if (canMove && !animator.GetBool("IsJumping"))
        {
            if (keyCode == Attack)
            {
                animator.Play("mixamo_com");
                canMove = false;
            }
            if (keyCode == Attack2)
            {
                animator.Play("mixamo_com 0");
                canMove = false;
            }
        }
    }
    
    public void AttackEnd()
    {
        canMove = true;
    }

    private void JumpCheck()
    {
        if (isJumping && rb.velocity.y <= -0.01f)
        {
            isJumping = false;
        }
        if (GroundCheck() && rb.velocity.y <= 0.01f && !isJumping)
        {
            canJumpCounter = stats.jumpCount;
            animator.SetBool("IsJumping", false);   
        }
        else if (!GroundCheck() && canJumpCounter == stats.jumpCount)
        {
            canJumpCounter = stats.jumpCount - 1;
        }
    }
}
