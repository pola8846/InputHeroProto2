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
        // �� ������ ĳ�õ����Ϳ� ���ؼ� ���� �ִϸ��̼��� ����
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

    // �ִϸ��̼��� ������ ��������Ʈ �������� �ݿ��մϴ�
    void SpriteRendererUpdate()
    {
        if (spriteRenderer == null) return;

        // ��������Ʈ �ݿ�
        spriteRenderer.sprite = sprites[currentAnimationInst.GetSpriteListIndex()];
    }
}