using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletSet : MonoBehaviour
{
    private Unit Attacker;
    private Attack attack;
    private bool isPlayers;
    void Start()
    {
        Attacker = GetComponentInParent<Unit>();
        attack = GetComponent<Attack>();
        attack.Initialization(Attacker, (isPlayers ? "Enemy" : "Player"), gameObject);
    }
}
