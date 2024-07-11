using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //싱글톤
    private static InputManager instance;
    public static InputManager Instance => instance;

    [SerializeField]
    private SerializedDictionary<InputType, KeyCode> keyDict;//입력을 받아줄 키
    private static SerializedDictionary<InputType, KeyCode> KeyDict
    {
        get
        {
            if (Instance.keyDict == null)
            {
                Instance.keyDict = new();
            }
            return Instance.keyDict;
        }
    }
    private static Dictionary<InputType, bool> keyStay;//현재 눌리고 있는 키
    private static Dictionary<InputType, bool> KeyStay
    {
        get
        {
            if (keyStay==null)
            {
                keyStay = new();
            }
            return keyStay;
        }
    }

    private void Awake()
    {
        //싱글톤
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    void Update()
    {
        foreach (var pair in KeyDict)
        {
            if (Input.GetKeyDown(pair.Value))
            {
                OnKeyDown(pair.Key);
            }
            else if (Input.GetKeyUp(pair.Value))
            {
                OnKeyUp(pair.Key);
            }
        }
    }

    private void OnKeyDown(InputType inputType)
    {
        //입력 검사
        if (KeyStay.ContainsKey(inputType))
        {
            KeyStay[inputType] = true;
        }
        else
        {
            KeyStay.Add(inputType, true);
        }

        if (GameManager.Player != null)
            GameManager.Player.KeyDown(inputType);
    }
    private void OnKeyUp(InputType inputType)
    {
        //입력 검사
        if (KeyStay.ContainsKey(inputType))
        {
            KeyStay[inputType] = false;
        }
        else
        {
            KeyStay.Add(inputType, false);
        }

        if (GameManager.Player != null)
            GameManager.Player.KeyUp(inputType);
    }

    public void KeyReset()
    {
        foreach (var item in KeyStay)
        {
            OnKeyUp(item.Key);
        }
        KeyStay.Clear();
    }

    public static bool IsKeyPushing(InputType inputType)
    {
        return KeyStay.ContainsKey(inputType) && KeyStay[inputType];
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