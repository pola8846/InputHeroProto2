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

    /// <summary>
    /// 주어진 점이 두 벡터를 잇는 선분을 중심으로 한 마름모 안에 있는지 검사
    /// </summary>
    /// <param name="point">검사할 점</param>
    /// <param name="a">선분의 한 쪽 점</param>
    /// <param name="b">선분의 반대 점</param>
    /// <param name="thickness">마름모의 두께</param>
    /// <returns>점이 마름모 안에 있는지 여부</returns>
    public static bool IsPointInRhombus(Vector2 point, Vector2 a, Vector2 b, float thickness)
    {
        // 선분 AB의 방향 벡터 및 길이 계산
        Vector2 direction = b - a;

        // 방향 벡터를 정규화
        direction.Normalize();

        // 마름모의 꼭짓점 계산 (수직 방향의 벡터)
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * thickness;

        // 마름모의 네 꼭짓점 계산
        Vector2 p1 = a + perpendicular;
        Vector2 p2 = a - perpendicular;
        Vector2 p3 = b - perpendicular;
        Vector2 p4 = b + perpendicular;

        // 점이 마름모 내부에 있는지 검사 (각 변을 기준으로 크로스 프로덕트 검사)
        if (IsPointInSquare(point, p1, p2, p3, p4))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 볼록 사각형 내부에 점이 있는지 검사하는 함수. 오목 사각형일 땐 작동하지 않음
    /// </summary>
    /// <param name="p">검사할 점</param>
    /// <param name="v1">사각형의 첫 번째 꼭지점</param>
    /// <param name="v2">사각형의 두 번째 꼭지점</param>
    /// <param name="v3">사각형의 세 번째 꼭지점</param>
    /// <param name="v4">사각형의 네 번째 꼭지점</param>
    /// <returns>점이 사각형 내부에 있는지 여부</returns>
    public static bool IsPointInSquare(Vector2 p, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
    {
        // 벡터 생성: 각 변에 대한 점의 위치를 기준으로 한 벡터
        Vector2 d1 = v2 - v1; // 첫 번째 변
        Vector2 d2 = v3 - v2; // 두 번째 변
        Vector2 d3 = v4 - v3; // 세 번째 변
        Vector2 d4 = v1 - v4; // 네 번째 변

        // 각 변에 대한 점과의 외적 계산
        float cross1 = CrossProduct(d1, p - v1);
        float cross2 = CrossProduct(d2, p - v2);
        float cross3 = CrossProduct(d3, p - v3);
        float cross4 = CrossProduct(d4, p - v4);

        // 모든 외적의 결과가 같은 부호인지 확인
        return (cross1 >= 0 && cross2 >= 0 && cross3 >= 0 && cross4 >= 0) ||
               (cross1 <= 0 && cross2 <= 0 && cross3 <= 0 && cross4 <= 0);
    }


    /// <summary>
    /// 주어진 점이 삼각형 안에 있는지 검사
    /// </summary>
    /// <param name="point">검사할 점</param>
    /// <param name="a">삼각형의 첫 번째 점</param>
    /// <param name="b">삼각형의 두 번째 점</param>
    /// <param name="c">삼각형의 세 번째 점</param>
    /// <returns>점이 삼각형 안에 있는지 여부</returns>
    private static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
        // 벡터 간의 크로스 프로덕트를 사용하여 점이 삼각형 내부에 있는지 검사
        bool b1 = CrossProduct(b - a, point - a) < 0.0f;
        bool b2 = CrossProduct(c - b, point - b) < 0.0f;
        bool b3 = CrossProduct(a - c, point - c) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }

    /// <summary>
    /// 2D 벡터의 외적 계산
    /// </summary>
    /// <param name="a">첫 번째 벡터</param>
    /// <param name="b">두 번째 벡터</param>
    /// <returns>벡터의 외적</returns>
    private static float CrossProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    /// <summary>
    /// 가장 가깝거나 먼 좌표 찾아서 반환
    /// </summary>
    /// <param name="originPos">찾을 기준 좌표</param>
    /// <param name="objects">찾을 오브젝트들</param>
    /// <param name="findFar">먼 것을 찾을 것인가?</param>
    /// <returns>찾은 오브젝트</returns>
    public static T FindClosest<T>(Vector3 originPos, List<T> objects, bool findFar = false) where T : MonoBehaviour
    {
        if (objects == null)
        {
            return null;
        }

        if (objects.Count > 0)
        {
            //찾은 모든 것 중 가장 가까운 것 찾기
            T target = objects[0];
            float sqrDist = (target.transform.position - originPos).sqrMagnitude;//target과의 거리

            //나머지 모두와 비교
            for (int i = 1; i < objects.Count; i++)
            {
                float temp = (objects[i].transform.position - originPos).sqrMagnitude;//temp와의 거리
                if (findFar ? temp > sqrDist : temp < sqrDist)//거리 비교
                {
                    target = objects[i];
                    sqrDist = temp;
                }
            }

            return target;
        }
        else
        {
            //없다면 null 반환
            //Debug.LogError("FindClosest");
            return null;
        }
    }

    /// <summary>
    /// 두 Rect의 겹치는 부분 반환, 없으면 zero 반환
    /// </summary>
    /// <returns>겹치는 영역, 없으면 zero</returns>
    public static Rect CalculateOverlapRect(Rect rect1, Rect rect2)
    {
        // 좌표 정의
        float overlapLeft = Mathf.Max(rect1.xMin, rect2.xMin);
        float overlapBottom = Mathf.Max(rect1.yMin, rect2.yMin);
        float overlapRight = Mathf.Min(rect1.xMax, rect2.xMax);
        float overlapTop = Mathf.Min(rect1.yMax, rect2.yMax);

        // 겹치는 부분이 있는지 확인
        if (overlapLeft < overlapRight && overlapBottom < overlapTop)
        {
            // 겹치는 영역의 사각형을 반환
            return new Rect(overlapLeft, overlapBottom, overlapRight - overlapLeft, overlapTop - overlapBottom);
        }
        else
        {
            // 겹치는 영역이 없을 경우 빈 Rect 반환
            return Rect.zero;
        }
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

    /// <summary>
    /// 트랜스폼을 Rect로 변환
    /// </summary>
    /// <param name="transform">변환할 트랜스폼</param>
    /// <returns>변환된 Rect</returns>
    public static Rect TransformToRect(Transform transform)
    {
        // Transform의 위치와 스케일을 사용하여 Rect 생성
        Vector2 position = transform.position;
        Vector2 size = transform.lossyScale;

        // Rect의 위치는 중심에서 절반 크기를 빼서 계산
        Vector2 bottomLeftCorner = position - size * 0.5f;
        Rect rect = new Rect(bottomLeftCorner, size);

        return rect;
    }

    public static Rect GetCameraViewportSize()
    {
        // 카메라의 뷰포트 경계 계산
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));

        return new(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }
    public static Vector3 ClampToRect(Vector3 position, Rect rect, float extra = 0)
    {
        // x와 y 값을 Rect의 경계 내로 클램핑
        float clampedX = rect.width > (extra * 2) ?
            Mathf.Clamp(position.x, rect.xMin + extra, rect.xMax - extra) :
            rect.xMin + (rect.width / 2);

        float clampedY = rect.height > (extra * 2) ?
            Mathf.Clamp(position.y, rect.yMin + extra, rect.yMax - extra) :
            rect.yMin + (rect.height / 2);

        // Vector3를 생성하여 반환 (z 값은 유지)
        return new Vector3(clampedX, clampedY, position.z);
    }
}