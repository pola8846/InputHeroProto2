using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{

    public float rotationSpeed;
    public float ZPos;
    private Vector3 dir;
    public float moveSpd;
    [SerializeField]
    private GameObject targetter;


    [SerializeField] private GameObject player;
    public bool dirChecker = true;

    private void Start()
    {
        ZPos = 11; // transform.position.z;
    }
    void Update()
    {

        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = ZPos;
        Vector3 convertToPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        Debug.Log("���콺 ��ġ : " + convertToPos);
        //Debug.Log("���콺 ��ġ : " + mousePos);
        Vector3 temp = Camera.main.ScreenToWorldPoint(convertToPos);

        if (dirChecker)
        {
            if (temp.x <= transform.position.x)
            {
                switchdirChecker();
            }
        }
        else
        {
            if (temp.x >= transform.position.x)
            {
                switchdirChecker();
            }
        }

        //Debug.Log(temp);
        //dir = (temp - transform.position);
        //Debug.Log("���� �ϴ� ���� : " + dir);

        /*float angle = Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg;
        Debug.Log("���� : " + angle);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);


        Vector3 cursorPos = temp;
        transform.position = Vector3.MoveTowards(transform.position, temp, moveSpd * Time.deltaTime);*/

        /// �� ���콺 ������ ��ġ - ���� ĳ������ ��ġ = 
        //Debug.Log("�� ��ġ :" + transform.position);

        Vector2 dir = (temp - (transform.position + (Vector3)Vector2.up * .5f)).normalized * 2;
        targetter.transform.position = (Vector3)dir + transform.position + (Vector3)Vector2.up * .5f;


        

        /// ���� �������� chest ���� -50 < x < 50
        /// ���� �������� head ���� =100 < x < 100
    }


    public void switchdirChecker()
    {
        player.transform.eulerAngles = player.transform.eulerAngles + new Vector3(0, 180, 0);
        dirChecker = !dirChecker;
    }
}
