using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 충돌 중인 콜라이더 캐싱 및 검색 대행
/// </summary>
public class CollisionChecker : MonoBehaviour
{
    private List<Collider2D> enteredColliders = new();//접촉 중인 콜라이더 리스트
    private bool isCached_enteredColliders = false;//캐시되었는가?
    private List<Collider2D> tempList = new();//캐시용 임시 리스트
    public List<Collider2D> EnteredColliders//캐시된 콜라이더 리스트
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


    //접촉 시 콜라이더 등록
    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enteredColliders.Contains(collision))
        {
            enteredColliders.Add(collision);
        }
        isCached_enteredColliders = false;
    }

    //접촉 종료 시 콜라이더 등록 해제
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
    /// 특정 리스트 중에서 해당 콜라이더에 충돌한 개체만 가져오기
    /// </summary>
    /// <param name="checkers">찾을 후보 목록</param>
    /// <returns>찾은 리스트</returns>
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
    /// 특정 리스트 중에서 해당 콜라이더에 충돌한 특정 클래스만 가져오기
    /// </summary>
    /// <typeparam name="T">찾을 클래스</typeparam>
    /// <param name="checkers">찾을 후보 목록</param>
    /// <returns>찾은 리스트</returns>
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