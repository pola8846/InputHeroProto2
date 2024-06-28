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
    [SerializeField]
    private Vector2 mouseTrackingRange;//마우스 위치에 따라 움직일 범위 최대치
    [SerializeField]
    private Vector2 mouseTrackingRangeOffset;//마우스 위치에 따라 움직일 범위 오프셋



    private void Start()
    {
        //originPos = transform.position;
        smooth = GetComponent<SmoothMoving>();
    }
    void Update()
    {
        Vector2 mouseDir = (Vector3)GameManager.MousePos - GameManager.Player.transform.position;//마우스까지의 거리
        mouseDir /= 2;
        mouseDir.x = Mathf.Clamp(mouseDir.x, mouseTrackingRangeOffset.x - Mathf.Abs(mouseTrackingRange.x), mouseTrackingRangeOffset.x + Mathf.Abs(mouseTrackingRange.x));
        mouseDir.y = Mathf.Clamp(mouseDir.y, mouseTrackingRangeOffset.y - Mathf.Abs(mouseTrackingRange.y), mouseTrackingRangeOffset.y + Mathf.Abs(mouseTrackingRange.y));

        Vector3 targetPos = (GameManager.Player.transform.position + (Vector3)mouseDir) + originPos;
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
