using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorVer2 : MonoBehaviour
{
    // PUBLIC
    public string spriteFileName;

    public int startIndex;
    public int endIndex;

    public bool looping;
    public float fps;

    // PRIVATE
    SpriteRenderer spriteRenderer;
    Sprite[] sprites;

    float timeSinceLastFrame;
    int currentIndex;

    int GetIndicesCount()
    {
        return endIndex - startIndex + 1;
    }

    void Start()
    {
        currentIndex = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(spriteFileName);
    }

    void Update()
    {
        if (!looping && currentIndex >= GetIndicesCount() - 1) return;

        timeSinceLastFrame += Time.deltaTime;

        if (fps <= 0.0F) return;

        if (timeSinceLastFrame >= 1.0F / fps)
        {
            timeSinceLastFrame = 0.0F;
            currentIndex = (currentIndex + 1) % GetIndicesCount();
        }

        spriteRenderer.sprite = sprites[startIndex + currentIndex];
    }
}
