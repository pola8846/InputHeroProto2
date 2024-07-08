using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorVer2 : MonoBehaviour
{
    public enum AnimType
    {
        AIM,
        RELOAD,
        DASH,
        DIE
    }

    public string spriteFileName;

    SpriteRenderer spriteRenderer;
    Sprite[] sprites;

    public AnimType currentAnimation;
    private AnimType cache;

    public Dictionary<AnimType, AnimationVer2> animations;

    AnimationVer2 currentAnimationInst;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(spriteFileName);

        currentAnimationInst = animations[currentAnimation];
    }

    void Update()
    {
        // 매 프레임 캐시데이터와 비교해서 현재 애니메이션을 선택
        if (cache != currentAnimation)
        {
            currentAnimationInst?.Exit();

            //currentAnimationInst = FindAnimation(animations, currentAnimation);
            cache = currentAnimation;

            currentAnimationInst?.Enter();
        }

        currentAnimationInst?.Run();

        SpriteRendererUpdate();
    }

    // 애니메이션의 정보를 스프라이트 렌더러에 반영합니다
    void SpriteRendererUpdate()
    {
        if (spriteRenderer == null) return;

        // 스프라이트 반영
        spriteRenderer.sprite = sprites[currentAnimationInst.GetSpriteListIndex()];
    }
}