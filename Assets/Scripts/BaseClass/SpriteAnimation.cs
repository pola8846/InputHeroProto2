using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation<T> : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;//����ϴ� ��������Ʈ ������
    protected Material material;//������ ���͸���
    protected TickTimer timer;//�ð� üũ�� Ÿ�̸�


    [SerializeField]
    protected T sourceUnit;//��ü ����

    [SerializeField]
    protected SpriteAnimationClip nowSpriteList;//���� ǥ�� ���� �ִϸ��̼� Ŭ��
    [SerializeField]
    protected int nowSpriteNum;//���� ǥ�� ���� ��������Ʈ �ε���

    [SerializeField]
    protected string targetSpriteName;//��������Ʈ ������ �ӽ� ĳ��
    [SerializeField]
    protected bool isTestMode;

    [SerializeField]
    protected List<SpriteAnimationClip> spriteList;//��� ������ �ִϸ��̼� Ŭ�� ����Ʈ
    [SerializeField]
    protected string startSpriteList;//���ʿ� ����� �ִϸ��̼� Ŭ��

    public bool flip;//true�� �������� ǥ��
    public bool skip;//true�� ǥ������ ����

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

    //�ش� Ŭ���� �ش� �ε����� ��ȯ
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

    //�ش� �ε����� ��ȯ
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
    /// Ư�� �� ���� ������ ������ ��������Ʈ �ε����� ��ȯ�Ͽ� ����. �ð��̳� ���� � ���� ������ �ε����� ã����
    /// </summary>
    /// <param name="delta">ã�� ��</param>
    /// <param name="min">���� �ּ� ��</param>
    /// <param name="max">���� �ִ� ��</param>
    public void ChangeSprite(float delta, float min, float max)
    {
        int num = GameTools.GetlinearGraphInCount(nowSpriteList.sprites.Count - 1, delta, min, max);
        spriteRenderer.sprite = nowSpriteList.sprites[num];
        nowSpriteNum = num;
    }

    /// <summary>
    /// ���� ��������Ʈ�� ��ȯ
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
    /// ���ϴ� ��ŭ ��������Ʈ ��ȯ. ������ ������ ���� ��������Ʈ�� �ٲ�
    /// </summary>
    /// <param name="num">�� ���� ��ȯ�� ��������Ʈ ��</param>
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
    /// ���� ��������Ʈ�� ��ȯ
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
    /// ���ϴ� ��ŭ ��������Ʈ ��ȯ(������). ������ ������ ���� ��������Ʈ�� �ٲ�
    /// </summary>
    /// <param name="num">�� ���� ��ȯ�� ��������Ʈ ��</param>
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

    // == ������ �����ε�
    public static bool operator ==(spriteAnimationList sal, string name)
    {
        return sal.name == name;
    }

    public static bool operator ==(string name, spriteAnimationList sal)
    {
        return sal.name == name;
    }

    // != ������ �����ε�
    public static bool operator !=(spriteAnimationList sal, string name)
    {
        return sal.name != name;
    }

    public static bool operator !=(string name, spriteAnimationList sal)
    {
        return sal.name != name;
    }

    // Equals �޼��� �������̵�
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

    // GetHashCode �޼��� �������̵�
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}