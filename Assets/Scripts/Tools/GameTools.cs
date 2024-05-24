using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTools
{
    #region 거리 비교
    /// <summary>
    /// 두 개 지점 사이가 주어진 거리보다 가까운지 체크
    /// </summary>
    /// <param name="start">시작 좌표</param>
    /// <param name="end">끝 좌표</param>
    /// <param name="distance">체크할 거리</param>
    /// <returns>주어진 거리보다 가까운가?</returns>
    public static bool IsInDistance(Vector2 start, Vector2 end, float distance)
    {
        PerformanceManager.StartTimer("GameTools.IsInDistance");
        Vector2 distanceVector = end - start;
        float sqrVector = distanceVector.sqrMagnitude;
        float sqrDistance = distance * distance;
        PerformanceManager.StopTimer("GameTools.IsInDistance");
        return sqrDistance > sqrVector;
    }
    #endregion

    #region 리스트 비교
    /// <summary>
    /// 주어진 두 리스트의 내용이 같은지 체크
    /// </summary>
    /// <typeparam name="T">타입</typeparam>
    /// <returns>같은가?</returns>
    public static bool CompareList<T>(List<T> listA, List<T> listB) where T : class
    {
        if (listA.Count != listB.Count)
        {
            return false;
        }

        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i] != listB[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareEnumList<T>(List<T> listA, List<T> listB) where T : Enum
    {
        if (listA.Count != listB.Count)
        {
            return false;
        }

        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i].Equals(listB[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareList<T>(List<T> list, T[] array) where T : class
    {
        if (list.Count != array.Length)
        {
            return false;
        }

        T[] temp = list.ToArray();

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != array[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareEnumList<T>(List<T> list, T[] array) where T : Enum
    {
        if (list.Count != array.Length)
        {
            return false;
        }

        T[] temp = list.ToArray();

        for (int i = 0; i < temp.Length; i++)
        {
            if (!temp[i].Equals(array[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareList<T>(T[] arrayA, T[] arrayB) where T : class
    {
        if (arrayA.Length != arrayB.Length)
        {
            return false;
        }

        for (int i = 0; i < arrayA.Length; i++)
        {
            if (arrayA[i] != arrayB[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareEnumList<T>(T[] arrayA, T[] arrayB) where T : Enum
    {
        if (arrayA.Length != arrayB.Length)
        {
            return false;
        }

        for (int i = 0; i < arrayA.Length; i++)
        {
            if (!arrayA[i].Equals(arrayB[i]))
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region 그래프

    /// <summary>
    /// 특정 지점을 기점으로 꺾이는 그래프
    /// </summary>
    /// <param name="graph">key 순서대로 정렬된 그래프</param>
    /// <param name="delta">값</param>
    /// <returns></returns>
    public static float GetNonlinearGraph(Dictionary<float, float> graph, float delta)
    {
        //예외처리
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: 잘못된 그래프 형식");
            return 0f;
        }

        //배열 정렬 생략(자주 호출해야 하므로 성능상 문제 예상). 정렬해서 넣어주기

        //결과 계산
        var tempArr = graph.Keys.ToArray();//graph의 키 배열

        if (graph.Count == 1 || delta <= tempArr[0])//최소치보다 낮으면 or 1개면
        {
            return graph[tempArr[0]];//최소치 반환
        }
        else if (delta >= tempArr[tempArr.Length - 1])//최대치보다 높으면
        {
            return graph[tempArr[tempArr.Length - 1]];//최대치 반환
        }
        else//그 사이면
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                float aValue = graph[aKey];
                float bKey = tempArr[i + 1];
                float bValue = graph[bKey];

                if (aKey < delta && delta <= bKey)
                {
                    float rate = Mathf.InverseLerp(aKey, bKey, delta);
                    Debug.Log($"aK:{aKey}, aV:{aValue}, bK:{bKey}, bV:{bValue}, del:{delta}, rate:{rate}");
                    return Mathf.Lerp(aValue, bValue, rate);
                }
            }
        }

        return 0f;
    }

    public static float GetNonlinearGraph(Dictionary<float, Func<float, float>> graph, float delta)
    {
        //예외처리
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: 잘못된 그래프 형식");
            return 0f;
        }

        //결과 계산
        var tempArr = graph.Keys.ToArray();//graph의 키 배열

        if (graph.Count == 1 || delta <= tempArr[0])//최소치보다 낮으면 or 1개면
        {
            return graph[tempArr[0]](delta);//최소치 반환
        }
        else if (delta >= tempArr[tempArr.Length - 1])//최대치보다 높으면
        {
            return graph[tempArr[tempArr.Length - 1]](delta);//최대치 반환
        }
        else//그 사이면
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                float aValue = graph[aKey](delta);
                float bKey = tempArr[i + 1];
                float bValue = graph[bKey](delta);
                //보간 체크해보고 수정할 것
                if (aKey < delta && delta <= bKey)
                {
                    float rate = Mathf.InverseLerp(aKey, bKey, delta);
                    Debug.Log($"aK:{aKey}, aV:{aValue}, bK:{bKey}, bV:{bValue}, del:{delta}, rate:{rate}");
                    return Mathf.Lerp(aValue, bValue, rate);
                }
            }
        }

        return 0f;
    }

    /// <summary>
    /// 특정 지점을 기점으로 꺾이는 그래프
    /// </summary>
    /// <param name="graph">key 순서대로 정렬된 그래프</param>
    /// <param name="delta">값</param>
    /// <returns></returns>
    public static Vector2 GetNonlinearGraph(Dictionary<float, Vector2> graph, float delta)
    {
        //예외처리
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: 잘못된 그래프 형식");
            return Vector2.zero;
        }


        //결과 계산
        var tempArr = graph.Keys.ToArray();//graph의 키 배열

        if (graph.Count == 1 || delta <= tempArr[0])//최소치보다 낮으면 or 1개면
        {
            return graph[tempArr[0]];//최소치 반환
        }
        else if (delta >= tempArr[tempArr.Length - 1])//최대치보다 높으면
        {
            return graph[tempArr[tempArr.Length - 1]];//최대치 반환
        }
        else//그 사이면
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                Vector2 aValue = graph[aKey];
                float bKey = tempArr[i + 1];
                Vector2 bValue = graph[bKey];

                if (aKey < delta && delta <= bKey)
                {
                    float rate = Mathf.InverseLerp(aKey, bKey, delta);
                    Debug.Log($"aK:{aKey}, aV:{aValue}, bK:{bKey}, bV:{bValue}, del:{delta}, rate:{rate}");
                    return Vector2.Lerp(aValue, bValue, rate);
                }
            }
        }

        return Vector2.zero;
    }

    #endregion
}
