using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �浹 ���� �ݶ��̴� ĳ�� �� �˻� ����
/// </summary>
public class CollisionChecker : MonoBehaviour
{
    private List<Collider2D> enteredColliders = new();//���� ���� �ݶ��̴� ����Ʈ
    private bool isCached_enteredColliders = false;//ĳ�õǾ��°�?
    private List<Collider2D> tempList = new();//ĳ�ÿ� �ӽ� ����Ʈ
    public List<Collider2D> EnteredColliders//ĳ�õ� �ݶ��̴� ����Ʈ
    {
        get
        {
            if (isCached_enteredColliders)
            {
                return enteredColliders;
            }

            tempList.Clear();
            foreach (var item in enteredColliders)
            {
                if (item != null)
                {
                    tempList.Add(item);
                }
            }
            enteredColliders = tempList.ToList();

            isCached_enteredColliders = true;
            return enteredColliders;
        }
    }


    //���� �� �ݶ��̴� ���
    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enteredColliders.Contains(collision))
        {
            enteredColliders.Add(collision);
        }
        isCached_enteredColliders = false;
    }

    //���� ���� �� �ݶ��̴� ��� ����
    virtual protected void OnTriggerExit2D(Collider2D collision)
    {
        if (enteredColliders.Contains(collision))
        {
            enteredColliders.Remove(collision);
        }
        isCached_enteredColliders = false;
    }

    /// <summary>
    /// �浹 ���� �ش� Ŭ������ ��������
    /// </summary>
    /// <typeparam name="T">������ Ŭ����</typeparam>
    /// <returns>ã�� Ŭ���� ����Ʈ</returns>
    public List<T> GetListOfClass<T>()
    {
        List<T> result = new();
        foreach (var item in EnteredColliders)
        {
            T temp = item.GetComponent<T>();
            if (temp is not null)
            {
                result.Add(temp);
            }
        }
        return result;
    }

    /// <summary>
    /// Ư�� ����Ʈ �߿��� �ش� �ݶ��̴��� �浹�� ��ü�� ��������
    /// </summary>
    /// <param name="checkers">ã�� �ĺ� ���</param>
    /// <returns>ã�� ����Ʈ</returns>
    public static List<Collider2D> GetItems(params CollisionChecker[] checkers)
    {
        List<Collider2D> result = new();

        foreach (var checker in checkers)
        {
            foreach (var collider in checker.EnteredColliders)
            {
                if (!result.Contains(collider))
                {
                    result.Add(collider);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Ư�� ����Ʈ �߿��� �ش� �ݶ��̴��� �浹�� Ư�� Ŭ������ ��������
    /// </summary>
    /// <typeparam name="T">ã�� Ŭ����</typeparam>
    /// <param name="checkers">ã�� �ĺ� ���</param>
    /// <returns>ã�� ����Ʈ</returns>
    public static List<T> GetListOfClassAtMulty<T>(params CollisionChecker[] checkers)
    {
        List<T> result = new();
        foreach (var checker in checkers)
        {
            foreach (var item in checker.GetListOfClass<T>())
            {
                if (!result.Contains(item))
                {
                    result.Add(item);
                }
            }
        }
        return result;
    }
}