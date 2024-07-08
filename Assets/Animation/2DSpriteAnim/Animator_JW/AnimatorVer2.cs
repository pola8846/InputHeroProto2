using UnityEngine;

/// <summary>
/// ����: ù ��°�� ������ ��������Ʈ �ε����� �Է��ϸ� �˾Ƽ� �̹����� �̾��ִ� �ִϸ�����
/// </summary>

public class AnimatorVer2 : MonoBehaviour
{
    public string spriteFileName;

    protected SpriteRenderer spriteRenderer;
    protected Sprite[] sprites;

    protected AnimationVer2 currentAnimationInst;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(spriteFileName);
    }

    protected virtual void Update()
    {
        SpriteRendererUpdate();
    }

    void SpriteRendererUpdate()
    {
        if (currentAnimationInst == null) return;

        // currentAnimationInst�� ������ ��������Ʈ�� �ݿ�
        spriteRenderer.sprite = sprites[currentAnimationInst.GetSpriteListIndex()];
        spriteRenderer.flipX = currentAnimationInst.flip;
    }
}