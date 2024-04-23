using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Capsule : HitBox
{
    private CapsuleCollider2D capsuleCollider;

    void OnDrawGizmos()
    {
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        GizmoDrawer.DrawCapsule(capsuleCollider, transform);
    }
}
