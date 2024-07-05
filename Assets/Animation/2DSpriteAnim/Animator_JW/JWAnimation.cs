using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JWAnimation���� �Ļ��Ǿ� ���������� ������ �ִϸ��̼ǵ�
/// </summary>

// �ϴ��� �÷��̾�, ����, ����� ��� �ִϸ��̼��� �� �̳ѿ� ���յǾ� �ֽ��ϴ�....
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
    PLYER_LOWER_JUMP,

    /// Enemy
    /// ...

    /// ALL
    NONE
}

[Serializable]
public class JWAnimation // ��� �ִϸ��̼��� ���̽� Ŭ����. �� �ִϸ��̼Ǹ� �� ������ ���
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

public class SpriteAnim : JWAnimation // ��������Ʈ�� �ִ� �ִϸ��̼ǵ��� ���̽� Ŭ����: Aim ���� Ư�� ������ �޸� �ִϸ��̼��� �� ���ؿ��� �Ļ����Ѽ� Run�Լ� ������ currentIndex�� flip���� �������ָ� �˴ϴ�
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

public class BasicSpriteAnim : SpriteAnim // ���ٸ� ���� ���� ��������Ʈ�� �ܼ��ϰ� �����Ÿ� ���⼭ �Ļ���Ű��
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
        // ���� ����
        if (!looping & currentIndex + 1 >= sprites.Count)
        {
            return;
        }

        // ��������Ʈ ������
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