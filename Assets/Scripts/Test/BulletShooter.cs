using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject GO;//�߻��� ������

    [SerializeField]
    private int bulletNum = 1;//�߻��� �Ѿ� ��
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

    public ShootType shootType;//�߻� Ÿ��

    [SerializeField]
    private float bulletSpeedMin = 1;//�Ѿ� �ּ� �ӵ�
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
    private float bulletSpeedMax = 1;//�Ѿ� �ִ� �ӵ�
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

    public float bulletAngleMin = 90;//�Ѿ� �ּ� ����
    public float bulletAngleMax = 90;//�Ѿ� �ִ� ����
    public float BulletAngle
    {
        set
        {
            SetBulletAngle(value, 0);
        }
    }

    public float lifeTime = 0;//�Ѿ� ����(��)
    public float lifeDistance = 0;//�Ѿ� ����(�Ÿ�)

    public Unit Unit;//��ü ����
    public bool isPlayers = false;//�÷��̾��� ���ΰ�?

    public bool testTriger = false;//�׽�Ʈ�� ���� �߻��

    private void Update()
    {
        if (testTriger)
        {
            Triger();
            testTriger = false;
        }
    }

    //�߻�
    public void Triger()
    {
        Shoot();
    }

    private void Shoot()
    {
        switch (shootType)
        {
            case ShootType.OneWay://�ش� �������� ���� ���� ���̿��� �������� �߻�
                for (int i = 0; i < bulletNum; i++)
                {
                    Quaternion quat = Quaternion.Euler(0, 0, Random.Range(bulletAngleMin, bulletAngleMax));
                    Vector2 direction = quat * Vector2.up;
                    //direction.x *= -1;
                    MakeProjectile(GameTools.GetDirectionFormDegreeAngle(Random.Range(bulletAngleMin, bulletAngleMax)));
                }
                break;

            case ShootType.Fan://�ش� ������ �������� ���� ���� ���̿��� ������ ���� �������� �߻�
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


    //�߻�ü ����
    private void MakeProjectile(Vector2 direction)
    {
        //����
        Attack attack = Attack.MakeGameObject(Unit, (isPlayers ? "Enemy" : "Player"));
        Destroy(attack.gameObject, lifeTime <= 0 ? 10f : lifeTime + 1);

        //�ʱ�ȭ
        GameObject go = Instantiate(GO, transform.position, transform.rotation, attack.transform);
        if (go.GetComponentsInChildren<DamageArea>().Length >= 1)
        {
            attack.EnrollDamage(go);
        }
        else
        {
            attack.isDestroySelfAuto = false;
        }

        //�Ѿ� ���ð� ����
        Projectile projectile = go.GetComponent<Projectile>();
        projectile?.Initialize(
            direction, Random.Range(bulletSpeedMin, bulletSpeedMax), Unit,
            lifeTime: lifeTime, lifeDistance: lifeDistance);
    }

    //�߻簢 ����
    public void SetBulletAngle(float angle, float range = 0)
    {
        bulletAngleMax = angle + range;
        bulletAngleMin = angle - range;
    }
}

//�߻� Ÿ��
public enum ShootType
{
    OneWay,//�ش� �������� ���� ���� ���̿��� �������� �߻�
    Fan,//�ش� ������ �������� ���� ���� ���̿��� ������ ���� �������� �߻�
}