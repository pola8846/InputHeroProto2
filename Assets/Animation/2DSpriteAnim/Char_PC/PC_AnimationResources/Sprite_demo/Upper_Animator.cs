using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Upper_Animator : MonoBehaviour
{
    [SerializeField, Range(1, 32)] int angleScale;

    float angle;

    SpriteRenderer spriteRenderer;
    Material material;
    int row;
    int col;
    int x;
    int y;

    public Texture2D spriteSheet;

    public Sprite[] findSprite;

   
    [SerializeField, Range(0, 180f)] float nowAnlge;
    float convertedAngle;
    [SerializeField] bool flip;
    Vector2 mousePos0;
    [SerializeField] GameObject targetParents;

    private void Awake()
    {

        

    }
    // Start is called before the first frame update
    void Start()
    {


        row = 11; col = 6;

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
            spriteRenderer.flipX = !enabled;
        }
        else
        { spriteRenderer.flipX = enabled; }


        spriteRenderer.sprite = findSprite[angleScale - 1];
        eulerAngleConverter();


    }


    void eulerAngleConverter()
    {

        Vector2 nowdir = (mousePos0 - new Vector2(targetParents.transform.position.x, targetParents.transform.position.y)).normalized;

        nowAnlge = GameTools.GetDegreeAngleFormDirection(nowdir);

        convertedAngle = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowAnlge / 5.625f)),0,31);
        
        angleScale = (int)(convertedAngle+1);

    }
}
