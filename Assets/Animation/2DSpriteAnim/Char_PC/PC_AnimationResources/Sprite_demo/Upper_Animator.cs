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
    public bool flip;
    Vector2 mousePos0;
    [SerializeField] GameObject targetParents;

    TickTimer tickTimer;

    public bool tickReload = false;

    private void Awake()
    {

        tickTimer = new();

    }
    // Start is called before the first frame update
    void Start()
    {



        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSheet = spriteRenderer.sprite.texture;
        material = spriteRenderer.material;

    }

    // Update is called once per frame
    void Update()
    {


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


        if (!tickReload)
        {
            animation_Aim();
        }
        else if (tickReload)
        {
            reload();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            tickReload = true;
        }
   




    }


    void eulerAngleConverter()
    {

        Vector2 nowdir = (mousePos0 - new Vector2(targetParents.transform.position.x, targetParents.transform.position.y)).normalized;

        nowAnlge = GameTools.GetDegreeAngleFormDirection(nowdir);

        convertedAngle = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowAnlge / 4.285714285714286f)), 0, 41);

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
        nowReload = (int)convertedReload + 3;
    }

}