using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTools
{
    #region 위치 비교
    /// <summary>
    /// 대상 위치가 특정 지점 주변 일정 거리 내에 있는가?
    /// </summary>
    /// <param name="targetPos">대상 위치</param>
    /// <param name="basePos">찾을 위치</param>
    /// <param name="distance">찾을 거리</param>
    /// <returns>대상 위치가 특정 지점 주변 일정 거리 내에 있는가?</returns>
    public static bool IsAround(Vector2 targetPos, Vector2 basePos, float distance)
    {
        // 두 점 사이의 거리의 제곱을 계산
        float squaredDistance = (targetPos - basePos).sqrMagnitude;

        // 주어진 거리의 제곱과 비교하여, 해당 거리 내에 있는지 여부를 반환
        return squaredDistance <= distance * distance;
    }

    /// <summary>
    /// 대상 위치가 특정 지점에서부터의 원뿔 형태의 범위 내에 있는가?
    /// </summary>
    /// <param name="targetPos">대상 위치</param>
    /// <param name="basePos">찾을 원뿔 범위의 시작점</param>
    /// <param name="angle">각도 기준점(Vector2D.up을 기준, 시계 방향으로 degree 각도)</param>
    /// <param name="angleSize">각도 크기(양 옆으로 절반의 degree각도)</param>
    /// <param name="distance">찾을 거리(원뿔 길이)</param>
    /// <returns>대상 위치가 특정 지점에서부터의 원뿔 형태의 범위 내에 있는가?</returns>
    public static bool IsInCorn(Vector2 targetPos, Vector2 basePos, float angle, float angleSize, float distance)
    {
        if (!IsAround(targetPos, basePos, distance)) { return false; }//거리 밖에 있으면 false


        // 기준 방향 벡터 계산: Vector2.up을 주어진 각도만큼 회전시킨다.
        float angleInRadians = angle * Mathf.Deg2Rad;
        Vector2 coneDirection = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));

        // 대상 벡터와 기준 벡터 간의 각도 계산
        float halfAngleSizeInRadians = angleSize * 0.5f * Mathf.Deg2Rad;
        float cosHalfAngleSize = Mathf.Cos(halfAngleSizeInRadians);

        // 내적을 사용하여 각도 비교
        Vector2 directionToTarget = targetPos - basePos;
        float cosAngleToTarget = Vector2.Dot(directionToTarget.normalized, coneDirection);

        // 대상 위치가 원뿔 형태의 각도 범위 내에 있는지 확인
        return cosAngleToTarget >= cosHalfAngleSize;
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
