using UnityEngine;

[System.Serializable]
public class AnimationVer2
{
    public int startIndex;  // ��������Ʈ ���� ù ��° �ε���
    public int endIndex;    // ��������Ʈ ���� ������ �ε���

    protected int currentIndex;   // ����� ���Ǹ� ���� �� �ִϸ��̼� ���ο����� 0���� ��������Ʈ ���������� �ε����� ������ ����Ѵ�
    public bool flip = false;

    public virtual void Enter() { } // �ִϸ��̼� ���Խ� �� �� ����� �ڵ�
    public virtual void Run() { }   // �ִϸ��̼� ����� �� ������ ������Ʈ�� �ڵ�
    public virtual void Exit() { }  // �ִϸ��̼ǿ��� ������ �� �� ����� �ڵ�

    public int GetSpriteListIndex() // �ִϸ��̼� ������ �ε����� ��������Ʈ ���� �ε����� ����
    {
        return Mathf.Clamp(startIndex + currentIndex, startIndex, endIndex);
    }

    protected int GetIndicesCount() // �� �ִϸ��̼��� ��������Ʈ ��������
    {
        return endIndex - startIndex + 1;
    }
}

// �׳� ��������Ʈ ������ �ִϸ��̼�
[System.Serializable]
public class BasicSpriteAnimVer2 : AnimationVer2
{
    public bool looping;
    public float fps;
    float timeSinceLastFrame;

    public override void Enter()
    {
        currentIndex = 0;
        timeSinceLastFrame = 0.0F;

        base.Enter();
    }

    public override void Run()
    {
        if (!looping && currentIndex >= GetIndicesCount() - 1) return;

        timeSinceLastFrame += Time.deltaTime;

        if (fps <= 0.0F) return;

        if (timeSinceLastFrame >= 1.0F / fps)
        {
            timeSinceLastFrame = 0.0F;
            currentIndex = (currentIndex + 1) % GetIndicesCount();
        }

        base.Run();
    }

    public override void Exit()
    {
        base.Exit();
    }
}