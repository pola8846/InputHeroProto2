using UnityEngine;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(SmoothMoving))]
public class CameraTracking : MonoBehaviour
{
    public Vector3 originPos;//초기 위치
    private SmoothMoving smooth;//이동 대행해줄 스크립트
    [SerializeField]
    private float trackingDistance;//추적 시작할 거리

    [SerializeField]
    private Vector2 mouseTrackingRange;//마우스 위치에 따라 움직일 범위 최대치
    [SerializeField]
    private Vector2 mouseTrackingRangeOffset;//마우스 위치에 따라 움직일 범위 오프셋
    public bool mouseTracking = true;// 마우스 위치에 따라 움직일 여부

    [SerializeField]
    private Transform focusPoint;// 카메라가 비출 타겟
    public bool isFocusing; // 카메라의 시선이 아직 타겟으로 이동 중인가?
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

        if (mouseTracking)//마우스 추적 시, 마우스 방향으로 일정 비율 따라감
        {
            Vector2 mouseDir = (Vector3)GameManager.MousePos - GameManager.Player.transform.position;//마우스까지의 거리
            mouseDir /= 2;
            mouseDir.x = Mathf.Clamp(mouseDir.x, mouseTrackingRangeOffset.x - Mathf.Abs(mouseTrackingRange.x), 
                mouseTrackingRangeOffset.x + Mathf.Abs(mouseTrackingRange.x));
            mouseDir.y = Mathf.Clamp(mouseDir.y, mouseTrackingRangeOffset.y - Mathf.Abs(mouseTrackingRange.y), 
                mouseTrackingRangeOffset.y + Mathf.Abs(mouseTrackingRange.y));

            targetPos = (focusPoint.position + (Vector3)mouseDir) + originPos;
        }
        else//그냥 따라감
        {
            targetPos = focusPoint.position + originPos;
        }

        //이동에 필요한 최소 거리보다 목표 지점까지의 거리가 길다면 이동
        if (Vector3.Distance(transform.position, targetPos) > trackingDistance)
        {
            isFocusing = true;
            Vector3 dist = targetPos - transform.position;
            dist = dist.normalized;
            dist *= trackingDistance;
            smooth.directionPos = targetPos - dist;
            smooth.move = true;
        }
        else//이동 정지
        {
            isFocusing = false;
            smooth.move = false;
        }
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