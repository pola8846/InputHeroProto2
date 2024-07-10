using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLowerAnimator : AnimatorVer2
{
    //-------------------------------(상속해서 애니메이터 생성시 바꿔야할 정보)-------------------------------//
    public enum AnimType
    {
        WALK,
        JUMP,
        // ...
    }

    [ContextMenu("일반 스프라이트 애니메이션 추가")]
    void AddBasicSpriteAnim() { animations.Add(new SerializablePair(AnimType.WALK, new BasicSpriteAnimVer2())); }
    // ...
    //------------------------------------(아래부터는 그냥 복붙해도됨,,)------------------------------------//

    [Serializable]
    public class SerializablePair
    {
        public SerializablePair(AnimType t, AnimationVer2 a)
        {
            type = t;
            anim = a;
        }

        public AnimType type;

        [SerializeReference]
        public AnimationVer2 anim;
    }

    private AnimType cacheIndex;
    public AnimType currentAnimation;

    public List<SerializablePair> animations = new List<SerializablePair>();

    protected override void Start()
    {
        base.Start();

        currentAnimationInst = GetAnimationInst(currentAnimation);
        currentAnimationInst?.Enter();
    }

    protected override void Update()
    {
        // 매 프레임 캐시데이터와 비교해서 현재 애니메이션을 선택
        if (cacheIndex != currentAnimation)
        {
            currentAnimationInst?.Exit();

            currentAnimationInst = GetAnimationInst(currentAnimation);
            cacheIndex = currentAnimation;

            currentAnimationInst?.Enter();
        }

        currentAnimationInst?.Run();

        base.Update();
    }

    AnimationVer2 GetAnimationInst(AnimType type)
    {
        // 타입이 일치하는 애니메이션중 인덱스가 낮은것을 리턴(= 타입 중복이 있으면 뒤에있는것은 무시됨)
        foreach (SerializablePair pair in animations)
        {
            if (pair.type == type) return pair.anim;
        }
        return null;
    }
}