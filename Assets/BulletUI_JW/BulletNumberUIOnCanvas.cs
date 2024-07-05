using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletNumberUIOnCanvas : MonoBehaviour
{
    public Transform targetWorldObject;

    public GameObject singleBulletPrefab;
    public List<Image> singleBulletUI = new List<Image>();

    public GameObject gaugeUI;

    public Sprite fullBullet;
    public Sprite emptyBullet;
    public Sprite warningBullet;

    // 총알UI 한칸 사이 간격
    float distance = 10F;

    float warningDuration = 0.3F;


    float gaugeValue = 1.0F;

    void Awake()
    {
        // maxBullet 개수만큼 총알 UI 생성
        for (int i = 0; i < BulletManager.Instance.MaxBullet; i++)
        {
            GameObject go = Instantiate(singleBulletPrefab);

            go.transform.SetParent(transform, false);
            go.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * distance, 0.0F);

            singleBulletUI.Add(go.GetComponent<Image>());
        }

        // 이벤트 함수 구독
        BulletManager.Instance.OnBulletNumUpdated.AddListener(SetBulletNum);
        BulletManager.Instance.OnBulletUseFailed.AddListener(StartNoBulletWarning);
        BulletManager.Instance.OnReload.AddListener(StartGaugeReloading);
    }

    void Start()
    {
        // 오른쪽으로만 생성했으니 옆으로 반쯤 밀어준다
        foreach (Image go in singleBulletUI)
        {
            go.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distance * (BulletManager.Instance.MaxBullet - 1) / 2.0F, 0);
        }

        gaugeUI.SetActive(false);

        SetBulletNum();
    }

    void Update()
    {
        // 위치 업데이트
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetWorldObject.position);

        if (gaugeUI.activeSelf) gaugeUI.GetComponent<Image>().fillAmount = gaugeValue;
    }

    // BulletManager의 현재 총알 개수에 맞춰 UI 업데이트
    void SetBulletNum()
    {
        for (int i = 0; i < BulletManager.Instance.MaxBullet; i++)
        {
            if (i < BulletManager.Instance.CurrentBullet)
            {
                singleBulletUI[i].sprite = fullBullet;
            }
            else
            {
                singleBulletUI[i].sprite = emptyBullet;
            }
        }
    }

    void StartNoBulletWarning()
    {
        StartCoroutine(NoBulletWarning());
    }

    void StartGaugeReloading()
    {
        // 그냥 인보크 함수 쓰자
        gaugeUI.SetActive(true);
        Invoke("EndGaugeReloading", BulletManager.Instance.ReloadDuration);
    }

    void EndGaugeReloading()
    {
        gaugeUI.SetActive(false);
    }

    IEnumerator NoBulletWarning()
    {
        foreach (Image go in singleBulletUI)
        {
            go.sprite = warningBullet;
        }

        yield return new WaitForSeconds(warningDuration);

        SetBulletNum();
    }
}
