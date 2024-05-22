using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject monster;
    public GameObject spawnPoint;


    public int spawnNum = 1;
    public int spawnCount = 0;

    public void Spawn()
    {
        if(spawnCount < spawnNum)
        {
            spawnCount++;
            GameObject mob = Instantiate(monster);
            mob.transform.position = spawnPoint.transform.position;
        }
    }
}
