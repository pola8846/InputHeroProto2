using UnityEngine;

public class TestRangeEnemy_Animation_Top : SpriteAnimation<TestRangeEnemy>
{
    [SerializeField]
    private float animationFrameTime;
    private TickTimer timer;
     

    protected override void Start()
    {
        base.Start();
        timer = new TickTimer();
    }

    protected override void Update()
    {
        flip = sourceUnit.IsLookLeft;
        spriteRenderer.flipX = !flip;

        switch (targetSpriteName)
        {
            case "Wait":
                animation_Wait();
                break;
            case "Run":
                animation_Run();
                break;
            case "Move":
                animation_Move();
                break;
            case "Attack":
                animation_Aim();
                break;
            default:
                break;
        }

        base.Update();
    }


    void animation_Wait()
    {
        if (nowSpriteList.name != "Wait")
        {
            ChangeSpriteList("Wait");
            sourceUnit._Bottom.skip = true;
        }
        if (timer.Check(animationFrameTime))
        {
            timer.Reset();
            ChangeSpriteNext();
        }
    }
    void animation_Run()
    {
        if (nowSpriteList.name != "Run")
        {
            ChangeSpriteList("Run");
            sourceUnit._Bottom.skip = false;
        }
        if (timer.Check(animationFrameTime))
        {
            timer.Reset();
            ChangeSpriteNext();
        }
    }
    void animation_Move()
    {
        if (nowSpriteList.name != "Move")
        {
            ChangeSpriteList("Move");
            sourceUnit._Bottom.skip = false;
        }
        //if (timer.Check(animationFrameTime))
        //{
        //    timer.Reset();
        //    ChangeSpriteNext();
        //}

        if (timer.Check(animationFrameTime))
        {
            timer.Reset();
            ChangeSprite(GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count, Mathf.Abs(sourceUnit.shooter.bulletAngleMax + sourceUnit.shooter.bulletAngleMin / 2), 0, 180));
        }
    }
    void animation_Aim()
    {
        if (nowSpriteList.name != "Attack")
        {
            ChangeSpriteList("Attack");
            sourceUnit._Bottom.skip = false;
        }

        if (timer.Check(animationFrameTime))
        {
            timer.Reset();
            ChangeSprite(GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count, Mathf.Abs(sourceUnit.shooter.bulletAngleMax + sourceUnit.shooter.bulletAngleMin / 2), 0, 180));
        }
    }


    public void SetTargetSprite(string str)
    {
        targetSpriteName = str;
    }
}