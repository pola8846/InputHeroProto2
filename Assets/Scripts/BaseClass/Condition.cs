using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태이상(지속 효과)이 공용으로 사용하는 상위 클래스
/// 상태이상별로 하위 클래스를 두어서 사용
/// 유닛에서 리스트로 관리
/// </summary>
public class Condition
{
    /// <summary>
    /// 지속시간(초)
    /// </summary>
    private float duration;
    /// <summary>
    /// 지속시간(초)
    /// </summary>
    public float Duration
    {
        get
        {
            return duration;
        }
        set
        {
            duration = value;
        }
    }

    /// <summary>
    /// 소유 유닛
    /// </summary>
    private Unit owner;
    /// <summary>
    /// 소유 유닛
    /// </summary>
    public Unit Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    /// <summary>
    /// 부여될 때 효과 트리거
    /// </summary>
    public void OnAddTriger()
    {
        OnAdd();
    }

    /// <summary>
    /// 부여될 때 효과
    /// </summary>
    public virtual void OnAdd()
    {

    }

    /// <summary>
    /// 프레임 당 효과 트리거
    /// </summary>
    public void OnUpdateTriger()
    {
        OnUpdate();
    }

    /// <summary>
    /// 프레임 당 효과
    /// </summary>
    public virtual void OnUpdate()
    {

    }

    /// <summary>
    /// 제거될 때 효과 트리거
    /// </summary>
    public virtual void OnRemoveTriger()
    {
        OnRemove();
    }

    /// <summary>
    /// 제거될 때 효과
    /// </summary>
    public virtual void OnRemove() 
    { 

    }
}
