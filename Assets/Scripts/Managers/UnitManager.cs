using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //싱글톤
    private static UnitManager instance;
    public static UnitManager Instance => instance;

    private SerializedDictionary<int, Unit> unitList = new();
    private int lastUnitNum = 0;

    //싱글톤
    private void Awake()
    {
        //싱글톤
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// 유닛 등록
    /// </summary>
    /// <param name="unit">등록하려는 유닛</param>
    /// <returns>할당된 유닛 ID</returns>
    public int EnrollUnit(Unit unit)
    {
        lastUnitNum++;
        unitList.Add(lastUnitNum, unit);
        return lastUnitNum;
    }

    /// <summary>
    /// 유닛 등록 해제
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit.UnitID);
    }

    /// <summary>
    /// 대미지넣기
    /// </summary>
    /// <param name="target">받는 대상</param>
    /// <param name="source">주는 대상</param>
    /// <param name="damage">대미지 클래스</param>
    /// <returns>생존여부</returns>
    public bool DamageUnitToUnit(Unit target, Unit source, DamageArea damageArea)
    {
        if (target == null || source == null || damageArea == null)
        {
            return true;
        }
        target.Damage(damageArea.damage);
        return true;
        //임시
    }

    public bool DamageUnitToUnit(Unit target, Unit source, float damage)
    {
        if (target == null || source == null)
        {
            return true;
        }
        target.Damage(damage);
        return true;
        //임시
    }

    public bool DamageUnitToHitbox(HitBox target, Unit source, DamageArea damageArea)
    {
        if (target == null || source == null || damageArea == null)
        {
            return true;
        }

        target.Damage(damageArea.damage);
        //Debug.Log($"피해 입힘:{source.name}이 {target.Unit.name}에게, {damageArea.damage} 피해");

        return true;
        //임시
    }
}
