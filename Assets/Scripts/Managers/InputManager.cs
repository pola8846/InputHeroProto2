using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�̱���
    private static InputManager instance;
    public static InputManager Instance
    {
           get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }

    [SerializeField]
    private SerializedDictionary<InputType, KeyCode> keyDict = new();


    private void Start()
    {
    }

    void Update()
    {
        if (GameManager.Player != null)
        {
            foreach (var pair in keyDict)
            {
                if (Input.GetKeyDown(pair.Value))
                {
                    GameManager.Player.KeyDown(pair.Key);
                }
                else if (Input.GetKeyUp(pair.Value))
                {
                    GameManager.Player.KeyUp(pair.Key);
                }
            }
        }
    }
}

public enum InputType
{
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    Jump,
    Dash,
    Shoot,
    Reload,
    Slow,
    MeleeAttack,

}