using System;
using System.Collections;
using System.Collections.Generic;
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
            if (temp[i].Equals(array[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareList<T>(T[] arrayA, T[] arrayB) where T : class
    {
        if (arrayA.Length!=arrayB.Length)
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
            if (arrayA[i].Equals(arrayB[i]))
            {
                return false;
            }
        }
        return true;
    }
    #endregion
}

public delegate void Act();