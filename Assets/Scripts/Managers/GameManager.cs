using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
    private static GameManager instance;
    public static GameManager Instance => instance;


    private static PlayerUnit player;
    public static PlayerUnit Player => player;

    private const int framePerSec = 50;

    private static event EventHandler<float> onTimeScaleChanged;
    public static event EventHandler<float> OnTimeScaleChanged
    {
        add { onTimeScaleChanged += value;}
        remove { onTimeScaleChanged -= value;}
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //ΩÃ±€≈Ê
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
    }

    public static void SetPlayer(PlayerUnit player)
    {
        GameManager.player = player;
    }

    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        onTimeScaleChanged?.Invoke(instance, Time.timeScale);
    }
}
