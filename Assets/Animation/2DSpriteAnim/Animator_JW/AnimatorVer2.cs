using UnityEngine;

/// <summary>
/// 지원: 첫 번째와 마지막 스프라이트 인덱스만 입력하면 알아서 이미지를 뽑아주는 애니메이터
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

        // currentAnimationInst의 정보를 스프라이트에 반영
        spriteRenderer.sprite = sprites[currentAnimationInst.GetSpriteListIndex()];
        spriteRenderer.flipX = currentAnimationInst.flip;
    }
}