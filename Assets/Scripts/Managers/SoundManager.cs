using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
    private static SoundManager instance;
    public static SoundManager Instance => instance;

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
}
