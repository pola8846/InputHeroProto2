using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DownAnimator_Integrated : MonoBehaviour
{
    public enum Animation_Down
    {
        WALK,
        STAND
        // ...
    }

    // 애니베이션별로 별도의 스프라이트 리스트 생성
    public List<Sprite> WalkAnimationSprites;
    public Sprite StandAnimationSprite;

    Animation_Down currentAnimation;
    SpriteRenderer spriteRenderer;

    // 전체 애니메이션 관련
    public Transform Center;

    // Walk 애니메이션 관련
    int walkAnimIndex = 0;
    float transform0;
    float maxXvalue = 5.0F;
    float xPosManipulator = 0.443F;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAnimation = Animation_Down.STAND;
        spriteRenderer.sprite = StandAnimationSprite;
    }

    public void SetAnimation(Animation_Down anim)
    {
        if (anim == currentAnimation) return;

        /// ENTER 새 애니메이션으로 들어올 때 한 번 설정할 것
        switch (anim)
        {
            case Animation_Down.WALK:
                walkAnimIndex = 0;
                break;
            case Animation_Down.STAND:
                spriteRenderer.sprite = StandAnimationSprite;
                break;
            // ...
            default:
                Debug.LogError("지원: 지정된 하체 애니메이션 없음");
                break;
        }

        currentAnimation = anim;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            SetAnimation(Animation_Down.STAND);
        }
        else
        {
            SetAnimation(Animation_Down.WALK);
        }

        FlipUpdate();

        /// UPDATE: 각 애니메이션별 UPDATE문 실행
        switch (currentAnimation)
        {
            case Animation_Down.WALK:
                WalkUpdate();
                break;
            case Animation_Down.STAND:
                break;
            // ...
            default:
                Debug.LogError("지원: 지정된 하체 애니메이션 없음");
                break;
        }
    }

    void WalkUpdate()
    {
        transform0 += Input.GetAxisRaw("Horizontal") * xPosManipulator;

        if (spriteRenderer.flipX)
        {
            if (transform0 > maxXvalue)
            {
                WalkSpriteChange();
            }
            else if (transform0 < -maxXvalue)
            {
                WalkSpriteChange_Resversed();
            }
        }
        else
        {
            if (transform0 > maxXvalue)
            {
                WalkSpriteChange_Resversed();
            }
            else if (transform0 < -maxXvalue)
            {
                WalkSpriteChange();
            }
        }
    }

    void WalkSpriteChange()
    {
        walkAnimIndex = (walkAnimIndex + 1) % WalkAnimationSprites.Count;
        spriteRenderer.sprite = WalkAnimationSprites[walkAnimIndex];
        transform0 = 0.0F;
    }

    void WalkSpriteChange_Resversed()
    {
        walkAnimIndex = (walkAnimIndex - 1 + WalkAnimationSprites.Count) % WalkAnimationSprites.Count;
        spriteRenderer.sprite = WalkAnimationSprites[walkAnimIndex];
        transform0 = 0.0F;
    }

    void FlipUpdate()
    {
        Vector2 mousePos0 = GameManager.MousePos;

        if (Center.position.x >= mousePos0.x)
        {
            spriteRenderer.flipX = !enabled;
        }
        else
        {
            spriteRenderer.flipX = enabled;
        }
    }
}
