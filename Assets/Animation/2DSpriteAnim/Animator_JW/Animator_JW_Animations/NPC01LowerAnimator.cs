using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC01LowerAnimator : AnimatorVer2
{
    //-------------------------------(����ؼ� �ִϸ����� ������ �ٲ���� ����)-------------------------------//
    public enum AnimType
    {
        WALK,
        // ...
    }

    [ContextMenu("�Ϲ� ��������Ʈ �ִϸ��̼� �߰�")]
    void AddBasicSpriteAnim() { animations.Add(new SerializablePair(AnimType.WALK, new BasicSpriteAnimVer2())); }
    // ...
    //------------------------------------(�Ʒ����ʹ� �׳� �����ص���,,)------------------------------------//

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
        // �� ������ ĳ�õ����Ϳ� ���ؼ� ���� �ִϸ��̼��� ����
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
        // Ÿ���� ��ġ�ϴ� �ִϸ��̼��� �ε����� �������� ����(= Ÿ�� �ߺ��� ������ �ڿ��ִ°��� ���õ�)
        foreach (SerializablePair pair in animations)
        {
            if (pair.type == type) return pair.anim;
        }
        return null;
    }
}