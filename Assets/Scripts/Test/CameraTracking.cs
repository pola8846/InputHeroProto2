using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(SmoothMoving))]
public class CameraTracking : MonoBehaviour
{
    public Vector3 originPos;
    private SmoothMoving smooth;
    [SerializeField]
    private float trackingDistance;//추적 시작할 거리



    private void Start()
    {
        //originPos = transform.position;
        smooth = GetComponent<SmoothMoving>();
    }
    void Update()
    {
        Vector3 targetPos = GameManager.Player.transform.position + originPos;
        if (Vector3.Distance(transform.position, targetPos) > trackingDistance)
        {
            Vector3 dist = targetPos - transform.position;
            dist = dist.normalized;
            dist *= trackingDistance;
            smooth.directionPos = targetPos - dist;
            smooth.move = true;
        }
        else
        {
            smooth.move = false;
        }
    }

    public void Move()
    {
        GameObject player = GameObject.Find("Player");
        transform.position = originPos + player.transform.position;
    }
}
