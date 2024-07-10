using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : MonoBehaviour
{
    // 싱글톤
    static BulletManager instance = null;

    public static BulletManager Instance
    {
        get
        {
            if (instance == null) return null;
            else return instance;
        }
    }

    // 기본 정보
    int maxBullet = 8;              // 최대 총알개수
    public int MaxBullet
    {
        get { return maxBullet; }
    }

    int currentBullet;              // 현재 총알개수
    public int CurrentBullet
    {
        get { return currentBullet; }
    }

    // 재장전 관련
    [SerializeField]
    float reloadDuration = 8.0F;    // 걸리는 시간
    public float ReloadDuration
    {
        get { return reloadDuration; }
    }

    bool isReloading = false;       // 재장전중인지 여부
    public bool IsReloading
    {
        get { return isReloading; }
    }

    Coroutine reloadingCoroutine;

    // UI 이벤트 함수
    [HideInInspector]
    public UnityEvent OnBulletNumUpdated;   // -> 총알 개수가 바뀌면 UI도 업데이트

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
