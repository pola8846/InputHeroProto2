using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation<T> : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;//담당하는 스프라이트 랜더러
    protected Material material;//적용할 매터리얼
    protected TickTimer timer;//시간 체크용 타이머


    [SerializeField]
    protected T sourceUnit;//모체 유닛

    [SerializeField]
    protected SpriteAnimationClip nowSpriteList;//현재 표시 중인 애니메이션 클립
    [SerializeField]
    protected int nowSpriteNum;//현재 표시 중인 스프라이트 인덱스

    [SerializeField]
    protected string targetSpriteName;//스프라이트 지정용 임시 캐시
    [SerializeField]
    protected bool isTestMode;

    [SerializeField]
    protected List<SpriteAnimationClip> spriteList;//재생 가능한 애니메이션 클립 리스트
    [SerializeField]
    protected string startSpriteList;//최초에 사용할 애니메이션 클립

    public bool flip;//true면 뒤집혀서 표시
    public bool skip;//true면 표시하지 않음

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = new TickTimer();
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

    //해당 클립의 해당 인덱스로 전환
    public void ChangeSpriteList(string name, int num = 0)
    {
        foreach (var sprite in spriteList)
        {
            if (sprite == null) continue;

            if (name == sprite.keycode)
            {
                nowSpriteList = sprite;
            }
        }
        ChangeSprite(num);
    }

    //해당 인덱스로 전환
    public void ChangeSprite(int num)
    {
        if (nowSpriteList == null || num < 0 || nowSpriteList.sprites.Count <= num)
        {
            return;
        }
        spriteRenderer.sprite = nowSpriteList.sprites[num];
        nowSpriteNum = num;
    }

    /// <summary>
    /// 특정 두 범위 사이의 비율을 스프라이트 인덱스로 변환하여 적용. 시간이나 각도 등에 따라 적절한 인덱스를 찾아줌
    /// </summary>
    /// <param name="delta">찾을 값</param>
    /// <param name="min">기준 최소 값</param>
    /// <param name="max">기준 최대 값</param>
    public void ChangeSprite(float delta, float min, float max)
    {
        int num = GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count - 1, delta, min, max);
        spriteRenderer.sprite = nowSpriteList.sprites[num];
        nowSpriteNum = num;
    }

    /// <summary>
    /// 다음 스프라이트로 전환
    /// </summary>
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

    /// <summary>
    /// 원하는 만큼 스프라이트 전환. 음수를 넣으면 이전 스프라이트로 바뀜
    /// </summary>
    /// <param name="num">한 번에 전환할 스프라이트 수</param>
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

    /// <summary>
    /// 이전 스프라이트로 전환
    /// </summary>
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


    /// <summary>
    /// 원하는 만큼 스프라이트 전환(역방향). 음수를 넣으면 다음 스프라이트로 바뀜
    /// </summary>
    /// <param name="num">한 번에 전환할 스프라이트 수</param>
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