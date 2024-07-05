using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class paticle_Movement : MonoBehaviour
{
    ParticleSystem afterImage;
    public Sprite afterSprite;
    public SpriteRenderer spriteRendererP;
    public ParticleSystemRenderer spriteRendererR;
    bool branch;
    
    [SerializeField] Vector3 flipX;
    [SerializeField] bool flip;


    void Start()
    {
        afterImage = GetComponent<ParticleSystem>();
        spriteRendererP = GetComponentInParent<SpriteRenderer>();
        spriteRendererR = GetComponent<ParticleSystemRenderer>();

        if (GetComponentInParent<Upper_Animator>() == null)
        {
            branch = true;
            flip = GetComponentInParent<Down_Animator>().flip;
        }
        else if (GetComponentInParent<Down_Animator>() == null)
        {
            branch = false;
            flip = GetComponentInParent<Upper_Animator>().flip;
        }


    }

    /// <summary>
    /// 
    /// row와 col을 animation sheet의 num을 따서 환산 가능해야 한다.
    /// ex 1번부터~ 1,0 2,0 3,0 4,0 ~~~
    /// 9,0 ~1,1
    /// 
    /// n진법 사용 - 응용하면 될듯
    /// 
    ///해당 스프라이트를 바로 받아서 파티클 시스템 시트에 넣기
    /// 
    /// </summary>

    void Update()
    {
        setflip();
        setSprite();
    }

    void setSprite()
    {
        afterSprite = spriteRendererP.sprite;
        afterImage.textureSheetAnimation.RemoveSprite(0);
        afterImage.textureSheetAnimation.AddSprite(afterSprite);
        afterImage.textureSheetAnimation.SetSprite(0, afterSprite);
        
    }

    void setflip()
    {

        if (branch)
        {
            flip = GetComponentInParent<Down_Animator>().flip;
        }
        else
        {
            flip = GetComponentInParent<Upper_Animator>().flip;
        }

        if (flip)
        {
            flipX = Vector3.zero;
        } else if (!flip)
        {
            flipX = Vector3.right;
        }

        spriteRendererR.flip = flipX;

    }
}
