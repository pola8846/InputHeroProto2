using UnityEngine;

/// <summary>
/// ����: ù ��°�� ������ ��������Ʈ �ε����� �Է��ϸ� �˾Ƽ� �̹����� �̾��ִ� �ִϸ�����
/// </summary>

public class AnimatorVer2 : MonoBehaviour
{
    public SpritesLoader loader;
    protected SpriteRenderer spriteRenderer;

    protected AnimationVer2 currentAnimationInst;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        SpriteRendererUpdate();
    }

    void SpriteRendererUpdate()
    {
        if (currentAnimationInst == null) return;

        // currentAnimationInst�� ������ ��������Ʈ�� �ݿ�
        //spriteRenderer.sprite = loader.sprites[currentAnimationInst.GetSpriteListIndex()];
        spriteRenderer.flipX = currentAnimationInst.flip;
    }
}