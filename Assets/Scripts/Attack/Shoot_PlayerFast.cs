using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_PlayerFast : Shoot
{
    protected override bool HitCheck(Collider2D target)
    {
        if (target != null)
        {
            // 충돌 처리

            //플레이어 히트박스면
            HitBox hitBox = target.GetComponent<HitBox>();
            if (hitBox != null && target.CompareTag("Enemy"))
            {
                hitBox.Damage(damage);
                //Hit(hitBox.Unit);
                return true;
            }

            //총알이면
            //if (Time.timeScale < 1)
            {
                var temp = hitBox.GetComponent<TestBulletChecker>();
                if (temp != null)
                {
                    Debug.Log("Hit");

                    hitBox.GetComponentInChildren<DamageArea>()?.Destroy();
                    return true;
                }
            }

            //기타 장애물이면
            if (true)
            {

            }

            // 충돌에 대한 처리 추가
        }
        return false;
    }
}
