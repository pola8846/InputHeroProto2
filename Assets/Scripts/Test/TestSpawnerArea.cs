using UnityEngine;

public class TestSpawnerArea : CollisionChecker
{
    public TestSpawner TestSpawner;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (TestSpawner != null && collision.GetComponent<PlayerUnit>() is not null)
        {
            TestSpawner.Spawn();
        }
    }

}
