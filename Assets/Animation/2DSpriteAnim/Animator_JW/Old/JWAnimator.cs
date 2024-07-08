using System.Collections.Generic;
using UnityEngine;

public class JWAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // 애니메이션 목록
    [SerializeReference]
    public List<JWAnimation> animations = new List<JWAnimation>();
    private JWAnimation noneAnim = new JWAnimation(JWAnimType.NONE);

    // 매 프레임 캐시데이터와 비교해서 현재 애니메이션을 선택
    private JWAnimType cache;
    public JWAnimType currentAnimation;

    // 애니메이션 목록에서 꺼내온 현재 애니메이션 인스턴스
    private JWAnimation currentAnimationInst;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cache = JWAnimType.NONE;
    }

    // 리스트에서 가장 먼저 찾은 애니메이션 인스턴스를 반환 (같은 타입이 리스트에 여러개 있으면 뒤에있는것들은 무시된다는 소리)
    private JWAnimation FindAnimation(List<JWAnimation> list, JWAnimType type)
    {
        // 지정한 타입 찾기
        foreach (JWAnimation anim in list)
        {
            if (anim.Type == type)
            {
                return anim;
            }
        }
        // 못찾으면 NONE타입 반환
        return noneAnim;
    }

    void Update()
    {
        // 매 프레임 캐시데이터와 비교해서 현재 애니메이션을 선택
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

    // 애니메이션의 정보를 스프라이트 렌더러에 반영합니다
    void SpriteRendererUpdate()
    {
        if (spriteRenderer == null) return;

        // 스프라이트 반영
        spriteRenderer.sprite = currentAnimationInst?.GetCurrentSprite();

        // 플립 반영
        if (currentAnimationInst is SpriteAnim)
        {
            if ((currentAnimationInst as SpriteAnim).Flip) { spriteRenderer.flipX = true; }
            else { spriteRenderer.flipX = false; }
        }
    }

    [ContextMenu("일반 스프라이트 애니메이션 추가")]
    void AddBasicSpriteAnim() { animations.Add(new BasicSpriteAnim(JWAnimType.NONE)); }

    [ContextMenu("PLAYER_UPPER_AIM 추가")]
    void AddPlayerUpperAimAnim() { animations.Add(new Player_Upper_Aim(JWAnimType.PLAYER_UPPER_AIM)); }
}