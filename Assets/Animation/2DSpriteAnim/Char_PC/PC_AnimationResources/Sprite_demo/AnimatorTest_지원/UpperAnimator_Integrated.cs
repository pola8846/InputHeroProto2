using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicAnimationInfo
{
    public List<Sprite> sprites;
    public Material material;
    public float speed_FPS;
    public bool looping;

    [HideInInspector]
    public int currentIndex;
}

// 지원: enum으로 다른 애니메이션도 추가할 수 있게 만든 통합 버전
public class UpperAnimator_Integrated : MonoBehaviour
{
    public enum AnimationType
    {
        AIM,
        SAMPLE
        // ...
    }

    /// 애니메이터 상태
    AnimationType currentAnimation;
    SpriteRenderer spriteRenderer;

    /// 모든 애니메이션 공통
    public Transform Center;
    float timeSinceLastFrame = 0.0F;

    /// 특수 애니메이션
    // AIM
    public List<Sprite> AimAnimationSprites;
    public GameObject LowerBody;
    int angleScale = 1;
    Vector2 mousePos0;

    /// 일반 애니메이션 - 스프라이트 돌리는 것
    // SAMPLE
    public BasicAnimationInfo SampleAnimation;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAnimation = AnimationType.AIM;
    }

    public void SetAnimation(AnimationType anim)
    {
        //if (anim == currentAnimation) return; // 이 조건은 고민중..

        /// EXIT: 이전 애니메이션에서 나오기 전 설정해줄 것
        switch (currentAnimation)
        {
            case AnimationType.AIM:
                LowerBody.SetActive(false);
                break;
            case AnimationType.SAMPLE:
                break;
            // ...
            default:
                Debug.LogError("지원: 지정된 상체 애니메이션 없음");
                break;
        }

        /// ENTER: 새 애니메이션으로 들어올 때 한 번 설정할 것
        switch (anim)
        {
            case AnimationType.AIM:
                LowerBody.SetActive(true);
                break;
            case AnimationType.SAMPLE:
                SampleAnimation.currentIndex = 0;
                spriteRenderer.sprite = SampleAnimation.sprites[SampleAnimation.currentIndex];
                break;
            // ...
            default:
                Debug.LogError("지원: 지정된 상체 애니메이션 없음");
                break;
        }

        timeSinceLastFrame = 0.0F;
        currentAnimation = anim;
    }

    void Update()
    {
        //// test
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    SetAnimation(AnimationType.SAMPLE);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    SetAnimation(AnimationType.AIM);
        //}

        // flip은 아마도 애니메이션 공통이므로..
        FlipUpdate();

        /// 각 애니메이션별 UPDATE문 실행
        switch (currentAnimation)
        {
            case AnimationType.AIM:
                AimUpdate();
                break;
            case AnimationType.SAMPLE:
                BasicAnimationUpdate(SampleAnimation);
                break;
            // ...
            default:
                Debug.LogError("지원: 지정된 상체 애니메이션 없음");
                break;
        }
    }

    void AimUpdate()
    {
        spriteRenderer.sprite = AimAnimationSprites[angleScale - 1];
        eulerAngleConverter();
    }

    // 스프라이트 쭉 돌려주는 일반 애니메이션은 이 업데이트 함수를 돌려쓰면됨
    void BasicAnimationUpdate(BasicAnimationInfo anim)
    {
        if (!anim.looping & anim.currentIndex + 1 >= anim.sprites.Count)
        {
            return;
        }

        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 1.0F / anim.speed_FPS)
        {
            timeSinceLastFrame = 0.0F;
            anim.currentIndex = (anim.currentIndex + 1) % anim.sprites.Count;
            spriteRenderer.sprite = anim.sprites[anim.currentIndex];
        }
    }

    void FlipUpdate()
    {
        mousePos0 = GameManager.MousePos;

        if (Center.transform.position.x >= mousePos0.x)
        {
            spriteRenderer.flipX = !enabled;
        }
        else
        {
            spriteRenderer.flipX = enabled;
        }
    }

    void eulerAngleConverter()
    {
        Vector2 nowdir = (mousePos0 - new Vector2(Center.transform.position.x, Center.transform.position.y)).normalized;
        float nowAngle = GameTools.GetDegreeAngleFormDirection(nowdir);
        float convertedAngle = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowAngle / 5.625f)), 0, 31);
        angleScale = (int)(convertedAngle + 1);
    }
}