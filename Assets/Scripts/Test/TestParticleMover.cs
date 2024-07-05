using UnityEngine;

public class TestParticleMover : MonoBehaviour
{
    public GameObject particle;
    public float rng = 30f;

    private void Update()
    {
        Vector2 shootDir = transform.up;
        int layer = (1 << LayerMask.NameToLayer("HitBox")) | (1 << LayerMask.NameToLayer("Bullet")) | (1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D ray = Physics2D.Raycast(transform.position, shootDir, 50, layer);
        Debug.DrawRay(transform.position, ray.point - (Vector2)transform.position, Color.yellow);
        if (ray.collider)
        {
            particle.SetActive(true);

            particle.transform.position = (Vector3)ray.point + (transform.position - (Vector3)ray.point).normalized * 0.2f;

            Vector2 reflectionDirection = Vector2.Reflect(shootDir, ray.normal);
            //Debug.Log($"reflectionDirection: {reflectionDirection}, shootDir: {shootDir}, ray.normal: {ray.normal}");

            Debug.DrawRay(ray.point, ray.normal, Color.white);
            Debug.DrawRay(ray.point, reflectionDirection, Color.red);

            particle.transform.LookAt(ray.point+ reflectionDirection);
        }
        else
        {
            particle.SetActive(false);
        }

    }
}
