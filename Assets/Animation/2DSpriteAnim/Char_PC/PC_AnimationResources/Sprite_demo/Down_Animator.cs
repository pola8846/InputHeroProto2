using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.Rendering.DebugUI.Table;

public class Down_Animator : MonoBehaviour
{
    [SerializeField, Range(1, 32)] int transformChange;
    [SerializeField]float transform0;
    [SerializeField] float xPos;
    [SerializeField, Range(0, 2)] float xPosManifulator;

    [SerializeField, Range(0,.1f)] float limitAxisValue;

    [SerializeField]float maxXValue;

    [SerializeField] float resetValue;

    SpriteRenderer spriteRenderer;
    Material material;

    public Texture2D spriteSheet;

    public Sprite[] findSprite;

    public Sprite Stand;

    [SerializeField] bool flip;

    

    void Start()
    {



        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSheet = spriteRenderer.sprite.texture;
        material = spriteRenderer.material;

    }

    private void Update()
    {

       
        transformChecker();

        if (transform0 > maxXValue)
        {
            spriteChanger(transformChange);
        }
        else if (transform0 < -maxXValue)
        {
            spriteChangerReversed(transformChange);
        }



        if (GameManager.Player.IsLookLeft)
        {
            spriteRenderer.flipX = enabled;
        } else {spriteRenderer.flipX =!enabled; }

    }


    private void transformChecker()
    {
        transform0 += Input.GetAxisRaw("Horizontal") * xPosManifulator;

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            standSprite();
        }
    }

    private void spriteChanger(int spriteNum)
    {
        int changeType;

        if(spriteNum == 0)
        {
            changeType = 0;
        } else if (spriteNum == 31)
        {
            changeType = 1;
        } else { changeType = 2; }

        switch (changeType)
        {
            case 0:
                spriteNum++;
                break;
            case 1:
                spriteNum = 1;
                break;
            case 2:
                spriteNum++;
                break;

        }

        transformChange = spriteNum;
        transform0 = 0;
        setSprite();
    }

    private void spriteChangerReversed(int spriteNum)
    {
        int changeType;

        if (spriteNum == 0)
        {
            changeType = 0;
        }
        else if (spriteNum == 31)
        {
            changeType = 1;
        }
        else { changeType = 2; }

        switch (changeType)
        {
            case 0:
                spriteNum = 31;
                break;
            case 1:
                spriteNum--;
                break;
            case 2:
                spriteNum--;
                break;

        }

        transformChange = spriteNum;
        transform0 = 0;
        setSprite();

    }

    private void standSprite()
    {
        spriteRenderer.sprite = Stand;
    }

    private void setSprite()
    {
        spriteRenderer.sprite = findSprite[transformChange];
    }
}
