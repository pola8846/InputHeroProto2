using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject GO;//발사할 프리팹

    [SerializeField]
    private int bulletNum = 1;//발사할 총알 수
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

    public ShootType shootType;//발사 타입

    [SerializeField]
    private float bulletSpeedMin = 1;//총알 최소 속도
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
    private float bulletSpeedMax = 1;//총알 최대 속도
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

    public float bulletAngleMin = 90;//총알 최소 각도
    public float bulletAngleMax = 90;//총알 최대 각도
    public float BulletAngle
    {
        set
        {
            SetBulletAngle(value, 0);
        }
    }

    public float lifeTime = 0;//총알 수명(초)
    public float lifeDistance = 0;//총알 수명(거리)

    public Unit Unit;//모체 유닛
    public bool isPlayers = false;//플레이어의 것인가?

    public bool testTriger = false;//테스트용 수동 발사기

    private void Update()
    {
        if (testTriger)
        {
            Triger();
            testTriger = false;
        }
    }

    //발사
    public void Triger()
    {
        Shoot();
    }

    private void Shoot()
    {
        switch (shootType)
        {
            case ShootType.oneWay://해당 방향으로 각도 범위 사이에서 무작위로 발사
                for (int i = 0; i < bulletNum; i++)
                {
                    Quaternion quat = Quaternion.Euler(0, 0, Random.Range(bulletAngleMin, bulletAngleMax));
                    Vector2 direction = quat * Vector2.up;
                    //direction.x *= -1;
                    MakeProjectile(GameTools.GetDirectionFormDegreeAngle(Random.Range(bulletAngleMin, bulletAngleMax)));
                }
                break;

            case ShootType.fan://해당 방향을 기준으로 각도 범위 사이에서 균일한 각도 간격으로 발사
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
    }


    //발사체 생성
    private void MakeProjectile(Vector2 direction)
    {
        //생성
        Attack attack = Attack.MakeGameObject(Unit, (isPlayers ? "Enemy" : "Player"));
        Destroy(attack.gameObject, lifeTime <= 0 ? 10f : lifeTime + 1);

        //초기화
        GameObject go = Instantiate(GO, transform.position, transform.rotation, attack.transform);
        if (go.GetComponentsInChildren<DamageArea>().Length >= 1)
        {
            attack.EnrollDamage(go);
        }
        else
        {
            attack.isDestroySelfAuto = false;
        }

        //총알 세팅값 설정
        Projectile projectile = go.GetComponent<Projectile>();
        projectile?.Initialize(
            direction, Random.Range(bulletSpeedMin, bulletSpeedMax), Unit,
            lifeTime: lifeTime, lifeDistance: lifeDistance);
    }

    //발사각 설정
    public void SetBulletAngle(float angle, float range = 0)
    {
        bulletAngleMax = angle + range;
        bulletAngleMin = angle - range;
    }
}

//발사 타입
public enum ShootType
{
    oneWay,//해당 방향으로 각도 범위 사이에서 무작위로 발사
    fan,//해당 방향을 기준으로 각도 범위 사이에서 균일한 각도 간격으로 발사
}