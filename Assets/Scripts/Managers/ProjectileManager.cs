using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //�̱���
    private static ProjectileManager instance;
    public static ProjectileManager Instance => instance;

    private List<Projectile> projectiles;

    private void Awake()
    {
        //�̱���
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        projectiles = new();
    }

    /// <summary>
    /// ����ü ���
    /// </summary>
    /// <param name="projectile">����� ����ü</param>
    public static void Enroll(Projectile projectile)
    {
        if (!instance.projectiles.Contains(projectile))
        {
            instance.projectiles.Add(projectile);
        }
    }

    /// <summary>
    /// ����ü ��� ����
    /// </summary>
    /// <param name="projectile">��� ������ ����ü</param>
    public static void Remove(Projectile projectile)
    {
        if (instance.projectiles.Contains(projectile))
        {
            instance.projectiles.Remove(projectile);
        }
    }

    /// <summary>
    /// ���ٽ� ������ �����ϴ� ����ü ã��
    /// </summary>
    /// <param name="func">ã�� ����</param>
    /// <returns>������ �����ϴ� ��� ����ü</returns>
    public static List<Projectile> FindByFunc(Func<Projectile, bool> func, bool isPlayers = false)
    {
        List<Projectile> result = new();

        foreach (Projectile p in instance.projectiles)
        {
            if (isPlayers != (p.AttackUnit==GameManager.Player))
            {
                continue;
            }
            if (func(p))
            {
                result.Add(p);
            }
        }

        return result;
    }

    /// <summary>
    /// ��� ��ġ���� ���� �Ÿ� ���� �ִ� źȯ ã��
    /// </summary>
    /// <param name="distance">ã�� �Ÿ�</param>
    /// <param name="origin">ã�� ����</param>
    /// <returns>ã�� źȯ</returns>
    public static List<Projectile> FindByDistance(Vector2 origin, float distance, bool isPlayers = false)
    {
        //�ֺ��� �ִ� ��� ź ��������
        return FindByFunc((Projectile) =>
        {
            return GameTools.IsAround(origin, Projectile.transform.position, distance);
        },
        isPlayers);
    }

    /// <summary>
    /// ��� ��ġ���� Ư�� ��ä�� ���� �ִ� źȯ ã��
    /// </summary>
    /// <param name="basePos">��ä�� ������</param>
    /// <param name="angle">��ä�� �߾� ����</param>
    /// <param name="angleSize">��ä�� �� ����</param>
    /// <param name="distance">��ä�� ������</param>
    /// <param name="isPlayers">�÷��̾� źȯ�� ã�� ���ΰ�?</param>
    /// <returns>ã�� źȯ</returns>
    public static List<Projectile> FindInCorn(Vector2 basePos, float angle, float angleSize, float distance, bool isPlayers = false)
    {
        return FindByFunc((Projectile) =>
        {
            return GameTools.IsInCorn(Projectile.transform.position, basePos, angle, angleSize, distance);
        },
            isPlayers);
    }
}
