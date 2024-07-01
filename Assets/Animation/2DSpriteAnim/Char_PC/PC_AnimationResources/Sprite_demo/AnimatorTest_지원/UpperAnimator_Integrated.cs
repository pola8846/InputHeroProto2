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

// ����: enum���� �ٸ� �ִϸ��̼ǵ� �߰��� �� �ְ� ���� ���� ����
public class UpperAnimator_Integrated : MonoBehaviour
{
    public enum AnimationType
    {
        AIM,
        SAMPLE
        // ...
    }

    /// �ִϸ����� ����
    AnimationType currentAnimation;
    SpriteRenderer spriteRenderer;

    /// ��� �ִϸ��̼� ����
    public Transform Center;
    float timeSinceLastFrame = 0.0F;

    /// Ư�� �ִϸ��̼�
    // AIM
    public List<Sprite> AimAnimationSprites;
    public GameObject LowerBody;
    int angleScale = 1;
    Vector2 mousePos0;

    /// �Ϲ� �ִϸ��̼� - ��������Ʈ ������ ��
    // SAMPLE
    public BasicAnimationInfo SampleAnimation;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentAnimation = AnimationType.AIM;
    }

    public void SetAnimation(AnimationType anim)
    {
        //if (anim == currentAnimation) return; // �� ������ �����..

        /// EXIT: ���� �ִϸ��̼ǿ��� ������ �� �������� ��
        switch (currentAnimation)
        {
            case AnimationType.AIM:
                LowerBody.SetActive(false);
                break;
            case AnimationType.SAMPLE:
                break;
            // ...
            default:
                Debug.LogError("����: ������ ��ü �ִϸ��̼� ����");
                break;
        }

        /// ENTER: �� �ִϸ��̼����� ���� �� �� �� ������ ��
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
                Debug.LogError("����: ������ ��ü �ִϸ��̼� ����");
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

        // flip�� �Ƹ��� �ִϸ��̼� �����̹Ƿ�..
        FlipUpdate();

        /// �� �ִϸ��̼Ǻ� UPDATE�� ����
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
                Debug.LogError("����: ������ ��ü �ִϸ��̼� ����");
                break;
        }
    }

    void AimUpdate()
    {
        spriteRenderer.sprite = AimAnimationSprites[angleScale - 1];
        eulerAngleConverter();
    }

    // ��������Ʈ �� �����ִ� �Ϲ� �ִϸ��̼��� �� ������Ʈ �Լ��� ���������
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