using AYellowpaper.SerializedCollections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //싱글톤
    private static UnitManager instance;
    public static UnitManager Instance => instance;

    private static SerializedDictionary<string, Unit> unitList;

    private static SerializedDictionary<string, Unit> UnitList
    {
        get
        {
            if (unitList == null)
            {
                unitList = new SerializedDictionary<string, Unit>();
                return unitList;
            }
            else
            {
                return unitList;
            }
        }
    }

    private static int lastUnitNum = 0;

    [SerializeField]
    private bool isTestMod = false;

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
    /// 유닛 등록(자동)
    /// </summary>
    /// <param name="unit">등록하려는 유닛</param>
    /// <returns>할당된 유닛 ID</returns>
    public static string EnrollUnit(Unit unit)
    {
        string id = lastUnitNum.ToString();
        while (UnitList.ContainsKey(id))
        {
            lastUnitNum++;
            id = lastUnitNum.ToString();
        }
        UnitList.Add(id, unit);
        if (Instance != null && Instance.isTestMod)
        {
            Debug.Log($"{unit.name}이 '{id}' ID로 등록됨");
        }
        return id;
    }

    /// <summary>
    /// 유닛 등록(수동)
    /// </summary>
    /// <param name="unit">등록하려는 유닛</param>
    /// <returns>할당된 유닛 ID</returns>
    public static void EnrollUnit(Unit unit, string id)
    {
        if (UnitList.ContainsKey(id))
        {
            Debug.LogError($"{unit.name}에서 이미 등록된 ID인 '{id}'를 등록하려고 시도함");
            return;
        }
        UnitList.Add(id, unit);
        if (Instance != null && Instance.isTestMod)
        {
            Debug.Log($"{unit.name}이 '{id}' ID로 등록됨");
        }
    }

    /// <summary>
    /// 유닛 등록 해제
    /// </summary>
    /// <param name="unit"></param>
    public static void RemoveUnit(Unit unit)
    {
        UnitList.Remove(unit.UnitID);
        if (Instance != null && Instance.isTestMod)
        {
            Debug.Log($"{unit.name}의 ID 등록이 해제됨");
        }
    }

    public static Unit GetUnitByID(string unitID)
    {
        UnitList.TryGetValue(unitID, out Unit unit);
        if (unit == null)
        {
            Debug.LogError($"없는 유닛 ID 호출 시도: {unitID}");
        }
        return unit;
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
