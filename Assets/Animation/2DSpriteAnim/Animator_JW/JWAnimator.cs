using System;
using System.Collections.Generic;
using UnityEngine;

public enum JWAnimType
{
    /// Player

    // UPPER
    PLAYER_UPPER_AIM,
    PLAYER_UPPER_SAMPLE,

    // LOWER
    PLAYER_LOWER_WALK,
    PLAYER_LOWER_STAND,

    /// Enemy
    /// ...

    /// ALL
    NONE
}

[Serializable]
public class JWAnimation
{
    [SerializeField]
    private JWAnimType type;
    public JWAnimType Type { get { return type; } }

    [SerializeField]
    private List<Sprite> sprites;

    [SerializeField]
    private Material material;

    [SerializeField]
    private bool looping;

    [SerializeField]
    private float fps;

    [SerializeField]
    private int currentIndex;

    private float timeSinceLastFrame;

    public Sprite GetCurrentSprite()
    {
        return sprites[currentIndex];
    }

    public void Enter()
    {
        currentIndex = 0;
        timeSinceLastFrame = 0.0F;
    }

    public void Run()
    {
        if (!looping & currentIndex + 1 >= sprites.Count)
        {
            return;
        }

        timeSinceLastFrame += Time.deltaTime;

        if (fps <= 0.0F) return;

        if (timeSinceLastFrame >= 1.0F / fps)
        {
            timeSinceLastFrame = 0.0F;
            currentIndex = (currentIndex + 1) % sprites.Count;
        }
    }

    public void Exit()
    {

    }
}

public class JWAnimator : MonoBehaviour
{
    public List<JWAnimation> animations;

    private JWAnimType cache;
    public JWAnimType currentAnimation;

    private JWAnimation currentAnimationInst;

    // 리스트에서 가장 먼저 찾은 애니메이션 인스턴스를 반환
    private JWAnimation FindAnimation(List<JWAnimation> list, JWAnimType type)
    {
        foreach (JWAnimation anim in list)
        {
            if (anim.Type == type)
            {
                return anim;
            }
        }
        return null;
    }

    void Update()
    {
        if (cache != currentAnimation)
        {
            currentAnimationInst?.Exit();

            currentAnimationInst = FindAnimation(animations, currentAnimation);
            cache = currentAnimation;

            currentAnimationInst?.Enter();
        }

        currentAnimationInst?.Run();
        GetComponent<SpriteRenderer>().sprite = currentAnimationInst?.GetCurrentSprite();
    }
}
