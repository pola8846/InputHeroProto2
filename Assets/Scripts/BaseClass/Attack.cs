using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 같은 판정으로 취급되는 공격 전체
/// </summary>
public class Attack : MonoBehaviour
{
    /// <summary>
    /// 공격하는 유닛
    /// </summary>
    private Unit attackUnit;
    /// <summary>
    /// 공격하는 유닛
    /// </summary>
    public Unit AttackUnit
    {
        get
        {
            return attackUnit;
        }
    }
    [SerializeField]
    private string targetTag;//피해를 입힐 대상의 태그
    private List<DamageArea> damageAreaList = new();//등록된 대미지
    private List<Unit> damagedUnitList = new();//대미지 받은 유닛 리스트
    private bool isInitialized = false;//초기화 되었는가?

    public bool dealDamageOnEnter = true;//접촉 시 대미지를 입히는가?
    public bool isDestroySelfAuto = true;//대미지가 전부 사라지면 파괴되는가?

    protected virtual void Update()
    {
        PerformanceManager.StartTimer("Attack.Update");
        //초기화 안했을 때 예외
        if (!isInitialized)
        {
            PerformanceManager.StopTimer("Attack.Update");
            return;
        }

        //isDestroySelfAuto 활성화 시 등록된 모든 대미지가 사라지면 파괴
        if (isDestroySelfAuto && damageAreaList.Count == 0)
        {
            Destroy(gameObject);
        }

        
        PerformanceManager.StopTimer("Attack.Update");
    }

    /// <summary>
    /// 동적 생성. 초기화도 동시에 이루어짐.
    /// 스크립트에서 위치를 지정해 줄 때 사용하면 될 것
    /// 주로 투사체 발사
    /// </summary>
    /// <param name="unit">공격자 유닛</param>
    /// <param name="targetTag">피격 대상의 태그</param>
    /// <param name="parent">부모가 될 Transform. null이면 부모가 없음</param>
    /// <param name="DamageAreaObjects">초기에 등록할 대미지의 게임 오브젝트</param>
    /// <returns>생성된 Attack</returns>
    public static Attack MakeGameObject(Unit unit, string targetTag, Transform parent = null, params GameObject[] DamageAreaObjects)
    {
        GameObject go = new GameObject();
        if (parent != null)
        {
            go.transform.parent = parent;
        }
        Attack result = go.AddComponent<Attack>();
        result.Initialization(unit, targetTag, DamageAreaObjects);
        return result;
    }

    /// <summary>
    /// 초기화
    /// 사용 전에 반드시 초기화하여야 함
    /// </summary>
    /// <param name="unit">공격자 유닛</param>
    /// <param name="targetTag">피격 대상의 태그</param>
    /// <param name="DamageAreaObjects">초기에 등록할 대미지의 게임 오브젝트</param>
    public void Initialization(Unit unit, string targetTag, params GameObject[] DamageAreaObjects)
    {
        attackUnit = unit;
        this.targetTag = targetTag;
        damageAreaList.Clear();
        damagedUnitList.Clear();
        foreach (var obj in DamageAreaObjects)
        {
            EnrollDamage(obj);
        }
        isInitialized = true;
    }

    /// <summary>
    /// 대미지 등록
    /// </summary>
    /// <param name="damageArea">등록할 대미지</param>
    public void EnrollDamage(DamageArea damageArea)//대미지 하나 등록
    {
        if (damageAreaList.Contains(damageArea))
        {
            return;
        }
        damageAreaList.Add(damageArea);
        damageArea.Source = this;
    }

    /// <summary>
    /// 본인 혹은 자식의 모든 대미지 등록
    /// </summary>
    /// <param name="go">기준이 될 오브젝트</param>
    public void EnrollDamage(GameObject go)//한 번에 여러 대미지 등록
    {
        foreach (DamageArea damageArea in go.GetComponentsInChildren<DamageArea>())
        {
            EnrollDamage(damageArea);
        }
    }

    /// <summary>
    /// 대미지 해제
    /// </summary>
    /// <param name="damageArea">해제할 대미지</param>
    public void WithdrawDamage(DamageArea damageArea)//대미지 해제
    {
        if (damageAreaList.Contains(damageArea))
        {
            damageAreaList.Remove(damageArea);
        }
    }

    /// <summary>
    /// 본인 혹은 자식의 모든 대미지 해제
    /// </summary>
    /// <param name="go">기준이 될 오브젝트</param>
    public void WithdrawDamage(GameObject go)
    {
        foreach (DamageArea damageArea in go.GetComponentsInChildren<DamageArea>())
        {
            WithdrawDamage(damageArea);
        }
    }

    public void DamageEnter()
    {
        PerformanceManager.StartTimer("Attack.DamageEnter");
        //dealDamageOnEnter 활성화 시 접촉할 때 대미지
        if (dealDamageOnEnter)
        {
            foreach (var contactedUnit in GetConnectedHitbox())
            {
                if (!damagedUnitList.Contains(contactedUnit.Key))
                {
                    damagedUnitList.Add(contactedUnit.Key);
                    contactedUnit.Value.Value.DealDamage(contactedUnit.Value.Key);
                }
            }
        }
        PerformanceManager.StopTimer("Attack.DamageEnter");
    }

    public void DamageExit()
    {

    }

    /// <summary>
    /// 접촉한 유닛 탐색
    /// 등록된 Damage에 접촉 중인 모든 유닛을 읽어서, 우선도를 고려하여 어느 대미지가 적용될 지 알려줌
    /// </summary>
    private Dictionary<Unit, KeyValuePair<HitBox, DamageArea>> GetConnectedHitbox()
    {
        PerformanceManager.StartTimer("Attack.GetConnectedHitbox");
        Dictionary<HitBox, DamageArea> dict = new();//우선권을 고려하여, 어느 히트박스에게 어떤 피해 영역이 충돌한 것으로 볼지 기록
        Dictionary<Unit, KeyValuePair<HitBox, DamageArea>> temp = new();
        //temp 채우기
        foreach (DamageArea damageArea in damageAreaList)//모든 피해 영역에 대해
        {
            foreach (HitBox hitBox in damageArea.HitBoxList)//충돌 중인 모든 히트박스에 대해
            {
                if (hitBox.CompareTag(targetTag))//목표 유닛이면
                {
                    if (!temp.ContainsKey(hitBox.Unit))//처음 충돌한 유닛이면
                    {
                        temp.Add(hitBox.Unit, new KeyValuePair<HitBox, DamageArea>(hitBox, damageArea));//등록한다
                    }
                    else//이전에 충돌 검사된 유닛이라면
                    {
                        if (temp[hitBox.Unit].Key.Priority + temp[hitBox.Unit].Value.Priority < hitBox.Priority + damageArea.Priority)//우선도 합이 더 높으면
                        {
                            temp[hitBox.Unit] = new KeyValuePair<HitBox, DamageArea>(hitBox, damageArea);//등록한다
                        }
                    }
                }
            }
        }

        ///
        /// 원래는 Attack에서 한 유닛에 한 번만 공격 체크
        /// 히트박스 사이 구분 없이 그냥 Unit으로 등록하여 충돌 중인 유닛을 감지
        /// 피해 영역의 우선도만 고려하여 등록하였음
        /// 
        /// <Unit, <HitBox, DamageArea>>로 한다면
        /// 첫 유닛을 찾으면 유닛을 key로, valued에 딕셔너리를 생성, 충돌한 hitbox를 key로, 충돌한 DamageArea를 value로 넣는다
        /// 같은 유닛이라면 hitbox나 damagearea의 우선도를 합산, 높으면 대체함
        ///

        PerformanceManager.StopTimer("Attack.GetConnectedHitbox");

        return temp;
    }

}