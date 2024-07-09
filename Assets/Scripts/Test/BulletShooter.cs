using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject GO;

    [SerializeField]
    private int bulletNum = 1;
    public int BulletNum
    {
        get
        {
            return bulletNum;
        }
        set
        {
            bulletNum = Mathf.Max(1, value);
        }
    }

    public ShootType shootType;

    [SerializeField]
    private float bulletSpeedMin = 1;
    public float BulletSpeedMin
    {
        get
        {
            return bulletSpeedMin;
        }
        set
        {
            bulletSpeedMin = Mathf.Max(0, value);
        }
    }
    [SerializeField]
    private float bulletSpeedMax = 1;
    public float BulletSpeedMax
    {
        get
        {
            return bulletSpeedMax;
        }
        set
        {
            bulletSpeedMax = Mathf.Max(0, value);
        }
    }
    public float BulletSpeed
    {
        set
        {
            bulletSpeedMax = bulletSpeedMin = Mathf.Max(0, value);
        }
    }
    public float bulletAngleMin = 90;
    public float bulletAngleMax = 90;
    public float BulletAngle
    {
        set
        {
            SetBulletAngle(value, 0);
        }
    }

    public float lifeTime = 0;
    public float lifeDistance = 0;

    public Unit Unit;
    public bool isPlayers = false;

    public bool testTriger = false;

    private void Update()
    {
        if (testTriger)
        {
            Triger();
            testTriger = false;
        }
    }

    public void Triger()
    {
        Shoot();
    }

    private void Shoot()
    {
        PerformanceManager.StartTimer("BulletShooter.Shoot");
        switch (shootType)
        {
            case ShootType.oneWay:
                for (int i = 0; i < bulletNum; i++)
                {
                    Quaternion quat = Quaternion.Euler(0, 0, Random.Range(bulletAngleMin, bulletAngleMax));
                    Vector2 direction = quat * Vector2.up;
                    //direction.x *= -1;
                    MakeProjectile(GameTools.GetDirectionFormDegreeAngle(Random.Range(bulletAngleMin, bulletAngleMax)));
                }
                break;

            case ShootType.fan:
                {
                    Quaternion quat = Quaternion.Euler(0, 0, bulletAngleMin);
                    Quaternion quatAtOnce = Quaternion.Euler(0, 0, (bulletAngleMax - bulletAngleMin) / bulletNum);

                    for (int i = 0; i < bulletNum; i++)
                    {
                        Vector2 direction = quat * Vector2.up;
                        MakeProjectile(direction);
                        //direction.x *= -1;
                        quat *= quatAtOnce;
                    }
                }
                break;

            default:
                break;
        }

        PerformanceManager.StopTimer("BulletShooter.Shoot");
    }

    private void MakeProjectile(Vector2 direction)
    {
        Attack attack = Attack.MakeGameObject(Unit, (isPlayers ? "Enemy" : "Player"));
        Destroy(attack.gameObject, lifeTime<=0? 10f : lifeTime +1);
        
        GameObject go = Instantiate(GO, transform.position, transform.rotation, attack.transform);
        if (go.GetComponentsInChildren<DamageArea>().Length>=1)
        {
            attack.EnrollDamage(go);
        }
        else
        {
            attack.isDestroySelfAuto = false;
        }


        Projectile projectile = go.GetComponent<Projectile>();
        projectile?.Initialize(
            direction, Random.Range(bulletSpeedMin, bulletSpeedMax), Unit,
            lifeTime: lifeTime, lifeDistance: lifeDistance);
    }

    public void SetBulletAngle(float angle, float range = 0)
    {
        bulletAngleMax = angle + range;
        bulletAngleMin = angle - range;
    }
}

public enum ShootType
{
    oneWay,
    fan,
}