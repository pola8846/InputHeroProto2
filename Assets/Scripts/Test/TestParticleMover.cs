using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TestParticleMover : MonoBehaviour
{
    public GameObject particle;
    public float rng = 30f;

    private void Update()
    {
        Vector2 shootDir = transform.up;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, shootDir, 50);
        Debug.DrawRay(transform.position, ray.point- (Vector2)transform.position, Color.yellow);
        if (ray.collider)
        {
            particle.SetActive(true);

            particle.transform.position = ray.point;

            Vector2 reflectionDirection = Vector2.Reflect(shootDir, ray.normal);
            Debug.Log($"reflectionDirection: {reflectionDirection}, shootDir: {shootDir}, ray.normal: {ray.normal}");

            Debug.DrawRay(ray.point, ray.normal, Color.white);
            Debug.DrawRay(ray.point, reflectionDirection, Color.red);

            particle.transform.up = reflectionDirection.normalized;
        }
        else
        {
            particle.SetActive(false);
        }

    }
}
