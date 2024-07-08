using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationVer2
{
    public int startIndex;
    public int endIndex;
    public bool looping;
    public float fps;

    float timeSinceLastFrame;
    int currentIndex;

    public int GetSpriteListIndex()
    {
        return startIndex + currentIndex;
    }
        
    int GetIndicesCount()
    {
        return endIndex - startIndex + 1;
    }

    public void Enter()
    {
        currentIndex = 0;
        timeSinceLastFrame = 0.0F;
    }

    public void Run()
    {
        if (!looping && currentIndex >= GetIndicesCount() - 1) return;

        timeSinceLastFrame += Time.deltaTime;

        if (fps <= 0.0F) return;

        if (timeSinceLastFrame >= 1.0F / fps)
        {
            timeSinceLastFrame = 0.0F;
            currentIndex = (currentIndex + 1) % GetIndicesCount();
        }
    }

    public void Exit()
    {
        
    }
}
