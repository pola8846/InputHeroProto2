using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTools
{

    #region ��ġ ��
    /// <summary>
    /// ��� ��ġ�� Ư�� ���� �ֺ� ���� �Ÿ� ���� �ִ°�?
    /// </summary>
    /// <param name="targetPos">��� ��ġ</param>
    /// <param name="basePos">ã�� ��ġ</param>
    /// <param name="distance">ã�� �Ÿ�</param>
    /// <returns>��� ��ġ�� Ư�� ���� �ֺ� ���� �Ÿ� ���� �ִ°�?</returns>
    public static bool IsAround(Vector2 targetPos, Vector2 basePos, float distance)
    {
        // �� �� ������ �Ÿ��� ������ ���
        float squaredDistance = (targetPos - basePos).sqrMagnitude;

        // �־��� �Ÿ��� ������ ���Ͽ�, �ش� �Ÿ� ���� �ִ��� ���θ� ��ȯ
        return squaredDistance <= distance * distance;
    }

    /// <summary>
    /// ��� ��ġ�� Ư�� �������������� ���� ������ ���� ���� �ִ°�?
    /// </summary>
    /// <param name="targetPos">��� ��ġ</param>
    /// <param name="basePos">ã�� ���� ������ ������</param>
    /// <param name="angle">���� ������(Vector2D.up�� ����, �ð� �������� degree ����)</param>
    /// <param name="angleSize">���� ũ��(�� ������ ������ degree����)</param>
    /// <param name="distance">ã�� �Ÿ�(���� ����)</param>
    /// <returns>��� ��ġ�� Ư�� �������������� ���� ������ ���� ���� �ִ°�?</returns>
    public static bool IsInCorn(Vector2 targetPos, Vector2 basePos, float angle, float angleSize, float distance)
    {
        if (!IsAround(targetPos, basePos, distance)) { return false; }//�Ÿ� �ۿ� ������ false


        // ���� ���� ���� ���: Vector2.up�� �־��� ������ŭ ȸ����Ų��.
        float angleInRadians = angle * Mathf.Deg2Rad;
        Vector2 coneDirection = new(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));

        // ��� ���Ϳ� ���� ���� ���� ���� ���
        float halfAngleSizeInRadians = angleSize * 0.5f * Mathf.Deg2Rad;
        float cosHalfAngleSize = Mathf.Cos(halfAngleSizeInRadians);

        // ������ ����Ͽ� ���� ��
        Vector2 directionToTarget = targetPos - basePos;
        float cosAngleToTarget = Vector2.Dot(directionToTarget.normalized, coneDirection);

        // ��� ��ġ�� ���� ������ ���� ���� ���� �ִ��� Ȯ��
        return cosAngleToTarget >= cosHalfAngleSize;
    }

    /// <summary>
    /// �־��� ���� �� ���͸� �մ� ������ �߽����� �� ������ �ȿ� �ִ��� �˻�
    /// </summary>
    /// <param name="point">�˻��� ��</param>
    /// <param name="a">������ �� �� ��</param>
    /// <param name="b">������ �ݴ� ��</param>
    /// <param name="thickness">�������� �β�</param>
    /// <returns>���� ������ �ȿ� �ִ��� ����</returns>
    public static bool IsPointInRhombus(Vector2 point, Vector2 a, Vector2 b, float thickness)
    {
        // ���� AB�� ���� ���� �� ���� ���
        Vector2 direction = b - a;

        // ���� ���͸� ����ȭ
        direction.Normalize();

        // �������� ������ ��� (���� ������ ����)
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * thickness;

        // �������� �� ������ ���
        Vector2 p1 = a + perpendicular;
        Vector2 p2 = a - perpendicular;
        Vector2 p3 = b - perpendicular;
        Vector2 p4 = b + perpendicular;

        // ���� ������ ���ο� �ִ��� �˻� (�� ���� �������� ũ�ν� ���δ�Ʈ �˻�)
        if (IsPointInSquare(point, p1, p2, p3, p4))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���� �簢�� ���ο� ���� �ִ��� �˻��ϴ� �Լ�. ���� �簢���� �� �۵����� ����
    /// </summary>
    /// <param name="p">�˻��� ��</param>
    /// <param name="v1">�簢���� ù ��° ������</param>
    /// <param name="v2">�簢���� �� ��° ������</param>
    /// <param name="v3">�簢���� �� ��° ������</param>
    /// <param name="v4">�簢���� �� ��° ������</param>
    /// <returns>���� �簢�� ���ο� �ִ��� ����</returns>
    public static bool IsPointInSquare(Vector2 p, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
    {
        // ���� ����: �� ���� ���� ���� ��ġ�� �������� �� ����
        Vector2 d1 = v2 - v1; // ù ��° ��
        Vector2 d2 = v3 - v2; // �� ��° ��
        Vector2 d3 = v4 - v3; // �� ��° ��
        Vector2 d4 = v1 - v4; // �� ��° ��

        // �� ���� ���� ������ ���� ���
        float cross1 = CrossProduct(d1, p - v1);
        float cross2 = CrossProduct(d2, p - v2);
        float cross3 = CrossProduct(d3, p - v3);
        float cross4 = CrossProduct(d4, p - v4);

        // ��� ������ ����� ���� ��ȣ���� Ȯ��
        return (cross1 >= 0 && cross2 >= 0 && cross3 >= 0 && cross4 >= 0) ||
               (cross1 <= 0 && cross2 <= 0 && cross3 <= 0 && cross4 <= 0);
    }


    /// <summary>
    /// �־��� ���� �ﰢ�� �ȿ� �ִ��� �˻�
    /// </summary>
    /// <param name="point">�˻��� ��</param>
    /// <param name="a">�ﰢ���� ù ��° ��</param>
    /// <param name="b">�ﰢ���� �� ��° ��</param>
    /// <param name="c">�ﰢ���� �� ��° ��</param>
    /// <returns>���� �ﰢ�� �ȿ� �ִ��� ����</returns>
    private static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
        // ���� ���� ũ�ν� ���δ�Ʈ�� ����Ͽ� ���� �ﰢ�� ���ο� �ִ��� �˻�
        bool b1 = CrossProduct(b - a, point - a) < 0.0f;
        bool b2 = CrossProduct(c - b, point - b) < 0.0f;
        bool b3 = CrossProduct(a - c, point - c) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }

    /// <summary>
    /// 2D ������ ���� ���
    /// </summary>
    /// <param name="a">ù ��° ����</param>
    /// <param name="b">�� ��° ����</param>
    /// <returns>������ ����</returns>
    private static float CrossProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    /// <summary>
    /// ���� �����ų� �� ��ǥ ã�Ƽ� ��ȯ
    /// </summary>
    /// <param name="originPos">ã�� ���� ��ǥ</param>
    /// <param name="objects">ã�� ������Ʈ��</param>
    /// <param name="findFar">�� ���� ã�� ���ΰ�?</param>
    /// <returns>ã�� ������Ʈ</returns>
    public static T FindClosest<T>(Vector3 originPos, List<T> objects, bool findFar = false) where T : MonoBehaviour
    {
        if (objects == null)
        {
            return null;
        }

        if (objects.Count > 0)
        {
            //ã�� ��� �� �� ���� ����� �� ã��
            T target = objects[0];
            float sqrDist = (target.transform.position - originPos).sqrMagnitude;//target���� �Ÿ�

            //������ ��ο� ��
            for (int i = 1; i < objects.Count; i++)
            {
                float temp = (objects[i].transform.position - originPos).sqrMagnitude;//temp���� �Ÿ�
                if (findFar ? temp > sqrDist : temp < sqrDist)//�Ÿ� ��
                {
                    target = objects[i];
                    sqrDist = temp;
                }
            }

            return target;
        }
        else
        {
            //���ٸ� null ��ȯ
            //Debug.LogError("FindClosest");
            return null;
        }
    }

    /// <summary>
    /// �� Rect�� ��ġ�� �κ� ��ȯ, ������ zero ��ȯ
    /// </summary>
    /// <returns>��ġ�� ����, ������ zero</returns>
    public static Rect CalculateOverlapRect(Rect rect1, Rect rect2)
    {
        // ��ǥ ����
        float overlapLeft = Mathf.Max(rect1.xMin, rect2.xMin);
        float overlapBottom = Mathf.Max(rect1.yMin, rect2.yMin);
        float overlapRight = Mathf.Min(rect1.xMax, rect2.xMax);
        float overlapTop = Mathf.Min(rect1.yMax, rect2.yMax);

        // ��ġ�� �κ��� �ִ��� Ȯ��
        if (overlapLeft < overlapRight && overlapBottom < overlapTop)
        {
            // ��ġ�� ������ �簢���� ��ȯ
            return new Rect(overlapLeft, overlapBottom, overlapRight - overlapLeft, overlapTop - overlapBottom);
        }
        else
        {
            // ��ġ�� ������ ���� ��� �� Rect ��ȯ
            return Rect.zero;
        }
    }
    #endregion

    #region ����Ʈ ��
    /// <summary>
    /// �־��� �� ����Ʈ�� ������ ������ üũ
    /// </summary>
    /// <typeparam name="T">Ÿ��</typeparam>
    /// <returns>������?</returns>
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

    #region �׷���

    /// <summary>
    /// Ư�� ������ �������� ���̴� �׷���
    /// </summary>
    /// <param name="graph">key ������� ���ĵ� �׷���</param>
    /// <param name="delta">��</param>
    /// <returns></returns>
    public static float GetNonlinearGraph(Dictionary<float, float> graph, float delta)
    {
        //����ó��
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: �߸��� �׷��� ����");
            return 0f;
        }

        //�迭 ���� ����(���� ȣ���ؾ� �ϹǷ� ���ɻ� ���� ����). �����ؼ� �־��ֱ�

        //��� ���
        var tempArr = graph.Keys.ToArray();//graph�� Ű �迭

        if (graph.Count == 1 || delta <= tempArr[0])//�ּ�ġ���� ������ or 1����
        {
            return graph[tempArr[0]];//�ּ�ġ ��ȯ
        }
        else if (delta >= tempArr[tempArr.Length - 1])//�ִ�ġ���� ������
        {
            return graph[tempArr[tempArr.Length - 1]];//�ִ�ġ ��ȯ
        }
        else//�� ���̸�
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
        //����ó��
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: �߸��� �׷��� ����");
            return 0f;
        }

        //��� ���
        var tempArr = graph.Keys.ToArray();//graph�� Ű �迭

        if (graph.Count == 1 || delta <= tempArr[0])//�ּ�ġ���� ������ or 1����
        {
            return graph[tempArr[0]](delta);//�ּ�ġ ��ȯ
        }
        else if (delta >= tempArr[tempArr.Length - 1])//�ִ�ġ���� ������
        {
            return graph[tempArr[tempArr.Length - 1]](delta);//�ִ�ġ ��ȯ
        }
        else//�� ���̸�
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                float aValue = graph[aKey](delta);
                float bKey = tempArr[i + 1];
                float bValue = graph[bKey](delta);
                //���� üũ�غ��� ������ ��
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
    /// Ư�� ������ �������� ���̴� �׷���
    /// </summary>
    /// <param name="graph">key ������� ���ĵ� �׷���</param>
    /// <param name="delta">��</param>
    /// <returns></returns>
    public static Vector2 GetNonlinearGraph(Dictionary<float, Vector2> graph, float delta)
    {
        //����ó��
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: �߸��� �׷��� ����");
            return Vector2.zero;
        }


        //��� ���
        var tempArr = graph.Keys.ToArray();//graph�� Ű �迭

        if (graph.Count == 1 || delta <= tempArr[0])//�ּ�ġ���� ������ or 1����
        {
            return graph[tempArr[0]];//�ּ�ġ ��ȯ
        }
        else if (delta >= tempArr[tempArr.Length - 1])//�ִ�ġ���� ������
        {
            return graph[tempArr[tempArr.Length - 1]];//�ִ�ġ ��ȯ
        }
        else//�� ���̸�
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

    /// <summary>
    /// ����Ʈ���� Ư�� ������ �ش��ϴ� ���� ��ȯ�ϴ� ���� �׷���
    /// </summary>
    /// <typeparam name="T">ã�� �ڷ���</typeparam>
    /// <param name="graph">ã�� ������</param>
    /// <param name="delta">ã�� ��</param>
    /// <param name="min">Ž�� ���� �ִ밪</param>
    /// <param name="max">Ž�� ���� �ּҰ�</param>
    public static T GetlinearGraphInList<T>(List<T> graph, float delta, float min, float max)
    {
        if (delta <= min)
        {
            return graph[0];
        }
        if (delta >= max)
        {
            return graph[graph.Count - 1];
        }

        float rate = Mathf.InverseLerp(min, max, delta);
        int target = Mathf.RoundToInt(Mathf.Lerp(0, graph.Count - 1, rate));
        return graph[target];
    }

    public static int GetlinearGraphInCount(int Count, float delta, float min, float max)
    {
        if (delta <= min)
        {
            return 0;
        }
        if (delta >= max)
        {
            return Count - 1;
        }

        float rate = Mathf.InverseLerp(min, max, delta);
        int target = Mathf.RoundToInt(Mathf.Lerp(0, Count, rate));
        return target;
    }

    #endregion

    #region ��ȯ
    /// <summary>
    /// Ʈ�������� Rect�� ��ȯ
    /// </summary>
    /// <param name="transform">��ȯ�� Ʈ������</param>
    /// <returns>��ȯ�� Rect</returns>
    public static Rect TransformToRect(Transform transform)
    {
        // Transform�� ��ġ�� �������� ����Ͽ� Rect ����
        Vector2 position = transform.position;
        Vector2 size = transform.lossyScale;

        // Rect�� ��ġ�� �߽ɿ��� ���� ũ�⸦ ���� ���
        Vector2 bottomLeftCorner = position - size * 0.5f;
        Rect rect = new(bottomLeftCorner, size);

        return rect;
    }

    /// <summary>
    /// ���͸� Degree(��) ������ ��ȯ. Up ���� ������ ����, �������� ���(-180~180)
    /// </summary>
    /// <param name="dir">��ȯ�� ���� ����</param>
    /// <returns>��ȯ�� ����</returns>
    public static float GetDegreeAngleFormDirection(Vector2 dir)
    {
        return Vector2.SignedAngle(Vector2.up, dir.normalized);
    }

    /// <summary>
    /// Degree(��) ������ ���� ���ͷ� ��ȯ. Up ���� ������ ����, �������� ���(-180~180)
    /// </summary>
    /// <param name="angle">��ȯ�� ����</param>
    /// <returns>��ȯ�� ���� ����</returns>
    public static Vector2 GetDirectionFormDegreeAngle(float angle)
    {
        Quaternion quat = Quaternion.Euler(0, 0, angle);
        return quat * Vector2.up;
    }

    #endregion




    public static Rect GetCameraViewportSize()
    {
        // ī�޶��� ����Ʈ ��� ���
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z + GameManager.CameraZPos));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z + GameManager.CameraZPos));

        return new(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }
    public static Vector3 ClampToRect(Vector3 position, Rect rect, float extra = 0)
    {
        // x�� y ���� Rect�� ��� ���� Ŭ����
        float clampedX = rect.width > (extra * 2) ?
            Mathf.Clamp(position.x, rect.xMin + extra, rect.xMax - extra) :
            rect.xMin + (rect.width / 2);

        float clampedY = rect.height > (extra * 2) ?
            Mathf.Clamp(position.y, rect.yMin + extra, rect.yMax - extra) :
            rect.yMin + (rect.height / 2);

        // Vector3�� �����Ͽ� ��ȯ (z ���� ����)
        return new Vector3(clampedX, clampedY, position.z);
    }
}