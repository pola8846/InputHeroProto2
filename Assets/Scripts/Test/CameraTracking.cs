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
    private float trackingDistance;//���� ������ �Ÿ�
    [SerializeField]
    private Vector2 mouseTrackingRange;//���콺 ��ġ�� ���� ������ ���� �ִ�ġ
    [SerializeField]
    private Vector2 mouseTrackingRangeOffset;//���콺 ��ġ�� ���� ������ ���� ������



    private void Start()
    {
        //originPos = transform.position;
        smooth = GetComponent<SmoothMoving>();
    }
    void Update()
    {
        Vector2 mouseDir = (Vector3)GameManager.MousePos - GameManager.Player.transform.position;//���콺������ �Ÿ�
        mouseDir /= 2;
        mouseDir.x = Mathf.Clamp(mouseDir.x, mouseTrackingRangeOffset.x - Mathf.Abs(mouseTrackingRange.x), mouseTrackingRangeOffset.x + Mathf.Abs(mouseTrackingRange.x));
        mouseDir.y = Mathf.Clamp(mouseDir.y, mouseTrackingRangeOffset.y - Mathf.Abs(mouseTrackingRange.y), mouseTrackingRangeOffset.y + Mathf.Abs(mouseTrackingRange.y));

        Debug.Log(mouseDir);
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
