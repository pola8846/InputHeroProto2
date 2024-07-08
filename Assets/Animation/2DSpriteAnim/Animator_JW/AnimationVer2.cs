using UnityEngine;

[System.Serializable]
public class AnimationVer2
{
    public int startIndex;  // 스프라이트 기준 첫 번째 인덱스
    public int endIndex;    // 스프라이트 기준 마지막 인덱스

    protected int currentIndex;   // 계산의 편의를 위해 각 애니메이션 내부에서는 0부터 스프라이트 개수까지의 인덱스를 가지고 계산한다
    public bool flip = false;

    public virtual void Enter() { } // 애니메이션 진입시 한 번 실행될 코드
    public virtual void Run() { }   // 애니메이션 진행시 매 프레임 업데이트될 코드
    public virtual void Exit() { }  // 애니메이션에서 나갈때 한 번 실행될 코드

    public int GetSpriteListIndex() // 애니메이션 내부의 인덱스를 스프라이트 기준 인덱스로 뱉어내줌
    {
        return startIndex + currentIndex;
    }

    protected int GetIndicesCount() // 이 애니메이션의 스프라이트 개수리턴
    {
        return endIndex - startIndex + 1;
    }
}

// 그냥 스프라이트 돌리는 애니메이션
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