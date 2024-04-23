using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxGizmo : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private void OnDrawGizmos()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        GizmoDrawer.DrawBox(boxCollider, transform);
    }
}
