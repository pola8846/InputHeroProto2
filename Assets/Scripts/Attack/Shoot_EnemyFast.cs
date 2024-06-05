using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_EnemyFast : Shoot
{
    protected override bool HitCheck(Transform target)
    {
        if (target != null)
        {
            // 충돌 처리
            Debug.Log("Hit");

            //플레이어 히트박스면
            if (target.GetComponent<HitBox>() != null && target.CompareTag("Player"))
            {
                Hit(target.GetComponent<HitBox>().Unit);
                return true;
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
