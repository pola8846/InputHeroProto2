using UnityEngine;

public class SmoothMoving : MonoBehaviour
{
    public Vector3 directionPos;//�̵��� ��ġ

    //�ּ� �ӵ�
    public float minSpeed;
    //�̵� �Ÿ��� �� �� ���ϸ� �ּ� �ӵ�
    public float minSpeedDistance;

    //�ִ� �ӵ�
    public float maxSpeed;
    //�̵� �Ÿ��� �� �� �̻��̸� �ִ� �ӵ�
    public float maxSpeedDistance;

    public bool move = false;//�̵� ���ΰ�?

    public bool isLimitByCameraArea = false;//Ư�� ������ ����� �ʴ°�?

    private void FixedUpdate()
    {
        if (!move)
        {
            return;
        }

        //�Ÿ� ���
        float distance = Vector3.Distance(directionPos, transform.position);

        //�ּ�&�ִ� �Ÿ� �ٱ��� ��� �ӵ� ó��
        if (distance < minSpeedDistance)
        {
            Move(minSpeed);
        }
        else if (distance > maxSpeedDistance)
        {
            Move(maxSpeed);
        }
        else//�ּ�&�ִ� �Ÿ� ������ ��� ���������� ���
        {
            float dist = Vector3.Distance(transform.position, directionPos);
            float speedT = Mathf.InverseLerp(minSpeedDistance, maxSpeedDistance, dist);
            Move(Mathf.Lerp(minSpeed, maxSpeed, speedT));
        }


        if (isLimitByCameraArea)//��ϵ� ���� ����� �ʵ��� ��ó��
        {
            transform.position = GameTools.ClampToRect(transform.position, GameManager.CameraLimit);
        }
    }

    //�̵�
    private void Move(float speed)
    {
        Vector3 temp = transform.position - directionPos;
        temp = temp.normalized;
        temp *= -speed * Time.fixedUnscaledDeltaTime;
        transform.Translate(temp, Space.World);
    }
}
