using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    //싱글톤
    private static GameManager instance;
    public static GameManager Instance => instance;


    private static PlayerUnit player;
    public static PlayerUnit Player => player;

    public GameObject testObj;


    [SerializeField]
    private float cameraZPos;
    public static float CameraZPos => instance.cameraZPos;

    //맵 경계
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
            UnityEngine.Debug.LogError("GameManager.MapLimit: mapLimit가 설정되어 있지 않음");
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
                 
                임시. 일단 1920*1080 해상도 기준으로 맞춰놨는데, 해상도가 달라지면 마우스 위치와 계산된 위치가 달라지는 문제가 있음
                이것저것 다 해봤는데 왜 오차가 생기는진 모르겠음. 에디터 환경이라 그런가??
                 
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

                ////화면 상의 마우스 위치(0~1)
                //Vector2 mp = Input.mousePosition;
                //mp.x /= Screen.width;
                //mp.y /= Screen.height;
                //UnityEngine.Debug.Log(mp);
                ////화면에 비치는 실제 영역 크기
                //Rect viewport = GameTools.GetCameraViewportSize();
                //instance.mousePos = (Vector2)Camera.main.transform.position + new Vector2((viewport.width) * (mp.x - 0.5f), (viewport.height) * (mp.y - 0.5f));

                instance.testObj.transform.position = instance.mousePos;
                instance.isMousePosCashed = true;
            }
            return instance.mousePos;
        }
    }

    //카메라 경계. 동적으로 설정/할당 해제하여 카메라 고정을 관리. null이면 고정하지 않음
    public Transform cameraLimit;
    //카메라 영역 제한이 있을 경우 맵 경계와 카메라 제한 겹치는 부분만
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
            UnityEngine.Debug.LogError("GameManager.MapLimit: mapLimit가 설정되어 있지 않음");
            return Rect.zero;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //싱글톤
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
