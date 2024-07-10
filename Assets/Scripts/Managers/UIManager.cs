using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //�̱���
    private static UIManager instance;
    public static UIManager Instance => instance;

    //
    [SerializeField]
    private TextMeshProUGUI bulletCounter;
    public TestActionBar testActionBar;

    // UI �̺�Ʈ �Լ�
    [HideInInspector]
    public UnityEvent OnBulletNumUpdated;   // -> �Ѿ� ������ �ٲ�� UI�� ������Ʈ

    [HideInInspector]
    public UnityEvent OnBulletUseFailed;    // -> UI warning color

    [HideInInspector]
    public UnityEvent OnReload;             // -> UI reload gauge

    [HideInInspector]
    public UnityEvent OnCancelReload;       // -> UI reload gauge cancel

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //�̱���
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    public static void SetBulletCounter(int bulletNum)
    {
        if (instance.bulletCounter == null)
        {
            return;
        }
        //Debug.Log(bulletNum);
        instance.bulletCounter.text = $"Bullet: {bulletNum}";
    }
}
