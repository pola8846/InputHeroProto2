using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    /// <summary>
    /// 모체 유닛. 자동으로 등록됨
    /// </summary>
    private Unit unit;
    /// <summary>
    /// 모체 유닛
    /// </summary>
    public Unit Unit
    {
        get { return unit; }
        set { unit = value; }
    }


    /// <summary>
    /// 우선도. 높은 것을 우선함.
    /// </summary>
    [SerializeField]
    private int priority = 0;
    public int Priority { get => priority; }

    protected virtual void Start()
    {
        //자동 등록
        var temp = GetComponentInParent<Unit>();
        if (temp != null)
        {
            Unit = temp;
            gameObject.layer = LayerMask.NameToLayer("HitBox");
            gameObject.tag = Unit.gameObject.tag;
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            Debug.LogWarning($"HitBox: {gameObject}에서 모체 Unit을 찾지 못함");
        }
    }

    /// <summary>
    /// 히트박스를 통해 대미지를 전달하는 구문
    /// HitBox와 DamageArea의 우선도를 더해 가장 높은 조합 하나만 피격 처리
    /// 상속하여 override하여 피격 시 임의 코드 실행 가능
    /// </summary>
    /// <param name="damage">받을 대미지</param>
    public virtual void Damage(float damage)
    {
        unit.Damage(damage);
    }
}
