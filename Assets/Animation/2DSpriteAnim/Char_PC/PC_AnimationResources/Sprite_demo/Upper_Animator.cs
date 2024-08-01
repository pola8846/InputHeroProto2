using UnityEngine;

public class Upper_Animator : SpriteAnimation<PlayerUnit>
{
    protected override void Update()
    {
        //���ε� ���̸� ���ε�, �ƴϸ� ����
        if (!sourceUnit.IsReloading)
        {
            Animation_Aim();
        }
        else if (sourceUnit.IsReloading)
        {
            Reload();
        }
        base.Update();
    }

    /// <summary>
    /// ���� ��ȯ
    /// </summary>
    public void FlipCheck()
    {
        flip = !sourceUnit.IsMouseLeft;
        spriteRenderer.flipX = !flip;
    }


    /// <summary>
    /// ���� �ִϸ��̼�
    /// </summary>
    void Animation_Aim()
    {
        if (nowSpriteList.name != "Find")
        {
            ChangeSpriteList("Find");
        }
        ChangeSprite(Mathf.Abs(sourceUnit.NowMouseAngle), 0, 180);
    }

    /// <summary>
    /// ������ �ִϸ��̼�
    /// </summary>
    void Reload()
    {
        if (nowSpriteList.name != "Reload")
        {
            ChangeSpriteList("Reload");
        }
        ChangeSprite(sourceUnit.RemainReloadTime, 0, sourceUnit.ReloadTime);
    }
}