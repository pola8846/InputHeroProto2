using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
    private static UIManager instance;
    public static UIManager Instance => instance;

    //
    [SerializeField]
    private TextMeshProUGUI bulletCounter;
    public TestActionBar testActionBar;

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
