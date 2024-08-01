using UnityEngine;

public class Upper_Animator : SpriteAnimation<PlayerUnit>
{
    protected override void Update()
    {
        //리로드 중이면 리로드, 아니면 조준
        if (!sourceUnit.IsReloading)
        {
            Animation_Aim();
        }
        else if (sourceUnit.IsReloading)
        {
            Reload();
        }
        base.Update();
    }

    public void FlipCheck()
    {
        flip = !sourceUnit.IsMouseLeft;
        spriteRenderer.flipX = !flip;
    }

    void Animation_Aim()
    {
        if (nowSpriteList.name != "Find")
        {
            ChangeSpriteList("Find");
        }
        ChangeSprite(Mathf.Abs(sourceUnit.NowMouseAngle), 0, 180);
    }


    void Reload()
    {
        if (nowSpriteList.name != "Reload")
        {
            ChangeSpriteList("Reload");
        }
        ChangeSprite(sourceUnit.RemainReloadTime, 0, sourceUnit.ReloadTime);
    }
}