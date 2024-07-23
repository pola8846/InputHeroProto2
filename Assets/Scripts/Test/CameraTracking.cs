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
    public bool mouseTracking = true;// 마우스 위치에 따라 움직일 여부

    [SerializeField]
    private Transform focusPoint;// 카메라가 비출 타겟
    public bool isFocusing; // 카메라의 시선이 아직 타겟으로 이동 중인가??
    public bool IsFocusing
    {
        get { return isFocusing; }
    }

    private void Start()
    {
        //originPos = transform.position;
        smooth = GetComponent<SmoothMoving>();

        //focusPoint = GameManager.Player.transform;
    }
    void Update()
    {
        if (GameManager.Player == null)
            return;

        Vector3 targetPos;

        if (mouseTracking)
        {
            Vector2 mouseDir = (Vector3)GameManager.MousePos - GameManager.Player.transform.position;//마우스까지의 거리
            mouseDir /= 2;
            mouseDir.x = Mathf.Clamp(mouseDir.x, mouseTrackingRangeOffset.x - Mathf.Abs(mouseTrackingRange.x), mouseTrackingRangeOffset.x + Mathf.Abs(mouseTrackingRange.x));
            mouseDir.y = Mathf.Clamp(mouseDir.y, mouseTrackingRangeOffset.y - Mathf.Abs(mouseTrackingRange.y), mouseTrackingRangeOffset.y + Mathf.Abs(mouseTrackingRange.y));

            targetPos = (focusPoint.position + (Vector3)mouseDir) + originPos;
        }
        else
        {
            targetPos = focusPoint.position + originPos;
        }

        if (Vector3.Distance(transform.position, targetPos) > trackingDistance)
        {
            isFocusing = true;
            Vector3 dist = targetPos - transform.position;
            dist = dist.normalized;
            dist *= trackingDistance;
            smooth.directionPos = targetPos - dist;
            smooth.move = true;
        }
        else
        {
            isFocusing = false;
            smooth.move = false;
        }

        // test
        if (Input.GetKeyDown(KeyCode.B)) { SetFocusPoint(GameObject.Find("barrel")); }
        else if (Input.GetKeyDown(KeyCode.P)) { SetFocusPoint(GameManager.Player.gameObject); }
    }

    public void Move()
    {
        GameObject player = GameObject.Find("Player");
        transform.position = originPos + player.transform.position;
    }

    public void SetFocusPoint(GameObject focus)
    {
        focusPoint = focus.transform;

        if (focus == GameManager.Player.gameObject)
        {
            mouseTracking = true;
        }
        else
        {
            mouseTracking = false;
        }
    }
}