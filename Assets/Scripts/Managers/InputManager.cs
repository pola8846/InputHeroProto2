using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
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

    private PlayerUnit mMoveReceiver;

    private void Start()
    {
        mMoveReceiver = GameManager.Player;
    }

    void Update()
    {
        if (mMoveReceiver != null)
        {
            foreach (var pair in keyDict)
            {
                if (Input.GetKeyDown(pair.Value))
                {
                    mMoveReceiver.KeyDown(pair.Key);
                }
                else if (Input.GetKeyUp(pair.Value))
                {
                    mMoveReceiver.KeyUp(pair.Key);
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