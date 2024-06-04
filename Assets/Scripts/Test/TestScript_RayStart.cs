using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_RayStart : MonoBehaviour
{
    private Vector2 previousPosition;
    private Vector2 currentPosition => transform.position;



    private void Start()
    {
        previousPosition = transform.position;
        
    }
    private void FixedUpdate()
    {
        Vector2 moveDist = currentPosition-previousPosition;

        var ray = Physics2D.RaycastAll(previousPosition, moveDist.normalized, moveDist.magnitude, 1 << LayerMask.NameToLayer("HitBox"));
        Debug.DrawRay(previousPosition, moveDist);
        

        if (ray != null && ray.Length >= 1)
        {
            Transform target = null;

            foreach (var hit in ray)
            {
                if (hit.transform == target || hit.transform.GetComponent<IBulletHitChecker>() == null)
                {
                    continue;
                }

                if (target == null || 
                    ((previousPosition - (Vector2)target.position).sqrMagnitude < (previousPosition - (Vector2)hit.transform.position).sqrMagnitude))
                {
                    target = hit.transform;
                }
            }

            if (target != null)
            {
                Debug.Log(target.gameObject.name);
                Destroy(gameObject);
                return;
            }
        }

        previousPosition = currentPosition;
    }

}
