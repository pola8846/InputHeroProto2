using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지원: enum으로 다른 애니메이션도 추가할 수 있게 만든 통합 버전
public class UpperAnimator_Integrated : MonoBehaviour
{
    public enum Animation
    {
        AIM,
        SAMPLE
        // ...
    }

    // 애니베이션별로 별도의 스프라이트 리스트 생성
    public List<Sprite> AimAnimationSprites;
    public List<Sprite> SampleAnimationSprites;
    // ...

    Animation currentAnimation;
    SpriteRenderer spriteRenderer;

    // 전체 애니메이션 관련
    public Transform Center;
    float timeSinceLastFrame = 0.0F;

    // Aim 애니메이션 관련
    public GameObject LowerBody;
    int angleScale = 1;
    Vector2 mousePos0;

    // Sample 애니메이션 관련
    int sampleCurrentIndex = 0;
    public float sampleAnimationSpeed; //초당 프레임수(FPS)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAnimation = Animation.AIM;
    }

    public void SetAnimation(Animation anim)
    {
        if (anim == currentAnimation) return;

        /// EXIT: 이전 애니메이션에서 나오기 전 설정해줄 것
        switch (currentAnimation)
        {
            case Animation.AIM:
                LowerBody.SetActive(false);
                break;
            case Animation.SAMPLE:
                break;
            // ...
            default:
                Debug.LogError("지원: 지정된 상체 애니메이션 없음");
                break;
        }

        /// ENTER: 새 애니메이션으로 들어올 때 한 번 설정할 것
        switch (anim)
        {
            case Animation.AIM:
                LowerBody.SetActive(true);
                break;
            case Animation.SAMPLE:
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
        // test
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetAnimation(Animation.SAMPLE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetAnimation(Animation.AIM);
        }

        FlipUpdate();

        /// 각 애니메이션별 UPDATE문 실행
        switch (currentAnimation)
        {
            case Animation.AIM:
                AimUpdate();
                break;
            case Animation.SAMPLE:
                SampleUpdate();
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

    void SampleUpdate()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 1.0F / sampleAnimationSpeed)
        {
            timeSinceLastFrame = 0.0F;
            sampleCurrentIndex = (sampleCurrentIndex + 1) % SampleAnimationSprites.Count;
            spriteRenderer.sprite = SampleAnimationSprites[sampleCurrentIndex];
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