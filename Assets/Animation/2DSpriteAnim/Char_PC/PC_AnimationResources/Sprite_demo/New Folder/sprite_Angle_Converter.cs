using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class sprite_Angle_Converter : MonoBehaviour
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


        
        spriteRenderer.sprite = findSprite[angleScale - 1];

    }
}
