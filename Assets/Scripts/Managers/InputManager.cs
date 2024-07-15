using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�̱���
    private static InputManager instance;
    public static InputManager Instance => instance;

    private static List<IMoveReceiver> receivers = new List<IMoveReceiver>();

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
    private static Dictionary<InputType, bool> keyStay;//���� ������ �ִ� Ű
    private static Dictionary<InputType, bool> KeyStay
    {
        get
        {
            if (keyStay == null)
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

    private void Start()
    {
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

        foreach (var receiver in receivers)
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

        foreach (var receiver in receivers)
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


    public static void EnrollReciver(IMoveReceiver receiver)
    {
        if (receiver != null && !receivers.Contains(receiver))
        {
            receivers.Add(receiver);
        }
    }

    public static void RemoveReciver(IMoveReceiver receiver)
    {
        if (receiver != null && receivers.Contains(receiver))
        {
            receivers.Remove(receiver);
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