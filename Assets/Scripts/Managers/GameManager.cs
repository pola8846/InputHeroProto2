using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    //�̱���
    private static GameManager instance;
    public static GameManager Instance => instance;


    private static PlayerUnit player;
    public static PlayerUnit Player => player;

    public GameObject testObj;


    [SerializeField]
    private float cameraZPos;
    public static float CameraZPos => instance.cameraZPos;

    //�� ���
    [SerializeField]
    private Transform mapLimit;
    public static Rect MapLimit
    {
        get
        {
            if (instance.mapLimit != null)
            {
                return GameTools.TransformToRect(instance.mapLimit);
            }
            UnityEngine.Debug.LogError("GameManager.MapLimit: mapLimit�� �����Ǿ� ���� ����");
            return Rect.zero;
        }
    }

    private Vector2 mousePos;
    private bool isMousePosCashed = false;
    public static Vector2 MousePos
    {
        get
        {
            if (!instance.isMousePosCashed)
            {
                /*
                 
                �ӽ�. �ϴ� 1920*1080 �ػ� �������� ������µ�, �ػ󵵰� �޶����� ���콺 ��ġ�� ���� ��ġ�� �޶����� ������ ����
                �̰����� �� �غôµ� �� ������ ������� �𸣰���. ������ ȯ���̶� �׷���??
                 
                 */
                Vector3 mp = Input.mousePosition;
                mp.z = Mathf.Abs(Camera.main.transform.position.z) - CameraZPos;
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mp);
                worldMousePos.z = 0;
                instance.mousePos = worldMousePos;
                UnityEngine.Debug.Log(worldMousePos);

                //Vector3 mp = Input.mousePosition;
                //UnityEngine.Debug.Log(mp);
                //Ray ray = Camera.main.ScreenPointToRay(mp);
                //UnityEngine.Debug.DrawRay(ray.origin, ray.origin + ray.direction * 100);
                //Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
                //float distance = -ray.origin.z / ray.direction.z;
                //instance.mousePos = ray.GetPoint(distance);
                //instance.testObj.transform.position = instance.mousePos;
                //UnityEngine.Debug.Log(instance.mousePos);

                ////ȭ�� ���� ���콺 ��ġ(0~1)
                //Vector2 mp = Input.mousePosition;
                //mp.x /= Screen.width;
                //mp.y /= Screen.height;
                //UnityEngine.Debug.Log(mp);
                ////ȭ�鿡 ��ġ�� ���� ���� ũ��
                //Rect viewport = GameTools.GetCameraViewportSize();
                //instance.mousePos = (Vector2)Camera.main.transform.position + new Vector2((viewport.width) * (mp.x - 0.5f), (viewport.height) * (mp.y - 0.5f));

                instance.testObj.transform.position = instance.mousePos;
                instance.isMousePosCashed = true;
            }
            return instance.mousePos;
        }
    }

    //ī�޶� ���. �������� ����/�Ҵ� �����Ͽ� ī�޶� ������ ����. null�̸� �������� ����
    public Transform cameraLimit;
    //ī�޶� ���� ������ ���� ��� �� ���� ī�޶� ���� ��ġ�� �κи�
    public static Rect CameraLimit
    {
        get
        {
            if (instance.mapLimit != null)
            {
                Rect map = GameTools.TransformToRect(instance.mapLimit);
                Rect result = map;
                if (instance.cameraLimit !=null)
                {
                    Rect cl = GameTools.TransformToRect(instance.cameraLimit);
                    result = GameTools.CalculateOverlapRect(map, cl);
                }

                Rect cameraSize = GameTools.GetCameraViewportSize();

                if (cameraSize.width< result.width)
                {
                    result.xMax -= cameraSize.width / 2;
                    result.xMin += cameraSize.width / 2;
                }
                else
                {
                    float temp = result.width/2;
                    result.xMax -= temp;
                    result.xMin += temp;
                }

                if (cameraSize.height<result.height)
                {
                    result.yMax -= cameraSize.height / 2;
                    result.yMin += cameraSize.height / 2;
                }
                else
                {
                    float temp = result.height / 2;
                    result.yMax -= temp;
                    result.yMin += temp;
                }

                return result;
            }
            UnityEngine.Debug.LogError("GameManager.MapLimit: mapLimit�� �����Ǿ� ���� ����");
            return Rect.zero;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //�̱���
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Update()
    {
        isMousePosCashed = false;
    }

    public static void SetPlayer(PlayerUnit player)
    {
        GameManager.player = player;
    }


}
