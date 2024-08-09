using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�̱���
    private static InputManager instance;
    public static InputManager Instance => instance;

    private List<IMoveReceiver> receivers = new List<IMoveReceiver>();

    [SerializeField]
    private float listCheckTime = 1f;

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
    private  Dictionary<InputType, bool> keyStay;//���� ������ �ִ� Ű
    private static Dictionary<InputType, bool> KeyStay
    {
        get
        {
            if (Instance.keyStay == null)
            {
                Instance.keyStay = new();
            }
            return Instance.keyStay;
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

    private void Start()
    {
        KeyReset();
        StartCoroutine(ListNullCheck());
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

        foreach (var receiver in Instance.receivers)
        {
            if (receiver!=null)
            {
                receiver.KeyDown(inputType);
            }
        }

        //if (GameManager.Player != null)
          //  GameManager.Player.KeyDown(inputType);
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

        foreach (var receiver in Instance.receivers)
        {
            if (receiver != null)
            {
                receiver.KeyUp(inputType);
            }
        }

        //if (GameManager.Player != null)
        //  GameManager.Player.KeyUp(inputType);
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

    // �ϴ� �߰�..
    public static bool IsKeyDown(InputType inputType)
    {
        return Input.GetKeyDown(KeyDict[inputType]);
    }
    public static bool IsKeyUp(InputType inputType)
    {
        return Input.GetKeyUp(KeyDict[inputType]);
    }
    public static bool IsKeyDownOrPushing(InputType inputType) // ���������� �ϴ�..
    {
        if (KeyStay.ContainsKey(inputType) && KeyStay[inputType])
        {
            return true;
        }
        else
        {
            if (IsKeyPushing(inputType))
            {
                return true;
            }
            else return false;
        }
    }

    public static void EnrollReciver(IMoveReceiver receiver)
    {
        if (receiver != null && !Instance.receivers.Contains(receiver))
        {
            Instance.receivers.Add(receiver);
        }
    }

    public static void RemoveReciver(IMoveReceiver receiver)
    {
        if (receiver != null && Instance.receivers.Contains(receiver))
        {
            Instance.receivers.Remove(receiver);
        }
    }

    private IEnumerator ListNullCheck()
    {
        while (true)
        {
            // �������� ����Ʈ�� ��ȸ�ϸ鼭 null üũ �� ����
            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] == null)
                {
                    receivers.RemoveAt(i);
                }
            }
            yield return new WaitForSecondsRealtime(listCheckTime);
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