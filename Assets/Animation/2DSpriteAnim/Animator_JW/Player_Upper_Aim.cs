using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Upper_Aim : SpriteAnim
{
    public Player_Upper_Aim(JWAnimType t)
        : base(t) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Run()
    {
        // 스프라이트 업데이트
        eulerAngleConverter();
        mousePos0 = GameManager.MousePos;
        currentIndex = angleScale - 1;

        // 플립 업데이트
        if (targetParents.transform.position.x >= mousePos0.x)
        {
            flip = false;
        }
        else
        {
            flip = true;
        }

        base.Run();
    }
    public override void Exit()
    {
        base.Exit();
    }

    [SerializeField]
    GameObject targetParents;

    Vector2 mousePos0;
    int angleScale;

    void eulerAngleConverter()
    {
        Vector2 nowdir = (mousePos0 - new Vector2(targetParents.transform.position.x, targetParents.transform.position.y)).normalized;
        float nowAnlge = GameTools.GetDegreeAngleFormDirection(nowdir);
        float convertedAngle = Mathf.Clamp(Mathf.Ceil(Mathf.Abs(nowAnlge / 4.285714285714286f)), 0, 41);
        angleScale = (int)(convertedAngle + 1);
    }
}
