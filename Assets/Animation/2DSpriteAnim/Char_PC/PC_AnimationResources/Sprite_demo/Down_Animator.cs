using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Down_Animator : MonoBehaviour
{
    [SerializeField, Range(1, 42)] int transformChange;
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
    public Sprite[] jumpSprite;

    public Sprite Stand;

    public bool flip;

    Vector2 mousePos0;

    public GameObject upper;

    public Sprite skip;

    [SerializeField] GameObject targetParents;

    public GameObject player;
    PlayerUnit playerUnit;

    [SerializeField] int maxJumpcount;

    void Start()
    {



        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteSheet = spriteRenderer.sprite.texture;
        material = spriteRenderer.material;
        playerUnit = player.GetComponent<PlayerUnit>();

    }

    public bool jump;

    private void Update()
    {

        mousePos0 = GameManager.MousePos;
       
        transformChecker();
        dirSwitcher();

        if(Input.GetKeyDown(KeyCode.Space)&& !jump)
        {
            jump = true;

        }

        if(jump)
        {
            jumpingSprite();

            if(playerUnit.CanJumpCounter == maxJumpcount)
            {
                nowJumpTime = 0;
                jump = false;
            }

        }
        else if(!jump)
        {
            if (!flip)
            {
                if (transform0 > maxXValue)
                {
                    spriteChanger(transformChange);
                }
                else if (transform0 < -maxXValue)
                {
                    spriteChangerReversed(transformChange);
                }
            }
            else if (flip)
            {
                if (transform0 > maxXValue)
                {
                    spriteChangerReversed(transformChange);
                }
                else if (transform0 < -maxXValue)
                {
                    spriteChanger(transformChange);
                }
            }
        }
        




        if(upper.GetComponent<Upper_Animator>().dashing)
        {
            spriteRenderer.sprite = skip;
        }
            /*if (GameManager.Player.IsLookLeft)
            {
                spriteRenderer.flipX = enabled;
            } else {spriteRenderer.flipX =!enabled; }*/




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
        } else if (spriteNum == findSprite.Length-1)
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
        else if (spriteNum == findSprite.Length - 1)
        {
            changeType = 1;
        }
        else { changeType = 2; }

        switch (changeType)
        {
            case 0:
                spriteNum = findSprite.Length - 1;
                break;
            case 1:
                spriteNum --;
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
        transform0 = 0;
    }

    private void setSprite()
    {
        spriteRenderer.sprite = findSprite[transformChange];
    }


    private void dirSwitcher()
    {
        if (targetParents.transform.position.x >= mousePos0.x)
        {
            spriteRenderer.flipX = !enabled;
            
            
            flip = true;
        }
        else


        { spriteRenderer.flipX = enabled;

            flip = false;
        }
    }


    [SerializeField] float nowJumpTime;
    [SerializeField] float maxJump;
    [SerializeField] float timeManifulatorJump;
    [SerializeField] int nowJump;
    [SerializeField] float convertedjump;


    void jumpingSprite()
    {
        nowJumpTime = nowJumpTime + Time.deltaTime * timeManifulatorJump;

        convertedjump = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowJumpTime / (maxJump /  jumpSprite.Length ))) , 0 , jumpSprite.Length - 1);
        nowJump = (int)convertedjump + 1;

        spriteRenderer.sprite = jumpSprite[nowJump-1];
    }
}
