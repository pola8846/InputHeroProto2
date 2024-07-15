using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation<T> : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Material material;

    [SerializeField]
    protected T sourceUnit;

    [SerializeField]
    protected spriteAnimationList nowSpriteList;
    [SerializeField]
    protected int nowSpriteNum;

    [SerializeField]
    protected string targetSpriteName;
    [SerializeField]
    protected bool isTestMode;

    [SerializeField]
    protected List<spriteAnimationList> spriteList;
    [SerializeField]
    protected string startSpriteList;

    public bool flip;
    public bool skip;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        ChangeSpriteList(startSpriteList);
    }

    protected virtual void Update()
    {
        if (skip)
        {
            spriteRenderer.sprite = null;
        }
    }

    public void ChangeSpriteList(string name, int num = 0)
    {
        foreach (var sprite in spriteList)
        {
            if (sprite == null) continue;

            if (name == sprite)
            {
                nowSpriteList = sprite;
            }
        }
        ChangeSprite(num);
    }

    public void ChangeSprite(int num)
    {
        if (nowSpriteList == null || num < 0 || nowSpriteList.sprites.Count <= num)
        {
            return;
        }
        spriteRenderer.sprite = nowSpriteList.sprites[num];
        nowSpriteNum = num;
    }

    public void ChangeSprite(float delta, float min, float max)
    {
        int num = GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count - 1, delta, min, max);
        spriteRenderer.sprite = nowSpriteList.sprites[num];
        nowSpriteNum = num;
    }

    public void ChangeSpriteNext()
    {
        if (nowSpriteList.sprites.Count == 0)
        {
            return;
        }
        nowSpriteNum++;
        nowSpriteNum %= nowSpriteList.sprites.Count;
        spriteRenderer.sprite = nowSpriteList.sprites[nowSpriteNum];
    }

    public void ChangeSpriteNext(int num)
    {
        if (num < 0)
        {
            ChangeSpritePrevious(-num);
            return;
        }

        for (int i = 0; i < num; i++)
        {
            ChangeSpriteNext();
        }
    }

    public void ChangeSpritePrevious()
    {
        if (nowSpriteList.sprites.Count == 0)
        {
            return;
        }
        nowSpriteNum--;
        if (nowSpriteNum < 0)
            nowSpriteNum = nowSpriteList.sprites.Count - 1;
        spriteRenderer.sprite = nowSpriteList.sprites[nowSpriteNum];
    }

    public void ChangeSpritePrevious(int num)
    {
        if (num < 0)
        {
            ChangeSpriteNext(-num);
            return;
        }

        for (int i = 0; i < num; i++)
        {
            ChangeSpritePrevious();
        }
    }
}

[Serializable]
public class spriteAnimationList
{
    public string name;
    public List<Sprite> sprites;

    // == 연산자 오버로딩
    public static bool operator ==(spriteAnimationList sal, string name)
    {
        return sal.name == name;
    }

    public static bool operator ==(string name, spriteAnimationList sal)
    {
        return sal.name == name;
    }

    // != 연산자 오버로딩
    public static bool operator !=(spriteAnimationList sal, string name)
    {
        return sal.name != name;
    }

    public static bool operator !=(string name, spriteAnimationList sal)
    {
        return sal.name != name;
    }

    // Equals 메서드 오버라이딩
    public override bool Equals(object obj)
    {
        if (obj is spriteAnimationList)
        {
            var sal = (spriteAnimationList)obj;
            return this.name == sal.name;
        }
        else if (obj is string)
        {
            var name = (string)obj;
            return this.name == name;
        }
        return false;
    }

    // GetHashCode 메서드 오버라이딩
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}