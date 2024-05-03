using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTools
{
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
}