using UnityEngine;

public class GizmoDrawer
{
    private static Color gizmoColor = Color.green;

    //�簢�� ����� �׸���
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

        // BoxCollider2D�� ��踦 ���
        Vector2 center = transform.TransformPoint(Collider.offset);
        Vector2 size = Collider.size * transform.lossyScale;

        // BoxCollider2D�� �� ���� ���
        Vector2 topLeft = center + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = center + new Vector2(size.x / 2, size.y / 2);
        Vector2 bottomLeft = center + new Vector2(-size.x / 2, -size.y / 2);
        Vector2 bottomRight = center + new Vector2(size.x / 2, -size.y / 2);

        // ����� �׸���
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    //ĸ���� ����� �׸���
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

        // Capsule Collider 2D�� �������� �����ϰ�, ���� ��ǥ��� ��ȯ
        Vector2 capsuleCenter = (Vector2)transform.position + Collider.offset;

        // Capsule Collider 2D�� ũ�⸦ ����Ͽ� ������ �� ���̸� ���
        float radius = Collider.size.x / 2 * Mathf.Abs(transform.lossyScale.x);
        float length = Collider.size.y * Mathf.Abs(transform.lossyScale.y);

        // ȸ���� ����Ͽ� Capsule Collider 2D�� �� ���� ���
        Vector2 pointA = capsuleCenter + (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * length / 2);
        pointA -= (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * radius);
        Vector2 pointB = capsuleCenter - (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * length / 2);
        pointB += (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.up * radius);

        // ����� �׸���
        Gizmos.DrawWireSphere(pointA, radius);
        Gizmos.DrawWireSphere(pointB, radius);
        Gizmos.DrawLine(pointA + Vector2.right * radius, pointB + Vector2.right * radius);
        Gizmos.DrawLine(pointA - Vector2.right * radius, pointB - Vector2.right * radius);
    }

    //���� ����� �׸���
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

        // Collider 2D�� �������� �����ϰ�, ���� ��ǥ��� ��ȯ
        Vector2 Center = (Vector2)transform.position + Collider.offset;
        float radius = Collider.radius * Mathf.Abs(transform.lossyScale.x);

        // ����� �׸���
        Gizmos.DrawWireSphere(Center, radius);
    }
}
