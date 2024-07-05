using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Upper_Animator : MonoBehaviour
{
    [SerializeField, Range(1, 42)] int angleScale;

    float angle;

    SpriteRenderer spriteRenderer;
    Material material;


    [SerializeField] int startReload;

    public Texture2D spriteSheet;

    public Sprite[] findSprite;
    public Sprite[] reloadSprites;
    

    [SerializeField, Range(0, 180f)] float nowAnlge;
    float convertedAngle;
    [SerializeField] float dampAngles;
    public bool flip;
    Vector2 mousePos0;
    [SerializeField] GameObject targetParents;

    TickTimer tickTimer;

    public bool tickReload = false;

    [SerializeField] GameObject player;
    public bool dashing;

    private void Awake()
    {

        tickTimer = new();

    }
    // Start is called before the first frame update
    void Start()
    {



        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteSheet = spriteRenderer.sprite.texture;
        material = spriteRenderer.material;
       


    }

    // Update is called once per frame
    void Update()
    {
        dashing = player.GetComponent<PlayerUnit>().isDash;

       mousePos0 = GameManager.MousePos;

        /*if (GameManager.Player.IsLookLeft)
        {
            spriteRenderer.flipX = enabled;
        }
        else { spriteRenderer.flipX = !enabled; }
        */

        if (targetParents.transform.position.x >= mousePos0.x)
        {
            flip = true;
            spriteRenderer.flipX = !enabled;
        }
        else
        {   flip = false;
            spriteRenderer.flipX = enabled; }

        if (dashing)
        {
            animation_Dash();
            Debug.Log("dash?");
        }
        else if (!tickReload)
        {
            animation_Aim();
        }
        else if (tickReload)
        {
            reload();
        } 


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            tickReload = true;
        }
   

     if(!dashing)
        {
            nowDashTime = 0;
        }

    }


    void eulerAngleConverter()
    {

        Vector2 nowdir = (mousePos0 - new Vector2(targetParents.transform.position.x, targetParents.transform.position.y)).normalized;

        nowAnlge = GameTools.GetDegreeAngleFormDirection(nowdir);



            convertedAngle = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowAnlge / (180/ findSprite.Length ))), 0, findSprite.Length - 1);

        angleScale = (int)(convertedAngle + 1);

    }

    void animation_Aim()
    {
        spriteRenderer.sprite = findSprite[angleScale - 1];
        eulerAngleConverter();
    }


    [SerializeField] int nowReload;


    void reload()
    {
        reloadSetSprite();

        spriteRenderer.sprite = reloadSprites[Mathf.Clamp(nowReload - 1,0,reloadSprites.Length)];

    }

    [SerializeField] float nowReloadTime;
    [SerializeField] float maxReload;
    [SerializeField] float timeManifulatorReload;
 

    float convertedReload;
    void reloadSetSprite()
    {
        nowReloadTime = nowReloadTime + Time.deltaTime * timeManifulatorReload;
        if (nowReloadTime > maxReload)
        {
            nowReloadTime = 0;
            tickReload = false;

        }

        convertedReload = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowReloadTime / (maxReload / reloadSprites.Length))), 0, reloadSprites.Length - 1);
        nowReload = (int)convertedReload + 1;
    }


    [SerializeField] Sprite[] dashSprite;
    [SerializeField] float nowDashTime;
    [SerializeField] float maxDashjTime;
    [SerializeField] float tMDash;
    float convertedDash;

    [SerializeField] int nowDash;


    void animation_Dash()
    {

        nowDashTime = nowDashTime + Time.deltaTime * tMDash;
        if (nowDashTime > maxDashjTime)
        {
            nowDashTime = 0;
        }

        convertedDash = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowDashTime / (maxDashjTime / dashSprite.Length))), 0, dashSprite.Length - 1);
        nowDash = (int)convertedDash + 1;

        spriteRenderer.sprite = dashSprite[nowDash - 1];
        Debug.Log(dashSprite[nowDash - 1]);

        
    }


}