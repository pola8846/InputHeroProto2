using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //싱글톤
    private static ProjectileManager instance;
    public static ProjectileManager Instance => instance;

    private List<Projectile> projectiles;

    private void Awake()
    {
        //싱글톤
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
    /// 투사체 등록
    /// </summary>
    /// <param name="projectile">등록할 투사체</param>
    public static void Enroll(Projectile projectile)
    {
        if (!instance.projectiles.Contains(projectile))
        {
            instance.projectiles.Add(projectile);
        }
    }

    /// <summary>
    /// 투사체 등록 해제
    /// </summary>
    /// <param name="projectile">등록 해제할 투사체</param>
    public static void Remove(Projectile projectile)
    {
        if (instance.projectiles.Contains(projectile))
        {
            instance.projectiles.Remove(projectile);
        }
    }

    /// <summary>
    /// 람다식 조건을 만족하는 투사체 찾기
    /// </summary>
    /// <param name="func">찾을 조건</param>
    /// <returns>조건을 만족하는 모든 투사체</returns>
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
    /// 대상 위치에서 일정 거리 내에 있는 가장 가까운 탄환 찾기
    /// </summary>
    /// <param name="distance">찾을 거리</param>
    /// <param name="origin">찾을 원점</param>
    /// <returns>찾은 탄환, 없다면 null</returns>
    public static Projectile FindByDistance(Vector2 origin, float distance, bool isPlayers = false)
    {
        //주변에 있는 모든 탄 가져오기
        List<Projectile> list = FindByFunc((Projectile) =>
        {
            return GameTools.IsAround(origin, Projectile.transform.position, distance);
        }, 
        isPlayers);

        if (list.Count > 0)
        {
            //찾은 모든 탄 중 가장 가까운 것 찾기
            Projectile closest = list[0];
            float sqrDist = ((Vector2)closest.transform.position - origin).sqrMagnitude;

            for (int i = 1; i < list.Count; i++)
            {
                float temp = ((Vector2)list[i].transform.position - origin).sqrMagnitude;
                if (temp < sqrDist)
                {
                    closest = list[i];
                    sqrDist = temp;
                }
            }

            return closest;
        }
        else
        {
            //찾은 탄 없다면 null 반환
            return null;
        }
    }
}
