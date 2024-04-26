using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawer
{
    private static Color gizmoColor = Color.green;

    public static void DrawBox(BoxCollider2D Collider, Transform transform, Color color = new())
    {
        if (color == new Color())
        {
            Gizmos.color = gizmoColor;
        }
        else
        {
            gizmoColor = color;
        }

        // BoxCollider2D의 경계를 계산합니다.
        Vector2 center = transform.TransformPoint(Collider.offset);
        Vector2 size = Collider.size * transform.lossyScale;

        // BoxCollider2D의 네 점을 계산합니다.
        Vector2 topLeft = center + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = center + new Vector2(size.x / 2, size.y / 2);
        Vector2 bottomLeft = center + new Vector2(-size.x / 2, -size.y / 2);
        Vector2 bottomRight = center + new Vector2(size.x / 2, -size.y / 2);

        // 기즈모를 그립니다.
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
    public static void DrawCapsule(CapsuleCollider2D Collider, Transform transform, Color color = new())
    {
        if (color == new Color())
        {
            Gizmos.color = gizmoColor;
        }
        else
        {
            gizmoColor = color;
        }

        // Capsule Collider 2D의 오프셋을 적용하고, 전역 좌표계로 변환합니다.
        Vector2 capsuleCenter = (Vector2)transform.position + Collider.offset;

        // Capsule Collider 2D의 크기를 고려하여 반지름 및 길이를 계산합니다.
        float radius = Collider.size.x / 2 * Mathf.Abs(transform.lossyScale.x);
        float length = Collider.size.y * Mathf.Abs(transform.lossyScale.y);

        // 회전을 고려하여 Capsule Collider 2D의 두 점을 계산합니다.
        Vector2 pointA = capsuleCenter + (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * length / 2);
        pointA -= (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * radius);
        Vector2 pointB = capsuleCenter - (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * length / 2);
        pointB += (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * radius);

        // 기즈모를 그립니다.
        Gizmos.DrawWireSphere(pointA, radius);
        Gizmos.DrawWireSphere(pointB, radius);
        Gizmos.DrawLine(pointA + Vector2.right * radius, pointB + Vector2.right * radius);
        Gizmos.DrawLine(pointA - Vector2.right * radius, pointB - Vector2.right * radius);
    }

    public static void DrawCircle(CircleCollider2D Collider, Transform transform, Color color = new())
    {
        if (color == new Color())
        {
            Gizmos.color = gizmoColor;
        }
        else
        {
            gizmoColor = color;
        }

        // Collider 2D의 오프셋을 적용하고, 전역 좌표계로 변환합니다.
        Vector2 Center = (Vector2)transform.position + Collider.offset;
        float radius = Collider.radius * Mathf.Abs(transform.lossyScale.x);

        // 기즈모를 그립니다.
        Gizmos.DrawWireSphere(Center, radius);
    }
}
