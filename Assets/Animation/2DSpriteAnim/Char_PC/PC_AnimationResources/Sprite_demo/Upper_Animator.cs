using UnityEngine;

public class Upper_Animator : SpriteAnimation<PlayerUnit>
{
    public bool tickReload = false;

    [SerializeField] float nowDashTime;
    [SerializeField] float maxDashjTime;
    [SerializeField] float tMDash;

    [SerializeField] float nowReloadTime;
    [SerializeField] float maxReload;
    [SerializeField] float timeManifulatorReload;

    protected override void Update()
    {
        flip = !sourceUnit.IsMouseLeft;
        spriteRenderer.flipX = !flip;

        //if (sourceUnit.isDash)
        //{
        //    animation_Dash();
        //}
        //else
        if (!tickReload)
        {
            animation_Aim();
        }
        else if (tickReload)
        {
            reload();
        }


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            tickReload = true;
        }

        if (!sourceUnit.isDash)
        {
            nowDashTime = 0;
        }

        base.Update();
    }

    void animation_Aim()
    {
        if (nowSpriteList.name != "Find")
        {
            ChangeSpriteList("Find");
        }
        ChangeSprite(GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count, Mathf.Abs(sourceUnit.NowMouseAngle), 0, 180));
    }


    void reload()
    {
        nowReloadTime = nowReloadTime + Time.deltaTime * timeManifulatorReload;
        if (nowReloadTime > maxReload)
        {
            nowReloadTime = 0;
            tickReload = false;

        }

        if (nowSpriteList.name != "Reload")
        {
            ChangeSpriteList("Reload");
        }
        ChangeSprite(GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count, nowReloadTime, 0, maxReload));
    }

    void animation_Dash()
    {
        nowDashTime = nowDashTime + Time.deltaTime * tMDash;
        if (nowDashTime > maxDashjTime)
        {
            nowDashTime = 0;
        }


        if (nowSpriteList.name != "Dash")
        {
            ChangeSpriteList("Dash");
        }
        ChangeSprite(GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count, nowDashTime, 0, maxDashjTime));
    }
}