using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxGizmo : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField]
    private Color color = Color.green;
    private void OnDrawGizmos()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        //GizmoDrawer.gizmoColor = color;
        GizmoDrawer.DrawBox(boxCollider, transform);
    }
}
