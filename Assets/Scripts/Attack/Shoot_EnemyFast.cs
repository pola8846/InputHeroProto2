using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_EnemyFast : Shoot
{
    protected override bool HitCheck(Collider2D target)
    {
        if (target != null)
        {
            // �浹 ó��

            //�÷��̾� ��Ʈ�ڽ���
            HitBox hitBox = target.GetComponent<HitBox>();
            if (hitBox != null && target.CompareTag("Player"))
            {
                Debug.Log("Hit");
                hitBox.Damage(damage);
                //Hit(hitBox.Unit);
                return true;
            }

            //���̸�
            if (target.CompareTag("Blocker"))
            {
                Debug.Log("Hit");
                return true;
            }

            //��Ÿ ��ֹ��̸�
            if (true)
            {

            }

            // �浹�� ���� ó�� �߰�
        }
        return false;
    }
}
