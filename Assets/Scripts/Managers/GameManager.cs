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
    }

    public static void SetPlayer(PlayerUnit player)
    {
        GameManager.player = player;
    }
}
