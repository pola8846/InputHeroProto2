using AYellowpaper.SerializedCollections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //�̱���
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

    //�̱���
    private void Awake()
    {
        //�̱���
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
    /// ���� ���(�ڵ�)
    /// </summary>
    /// <param name="unit">����Ϸ��� ����</param>
    /// <returns>�Ҵ�� ���� ID</returns>
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
            Debug.Log($"{unit.name}�� '{id}' ID�� ��ϵ�");
        }
        return id;
    }

    /// <summary>
    /// ���� ���(����)
    /// </summary>
    /// <param name="unit">����Ϸ��� ����</param>
    /// <returns>�Ҵ�� ���� ID</returns>
    public static void EnrollUnit(Unit unit, string id)
    {
        if (UnitList.ContainsKey(id))
        {
            Debug.LogError($"{unit.name}���� �̹� ��ϵ� ID�� '{id}'�� ����Ϸ��� �õ���");
            return;
        }
        UnitList.Add(id, unit);
        if (Instance != null && Instance.isTestMod)
        {
            Debug.Log($"{unit.name}�� '{id}' ID�� ��ϵ�");
        }
    }

    /// <summary>
    /// ���� ��� ����
    /// </summary>
    /// <param name="unit"></param>
    public static void RemoveUnit(Unit unit)
    {
        UnitList.Remove(unit.UnitID);
        if (Instance != null && Instance.isTestMod)
        {
            Debug.Log($"{unit.name}�� ID ����� ������");
        }
    }

    public static Unit GetUnitByID(string unitID)
    {
        UnitList.TryGetValue(unitID, out Unit unit);
        if (unit == null)
        {
            Debug.LogError($"���� ���� ID ȣ�� �õ�: {unitID}");
        }
        return unit;
    }

    /// <summary>
    /// ������ֱ�
    /// </summary>
    /// <param name="target">�޴� ���</param>
    /// <param name="source">�ִ� ���</param>
    /// <param name="damage">����� Ŭ����</param>
    /// <returns>��������</returns>
    public bool DamageUnitToUnit(Unit target, Unit source, DamageArea damageArea)
    {
        if (target == null || source == null || damageArea == null)
        {
            return true;
        }
        target.Damage(damageArea.damage);
        return true;
        //�ӽ�
    }

    public bool DamageUnitToUnit(Unit target, Unit source, float damage)
    {
        if (target == null || source == null)
        {
            return true;
        }
        target.Damage(damage);
        return true;
        //�ӽ�
    }

    public bool DamageUnitToHitbox(HitBox target, Unit source, DamageArea damageArea)
    {
        if (target == null || source == null || damageArea == null)
        {
            return true;
        }

        target.Damage(damageArea.damage);
        //Debug.Log($"���� ����:{source.name}�� {target.Unit.name}����, {damageArea.damage} ����");

        return true;
        //�ӽ�
    }
}
