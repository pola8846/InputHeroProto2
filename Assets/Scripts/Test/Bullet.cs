using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;//진행 방향
    private float turnSpeed;
    [SerializeField]
    private float speed;
    private BulletMoveType moveType;
    private float lifeDistance = 0f;
    private Vector3 originPos;


    public bool hitOnce = true;
    public float damage = 1f;

    public void DealDamage(Unit unit)
    {
        if (hitOnce)
        {
            // 총알을 파괴
            Destroy();
        }

        // 대미지
        //unit.Health -= damage;
    }

    private void Start()
    {
        originPos = transform.position;
    }

    public void Initialize(BulletMoveType moveType, Vector2 dir, float damage, float speed, float turnSpeed = 0, float lifeTime = 0f, float lifeDistance = 0f)
    {
        this.moveType = moveType;
        direction = dir;
        this.speed = speed;
        this.turnSpeed = turnSpeed;
        this.damage = damage;
        if (lifeTime > 0)
        {
            Destroy(lifeTime);
        }
        this.lifeDistance = lifeDistance;
        originPos = transform.position;
    }

    private void Update()
    {
        if (lifeDistance > 0 && Vector3.Distance(originPos, transform.position) >= lifeDistance)
        {
            Destroy();
            return;
        }
        if (direction == null)
        {
            return;
        }
        switch (moveType)
        {
            case BulletMoveType.straight:
                transform.Translate(speed * Time.deltaTime * direction);
                break;
            case BulletMoveType.curve:
                transform.Translate(speed * Time.deltaTime * direction);
                Quaternion quat = Quaternion.Euler(0, 0, turnSpeed * Time.deltaTime);
                direction = quat * direction;
                break;
            default:
                break;
        }
    }

    private void Destroy(float lifeTime = -1)
    {
        if (lifeTime>0)
        {
            StartCoroutine(DestroyDelay(lifeTime));
        }
        else
        {
            var da = GetComponent<DamageArea>();
            da.Destroy();
        }
        
    }

    private IEnumerator DestroyDelay(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        var da = GetComponent<DamageArea>();
        da.Destroy();
    }
}

public enum BulletMoveType
{
    straight,
    curve,
}