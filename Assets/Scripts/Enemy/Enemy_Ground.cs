using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ground : Enemy
{
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (FindPlayer())
        {
            var player = GameManager.Player;
            bool isRight = player.transform.position.x >= transform.position.x;//플레이어가 오른쪽에 있는가?
            float movementX = Mathf.Max(0, Speed) * (isRight ? 1 : -1);

            rb.velocity = new Vector2(movementX, rb.velocity.y);

        }
    }
}
