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
                hitBox.Damage(damage);
                return true;
            }

            //���̸�
            if (target.CompareTag("Blocker"))
            {
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
