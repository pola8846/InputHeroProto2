using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�̱���
    private static InputManager instance;
    public static InputManager Instance => instance;

    [SerializeField]
    private SerializedDictionary<InputType, KeyCode> keyDict;//�Է��� �޾��� Ű
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
    private static Dictionary<InputType, bool> keyStay;//���� ������ �ִ� Ű
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
        //�̱���
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
        //�Է� �˻�
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
        //�Է� �˻�
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