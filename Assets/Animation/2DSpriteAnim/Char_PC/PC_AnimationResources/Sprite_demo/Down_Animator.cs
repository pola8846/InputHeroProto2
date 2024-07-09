using UnityEngine;


public class Down_Animator : SpriteAnimation<PlayerUnit>
{
    [Header("모션 속도")]
    [SerializeField, Range(0, 2)] float xPosManifulator;
    [SerializeField] float maxXValue;

    [SerializeField]
    private bool jump;
    private float horizontal;


    [SerializeField] float nowJumpTime;
    [SerializeField] float maxJump;
    [SerializeField] float timeManifulatorJump;

    protected override void Update()
    {
        flip = !sourceUnit.IsMouseLeft;
        spriteRenderer.flipX = !flip;

        if (Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            jump = true;
        }

        if (jump)
        {
            jumpingSprite();

            if (sourceUnit.CanJumpCounter == sourceUnit.Stats.jumpCount)
            {
                nowJumpTime = 0;
                jump = false;
            }
        }
        else
        {
            horizontal += Input.GetAxisRaw("Horizontal") * xPosManifulator * Time.deltaTime;


            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                if (nowSpriteList.name != "Stand")
                {
                    ChangeSpriteList("Stand");
                }
                horizontal = 0;
            }
            else if (Mathf.Abs(horizontal) > maxXValue)
            {
                if (nowSpriteList.name != "Find")
                {
                    ChangeSpriteList("Find");
                }

                if (horizontal > maxXValue)
                {
                    if (flip)
                        ChangeSpritePrevious();
                    else
                        ChangeSpriteNext();
                    horizontal = 0;
                }
                else if (horizontal < -maxXValue)
                {
                    if (flip)
                        ChangeSpriteNext();
                    else
                        ChangeSpritePrevious();
                    horizontal = 0;
                }
            }
        }

        skip = sourceUnit.isDash;
        base.Update();
    }

    void jumpingSprite()
    {
        nowJumpTime = nowJumpTime + Time.deltaTime * timeManifulatorJump;

        if (nowSpriteList.name != "Jump")
        {
            ChangeSpriteList("Jump");
        }
        ChangeSprite(GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count, nowJumpTime, 0, maxJump));
    }


}
