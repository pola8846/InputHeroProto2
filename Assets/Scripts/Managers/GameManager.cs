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
                Vector3 mp = Input.mousePosition;
                mp.z = Mathf.Abs(Camera.main.transform.position.z);
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mp);
                worldMousePos.z = 0;
                instance.mousePos = worldMousePos;

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
