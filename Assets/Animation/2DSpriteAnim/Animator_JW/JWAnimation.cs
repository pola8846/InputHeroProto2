using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JWAnimation에서 파생되어 다형적으로 동작할 애니메이션들
/// </summary>

// 일단은 플레이어, 몬스터, 등등의 모든 애니메이션이 한 이넘에 통합되어 있습니다....
public enum JWAnimType
{
    /// Player

    // UPPER
    PLAYER_UPPER_AIM,
    PLAYER_UPPER_RELOAD,
    PLAYER_UPPER_DASH,
    PLAYER_UPPER_DIE,

    // LOWER
    PLAYER_LOWER_WALK,
    PLAYER_LOWER_STAND,

    /// Enemy
    /// ...

    /// ALL
    NONE
}

[Serializable]
public class JWAnimation // 모든 애니메이션의 베이스 클래스. 빈 애니메이션만 이 수준을 사용
{
    [SerializeField]
    private JWAnimType type;
    public JWAnimType Type { get { return type; } }

    public JWAnimation(JWAnimType t)
    {
        type = t;
    }

    public virtual Sprite GetCurrentSprite()
    {
        return null;
    }

    public virtual void Enter() { }
    public virtual void Run() { }
    public virtual void Exit() { }
}

public class SpriteAnim : JWAnimation // 스프라이트가 있는 애니메이션들의 베이스 클래스: Aim 등의 특수 조건이 달린 애니메이션은 이 수준에서 파생시켜서 Run함수 내에서 currentIndex와 flip값만 조작해주면 됩니당
{
    [SerializeField]
    protected List<Sprite> sprites;
    protected int currentIndex;

    protected bool flip = false;
    public bool Flip { get { return flip; } }

    public SpriteAnim(JWAnimType t)
        : base(t) { }

    public override Sprite GetCurrentSprite()
    {
        return sprites[currentIndex];
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Run()
    {
        base.Run();
    }
    public override void Exit()
    {
        base.Exit();
    }
}

public class BasicSpriteAnim : SpriteAnim // 별다른 조건 없이 스프라이트만 단순하게 돌릴거면 여기서 파생시키기
{
    [SerializeField]
    private bool looping;

    [SerializeField]
    private float fps;

    private float timeSinceLastFrame;

    public BasicSpriteAnim(JWAnimType t)
    : base(t) { }

    public override void Enter()
    {
        currentIndex = 0;
        timeSinceLastFrame = 0.0F;

        base.Enter();
    }

    public override void Run()
    {
        // 루프 조건
        if (!looping & currentIndex + 1 >= sprites.Count)
        {
            return;
        }

        // 스프라이트 돌리기
        timeSinceLastFrame += Time.deltaTime;

        if (fps <= 0.0F) return;

        if (timeSinceLastFrame >= 1.0F / fps)
        {
            timeSinceLastFrame = 0.0F;
            currentIndex = (currentIndex + 1) % sprites.Count;
        }

        base.Run();
    }

    public override void Exit()
    {
        base.Exit();
    }
}