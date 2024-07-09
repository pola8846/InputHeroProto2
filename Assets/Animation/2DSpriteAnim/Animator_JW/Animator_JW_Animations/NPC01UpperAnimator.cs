using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC01UpperAnimator : AnimatorVer2
{
    //-------------------------------(����ؼ� �ִϸ����� ������ �ٲ���� ����)-------------------------------//
    public enum AnimType
    {
        DIE1,
        DIE2,
        PREPARE, // �����غ�
        IDLE,
        PATROL,
        RUN,
        AIM
        // ...
    }

    [ContextMenu("�Ϲ� ��������Ʈ �ִϸ��̼� �߰�")]
    void AddBasicSpriteAnim() { animations.Add(new SerializablePair(AnimType.DIE1, new BasicSpriteAnimVer2())); }
    [ContextMenu("AIM �ִϸ��̼� �߰�")]
    void AddAimAnim() { animations.Add(new SerializablePair(AnimType.AIM, new PlayerUpperAim())); }
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