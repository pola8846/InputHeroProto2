using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private List<Collider2D> enteredColliders = new();
    public List<Collider2D> EnteredColliders
    {
        get
        {
            PerformanceManager.StartTimer("CollisionChecker.EnteredColliders.get");
            List<Collider2D> temp = new();
            foreach (var item in enteredColliders)
            {
                if (item != null)
                {
                    temp.Add(item);
                }
            }
            enteredColliders = temp.ToList();
            PerformanceManager.StopTimer("CollisionChecker.EnteredColliders.get");
            return enteredColliders;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enteredColliders.Contains(collision))
        {
            enteredColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enteredColliders.Contains(collision))
        {
            enteredColliders.Remove(collision);
        }
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
}