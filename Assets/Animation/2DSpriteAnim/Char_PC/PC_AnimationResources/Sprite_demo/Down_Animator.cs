using UnityEngine;


public class Down_Animator : SpriteAnimation<PlayerUnit>
{
    //이동
    [Header("모션 속도")]
    [SerializeField, Range(0, 2)] float xPosManifulator;
    [SerializeField] float maxXValue;
    private float MoveCount;


    //점프
    [SerializeField] float jumpTime;
    private TickTimer jumpTimer;

    protected override void Start()
    {
        base.Start();
        jumpTimer = new();
    }

    protected override void Update()
    {

        if (sourceUnit.IsJumping)//점프면
        {
            jumpingSprite();
        }
        else if(sourceUnit.GroundCheck())//점프가 아니면(이동)
        {
            //MoveCount를 좌우 조작*배율만큼 변경
            MoveCount += sourceUnit.MoverV.Velocity.x * xPosManifulator * Time.deltaTime;

            //조작이 없으면 idle
            if (Mathf.Abs(sourceUnit.MoverV.Velocity.x) <= maxXValue)
            {
                if (nowSpriteList.keycode != "Idle")
                {
                    ChangeSpriteList("Idle");
                }
                MoveCount = 0;
            }//조작이 있으면
            else if (Mathf.Abs(MoveCount) > maxXValue)
            {
                if (nowSpriteList.keycode != "Move")
                {
                    ChangeSpriteList("Move");
                }

                if (MoveCount > 0)
                {
                    if (flip)
                        ChangeSpritePrevious();
                    else
                        ChangeSpriteNext();
                    MoveCount = 0;
                }
                else if (MoveCount < 0) 
                {
                    if (flip)
                        ChangeSpriteNext();
                    else
                        ChangeSpritePrevious();
                    MoveCount = 0;
                }
            }
        }

        base.Update();
    }

    public void FlipCheck()
    {
        flip = !sourceUnit.IsMouseLeft;
        spriteRenderer.flipX = !flip;
    }

    void jumpingSprite()
    {
        if (nowSpriteList.keycode != "Jump")
        {
            ChangeSpriteList("Jump");
            jumpTimer.Reset();
        }
        ChangeSprite(jumpTime - jumpTimer.GetRemain(jumpTime), 0, jumpTime);
    }
}
