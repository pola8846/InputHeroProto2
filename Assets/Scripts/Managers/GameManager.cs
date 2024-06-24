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
            Debug.LogError("GameManager.MapLimit: mapLimit가 설정되어 있지 않음");
            return Rect.zero;
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
            Debug.LogError("GameManager.MapLimit: mapLimit가 설정되어 있지 않음");
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

    public static void SetPlayer(PlayerUnit player)
    {
        GameManager.player = player;
    }


}
