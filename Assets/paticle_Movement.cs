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
    /// row�� col�� animation sheet�� num�� ���� ȯ�� �����ؾ� �Ѵ�.
    /// ex 1������~ 1,0 2,0 3,0 4,0 ~~~
    /// 9,0 ~1,1
    /// 
    /// n���� ��� - �����ϸ� �ɵ�
    /// 
    ///�ش� ��������Ʈ�� �ٷ� �޾Ƽ� ��ƼŬ �ý��� ��Ʈ�� �ֱ�
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
