using UnityEngine;

public class SmoothMoving : MonoBehaviour
{
    public Vector3 directionPos;//이동할 위치

    //최소 속도
    public float minSpeed;
    //이동 거리가 이 값 이하면 최소 속도
    public float minSpeedDistance;

    //최대 속도
    public float maxSpeed;
    //이동 거리가 이 값 이상이면 최대 속도
    public float maxSpeedDistance;

    public bool move = false;//이동 중인가?

    public bool isLimitByCameraArea = false;//특정 영역을 벗어나지 않는가?

    private void FixedUpdate()
    {
        if (!move)
        {
            return;
        }

        //거리 계산
        float distance = Vector3.Distance(directionPos, transform.position);

        //최소&최대 거리 바깥일 경우 속도 처리
        if (distance < minSpeedDistance)
        {
            Move(minSpeed);
        }
        else if (distance > maxSpeedDistance)
        {
            Move(maxSpeed);
        }
        else//최소&최대 거리 사이일 경우 선형적으로 계산
        {
            float dist = Vector3.Distance(transform.position, directionPos);
            float speedT = Mathf.InverseLerp(minSpeedDistance, maxSpeedDistance, dist);
            Move(Mathf.Lerp(minSpeed, maxSpeed, speedT));
        }


        if (isLimitByCameraArea)//등록된 영역 벗어나지 않도록 후처리
        {
            transform.position = GameTools.ClampToRect(transform.position, GameManager.CameraLimit);
        }
    }

    //이동
    private void Move(float speed)
    {
        Vector3 temp = transform.position - directionPos;
        temp = temp.normalized;
        temp *= -speed * Time.fixedUnscaledDeltaTime;
        transform.Translate(temp, Space.World);
    }
}
