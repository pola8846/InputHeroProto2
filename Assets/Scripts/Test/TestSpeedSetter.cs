using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedSetter : MonoBehaviour
{
    [Range(0f, 2f)]
    public float speed;
    public ParticleSystem ParticleSystem;
    public bool check;
    private void Update()
    {
        Time.timeScale = speed;
    }
}
