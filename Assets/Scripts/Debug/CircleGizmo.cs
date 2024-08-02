using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CircleGizmo : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    private void OnDrawGizmos()
    {
        if (circleCollider == null)
        {
            circleCollider = GetComponent<CircleCollider2D>();
        }
        GizmoDrawer.DrawCircle(circleCollider, transform);
    }
}
