using System.Collections.Generic;
using UnityEngine;

public class JWAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // �ִϸ��̼� ���
    [SerializeReference]
    public List<JWAnimation> animations = new List<JWAnimation>();
    private JWAnimation noneAnim = new JWAnimation(JWAnimType.NONE);

    // �� ������ ĳ�õ����Ϳ� ���ؼ� ���� �ִϸ��̼��� ����
    private JWAnimType cache;
    public JWAnimType currentAnimation;

    // �ִϸ��̼� ��Ͽ��� ������ ���� �ִϸ��̼� �ν��Ͻ�
    private JWAnimation currentAnimationInst;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cache = JWAnimType.NONE;
    }

    // ����Ʈ���� ���� ���� ã�� �ִϸ��̼� �ν��Ͻ��� ��ȯ (���� Ÿ���� ����Ʈ�� ������ ������ �ڿ��ִ°͵��� ���õȴٴ� �Ҹ�)
    private JWAnimation FindAnimation(List<JWAnimation> list, JWAnimType type)
    {
        // ������ Ÿ�� ã��
        foreach (JWAnimation anim in list)
        {
            if (anim.Type == type)
            {
                return anim;
            }
        }
        // ��ã���� NONEŸ�� ��ȯ
        return noneAnim;
    }

    void Update()
    {
        // �� ������ ĳ�õ����Ϳ� ���ؼ� ���� �ִϸ��̼��� ����
        if (cache != currentAnimation)
        {
            currentAnimationInst?.Exit();

            currentAnimationInst = FindAnimation(animations, currentAnimation);
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
        spriteRenderer.sprite = currentAnimationInst?.GetCurrentSprite();

        // �ø� �ݿ�
        if (currentAnimationInst is SpriteAnim)
        {
            if ((currentAnimationInst as SpriteAnim).Flip) { spriteRenderer.flipX = true; }
            else { spriteRenderer.flipX = false; }
        }
    }

    [ContextMenu("�Ϲ� ��������Ʈ �ִϸ��̼� �߰�")]
    void AddBasicSpriteAnim() { animations.Add(new BasicSpriteAnim(JWAnimType.NONE)); }

    [ContextMenu("PLAYER_UPPER_AIM �߰�")]
    void AddPlayerUpperAimAnim() { animations.Add(new Player_Upper_Aim(JWAnimType.PLAYER_UPPER_AIM)); }
}