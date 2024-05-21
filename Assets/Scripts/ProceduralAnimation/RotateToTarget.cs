using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RotateToTarget : MonoBehaviour
{

    public float rotationSpeed;
    public float ZPos;
    private Vector3 dir;
    public float moveSpd;
    [SerializeField, Range(1,5)] private float radious;
    [SerializeField]
    private GameObject targetter;
    [SerializeField]
    private GameObject hightarget;

    [SerializeField] private GameObject leftHint; 
    [SerializeField] private GameObject rightHint;

    public float hintOffset;
    private Vector3 hintFix;

    [SerializeField] private GameObject rightHandIK;
    [SerializeField] private GameObject leftHandIK;

    private float changeEllipse;

    [SerializeField] private GameObject player;

    [SerializeField, Range(0.01f, 2)]
    private float dist;
    public bool dirChecker = true;

    private void Start()
    {
        ZPos = 11; // transform.position.z;
        hintFix = new Vector3(hintOffset,0,0);
    }
    void Update()
    {

        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = ZPos;
        Vector3 convertToPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        //Debug.Log("���콺 ��ġ : " + convertToPos);
        //Debug.Log("���콺 ��ġ : " + mousePos);
        Vector3 temp = Camera.main.ScreenToWorldPoint(convertToPos);

        temp.z = 0;

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
        
        //dir = Camera.main.ScreenToWorldPoint(convertToPos) - transform.position;


       // dir = (temp - transform.position);
        //Debug.Log("���� �ϴ� ���� : " + dir);

       // float angle = Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg;
        ///Debug.Log("���� : " + angle);
      //  Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

      //  float ellipseX = targetter.transform.position.x;
      //  float ellipseY = targetter.transform.position.y;

      //  changeEllipse = (ellipseX * t1) * (ellipseY * t2);

      //  ellipseX = ellipseX + (Mathf.Cos(angle) * radious);
      //  ellipseY = ellipseY + (Mathf.Sin(angle) * radious - changeEllipse);



        //Vector3 cursorPos = temp;
        //transform.position = Vector3.MoveTowards(transform.position, temp, moveSpd * Time.deltaTime);*/

        /// �� ���콺 ������ ��ġ - ���� ĳ������ ��ġ = 
        //Debug.Log("�� ��ġ :" + transform.position);

        Vector3 temp2 = transform.position;
        temp2.z = 0;
        Vector2 dir = (temp - (temp2 + (Vector3)Vector2.up * .5f)).normalized * dist;
        Debug.Log((temp - (temp2 + (Vector3)Vector2.up * .5f)));
        targetter.transform.position = (Vector3)dir + temp2 + (Vector3)Vector2.up *1.5f;
        hightarget.transform.position = (Vector3)dir * 2.5f + temp2 + (Vector3)Vector2.up * 1.5f;
        
        Debug.Log(dir.magnitude);
        

        /// ���� �������� chest ���� -50 < x < 50
        /// ���� �������� head ���� =100 < x < 100
        /// 
        /// y�� ���� �� ���� ���ݾ� ��ǥ�� ������� �Ѵ�
    }


    public void switchdirChecker()
    {
        if(dirChecker)
        {
            //Vector3 temp = new Vector3(player.transform.localScale.x*-1, player.transform.localScale.y, player.transform.localScale.z*-1);
            //player.transform.localScale = temp;

            //player.transform.rotation = Quaternion.EulerAngles(player.transform.rotation.x, 154, player.transform.rotation.z);

            player.transform.eulerAngles = player.transform.eulerAngles + new Vector3(0, 60, 0);

            //leftHandIK.GetComponent<TwoBoneIKConstraint>().

    

            dirChecker = !dirChecker;
        }
        else
        {
           // Vector3 temp = new Vector3(player.transform.localScale.x * -1, player.transform.localScale.y, player.transform.localScale.z * -1);
           // player.transform.localScale = temp;
            //player.transform.rotation = Quaternion.EulerAngles(player.transform.rotation.x, 334, player.transform.rotation.z);
            player.transform.eulerAngles = player.transform.eulerAngles + new Vector3(0, -60, 0);

          

            dirChecker = !dirChecker;
        }

    }
}
