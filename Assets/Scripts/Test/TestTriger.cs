using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTriger : MonoBehaviour
{
    public List<BulletShooter> bulletShooters = new List<BulletShooter>();
    public float delay = 0.1f;
    private void Start()
    {
        StartCoroutine(shoot());   
    }

    private IEnumerator shoot()
    {
        while (true)
        {
            for (int i = 0; i < bulletShooters.Count; i++)
            {
                if (bulletShooters[i]==null)
                {
                    continue;
                }
                bulletShooters[i].triger = true;
                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }
}
