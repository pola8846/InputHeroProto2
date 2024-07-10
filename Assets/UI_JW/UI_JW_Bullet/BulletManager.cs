using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : MonoBehaviour
{
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
    float reloadDuration = 8.0F;    // �ɸ��� �ð�
    public float ReloadDuration
    {
        get { return reloadDuration; }
    }

    bool isReloading = false;       // ������������ ����
    public bool IsReloading
    {
        get { return isReloading; }
    }

    Coroutine reloadingCoroutine;

    // UI �̺�Ʈ �Լ�
    [HideInInspector]
    public UnityEvent OnBulletNumUpdated;   // -> �Ѿ� ������ �ٲ�� UI�� ������Ʈ

    [HideInInspector]
    public UnityEvent OnBulletUseFailed;    // -> UI warning color

    [HideInInspector]
    public UnityEvent OnReload;             // -> UI reload gauge

    [HideInInspector]
    public UnityEvent OnCancelReload;       // -> UI reload cancel

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

    void ReloadBulletNum()
    {
        currentBullet = maxBullet;
        OnBulletNumUpdated?.Invoke();
    }

    public bool UseOneBullet()
    {
        if (currentBullet > 0)
        {
            currentBullet--;
            OnBulletNumUpdated?.Invoke();
            return true;
        }
        else
        {
            OnBulletUseFailed?.Invoke(); // UI Warning
            return false;
        }
    }

    public void StartReloading()
    {
        if (!isReloading)
        {
            reloadingCoroutine = StartCoroutine(Reloading());
        }
    }

    public void CancelReloading()
    {
        if (reloadingCoroutine != null) StopCoroutine(reloadingCoroutine);
        isReloading = false;

        OnCancelReload?.Invoke();
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        OnReload?.Invoke();

        yield return new WaitForSeconds(reloadDuration);

        ReloadBulletNum();

        isReloading = false;
    }

    void Update()
    {
        // test

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UseOneBullet();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartReloading();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CancelReloading();
        }
    }
}
