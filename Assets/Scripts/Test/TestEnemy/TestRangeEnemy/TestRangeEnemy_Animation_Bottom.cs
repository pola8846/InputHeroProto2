using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeEnemy_Animation_Bottom : SpriteAnimation<TestRangeEnemy>
{
    private float speed => sourceUnit.MoverV.TargetSpeedX;


    [Header("모션 속도")]
    [SerializeField, Range(0, 2)] float xPosManifulator;
    [SerializeField] float maxXValue;

    private float horizontal;

    protected override void Update()
    {
        flip = sourceUnit.IsLookLeft;
        spriteRenderer.flipX = !flip;

        horizontal += speed * xPosManifulator * Time.deltaTime;


        if (speed == 0)
        {
            if (nowSpriteList.name != "Wait")
            {
                ChangeSpriteList("Wait");
            }
            horizontal = 0;
        }
        else if (Mathf.Abs(horizontal) > maxXValue)
        {
            if (nowSpriteList.name != "Walk")
            {
                ChangeSpriteList("Walk");
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

        base.Update();
    }

    public void SetTargetSprite(string str)
    {
        targetSpriteName = str;
    }
}