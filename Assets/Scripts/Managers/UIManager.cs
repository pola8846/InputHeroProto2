using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
    private static UIManager instance;
    public static UIManager Instance => instance;

    // UI ¿Ã∫•∆Æ «‘ºˆ
    [HideInInspector]
    public UnityEvent OnBulletNumUpdated;   // -> √—æÀ ∞≥ºˆ∞° πŸ≤Ó∏È UIµµ æ˜µ•¿Ã∆Æ

    [HideInInspector]
    public UnityEvent OnBulletUseFailed;    // -> UI warning color

    [HideInInspector]
    public UnityEvent OnReload;             // -> UI reload gauge

    [HideInInspector]
    public UnityEvent OnCancelReload;       // -> UI reload gauge

    //
    [SerializeField]
    private TextMeshProUGUI bulletCounter;
    public TestActionBar testActionBar;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //ΩÃ±€≈Ê
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
