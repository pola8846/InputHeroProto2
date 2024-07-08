using UnityEngine;

public class SpritesLoader : MonoBehaviour
{
    public string spriteFileName;

    [HideInInspector]
    public Sprite[] sprites;

    void Start()
    {
        if (spriteFileName != null)
        {
            sprites = Resources.LoadAll<Sprite>(spriteFileName);
        }
    }
}