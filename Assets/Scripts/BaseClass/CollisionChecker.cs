using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollisionChecker : MonoBehaviour
{
    private List<Collider2D> enteredColliders = new();
    private bool isCached_enteredColliders = false;
    private List<Collider2D> tempList = new();
    public List<Collider2D> EnteredColliders
    {
        get
        {
            PerformanceManager.StartTimer("CollisionChecker.EnteredColliders.get");

            if (isCached_enteredColliders)
            {
                PerformanceManager.StopTimer("CollisionChecker.EnteredColliders.get");
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
            PerformanceManager.StopTimer("CollisionChecker.EnteredColliders.get");
            return enteredColliders;
        }
    }



    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enteredColliders.Contains(collision))
        {
            enteredColliders.Add(collision);
        }
        isCached_enteredColliders = false;
    }

    virtual protected void OnTriggerExit2D(Collider2D collision)
    {
        if (enteredColliders.Contains(collision))
        {
            enteredColliders.Remove(collision);
        }
        isCached_enteredColliders = false;
    }

    /// <summary>
    /// 충돌 중인 해당 클래스만 가져오기
    /// </summary>
    /// <typeparam name="T">가져올 클래스</typeparam>
    /// <returns>찾은 클래스 리스트</returns>
    public List<T> GetListOfClass<T>()
    {
        PerformanceManager.StartTimer("CollisionChecker.GetListOfClass");

        List<T> result = new();
        foreach (var item in EnteredColliders)
        {
            T temp = item.GetComponent<T>();
            if (temp is not null)
            {
                result.Add(temp);
            }
        }

        PerformanceManager.StopTimer("CollisionChecker.GetListOfClass");
        return result;
    }

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