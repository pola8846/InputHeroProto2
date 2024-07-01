using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����: enum���� �ٸ� �ִϸ��̼ǵ� �߰��� �� �ְ� ���� ���� ����
public class UpperAnimator_Integrated : MonoBehaviour
{
    public enum Animation
    {
        AIM,
        SAMPLE
        // ...
    }

    // �ִϺ��̼Ǻ��� ������ ��������Ʈ ����Ʈ ����
    public List<Sprite> AimAnimationSprites;
    public List<Sprite> SampleAnimationSprites;
    // ...

    Animation currentAnimation;
    SpriteRenderer spriteRenderer;

    // ��ü �ִϸ��̼� ����
    public Transform Center;
    float timeSinceLastFrame = 0.0F;

    // Aim �ִϸ��̼� ����
    public GameObject LowerBody;
    int angleScale = 1;
    Vector2 mousePos0;

    // Sample �ִϸ��̼� ����
    int sampleCurrentIndex = 0;
    public float sampleAnimationSpeed; //�ʴ� �����Ӽ�(FPS)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAnimation = Animation.AIM;
    }

    public void SetAnimation(Animation anim)
    {
        if (anim == currentAnimation) return;

        /// EXIT: ���� �ִϸ��̼ǿ��� ������ �� �������� ��
        switch (currentAnimation)
        {
            case Animation.AIM:
                LowerBody.SetActive(false);
                break;
            case Animation.SAMPLE:
                break;
            // ...
            default:
                Debug.LogError("����: ������ ��ü �ִϸ��̼� ����");
                break;
        }

        /// ENTER: �� �ִϸ��̼����� ���� �� �� �� ������ ��
        switch (anim)
        {
            case Animation.AIM:
                LowerBody.SetActive(true);
                break;
            case Animation.SAMPLE:
                break;
            // ...
            default:
                Debug.LogError("����: ������ ��ü �ִϸ��̼� ����");
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

        /// �� �ִϸ��̼Ǻ� UPDATE�� ����
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
                Debug.LogError("����: ������ ��ü �ִϸ��̼� ����");
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