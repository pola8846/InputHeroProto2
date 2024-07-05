using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : MonoBehaviour
{
    // �⺻ ����
    int maxBullet = 8;              // �ִ� �Ѿ˰���
    public int MaxBullet
    {
        get { return maxBullet; }
    }

    int currentBullet;              // ���� �Ѿ˰���
    public int CurrentBullet
    {
        get { return currentBullet; }
    }

    // ������ ����
    [SerializeField]
    float reloadDuration = 1.0F;    // �ɸ��� �ð�
    public float ReloadDuration
    {
        get { return reloadDuration; }
    }

    bool isReloading = false;       // ������������ ����
    public bool IsReloading
    {
        get { return isReloading; }
    }

    // UI �̺�Ʈ �Լ�
    [HideInInspector]
    public UnityEvent OnBulletNumUpdated;   // -> �Ѿ� ������ �ٲ�� UI�� ������Ʈ

    [HideInInspector]
    public UnityEvent OnBulletUseFailed;    // -> UI warning color

    [HideInInspector]
    public UnityEvent OnReload;             // -> UI reload gauge


    // �̱���
    static BulletManager instance = null;

    public static BulletManager Instance
    {
        get
        {
            if (instance == null) return null;
            else return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentBullet = maxBullet;
    }

    //void SetBulletNum(int num)
    //{
    //    if (num < 0) num = 0;
    //    else if (num > maxBullet) num = maxBullet;

    //    currentBullet = num;
    //}

    void ReloadBulletNum()
    {
        currentBullet = maxBullet;
        OnBulletNumUpdated?.Invoke();
    }

    public void UseOneBullet()
    {
        if (currentBullet > 0)
        {
            currentBullet--;
            OnBulletNumUpdated?.Invoke();
        }
        else
        {
            OnBulletUseFailed?.Invoke(); // UI Warning
        }
    }

    public void StartReloading()
    {
        if (!isReloading)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        OnReload?.Invoke();

        yield return new WaitForSeconds(reloadDuration);

        ReloadBulletNum();

        isReloading = false;
    }
}
