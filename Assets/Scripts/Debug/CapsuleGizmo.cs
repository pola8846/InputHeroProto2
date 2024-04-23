using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class CapsuleGizmo : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;

    void OnDrawGizmos()
    {
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        GizmoDrawer.DrawCapsule(capsuleCollider, transform);
    }
}
