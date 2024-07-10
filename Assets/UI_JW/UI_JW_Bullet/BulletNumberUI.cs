using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.ComponentModel;

public class BulletNumberUI : MonoBehaviour
{
    public Transform targetWorldObject;

    public GameObject singleBulletPrefab;
    public List<Image> singleBulletUI = new List<Image>();

    public GameObject gaugeUI;

    public Sprite fullBullet;
    public Sprite emptyBullet;
    public Sprite warningBullet;

    // �Ѿ�UI ��ĭ ���� ����
    float distance = 10F;

    float warningDuration = 0.3F;
    Tween gaugeTween;

    float gaugeValue = 0.0F;

    void Awake()
    {
        // maxBullet ������ŭ �Ѿ� UI ����
        for (int i = 0; i < BulletManager.Instance.MaxBullet; i++)
        {
            GameObject go = Instantiate(singleBulletPrefab);

            go.transform.SetParent(transform, false);
            go.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * distance, 0.0F);

            singleBulletUI.Add(go.GetComponent<Image>());
        }

        // �̺�Ʈ �Լ� ����
        BulletManager.Instance.OnBulletNumUpdated.AddListener(SetBulletNum);
        BulletManager.Instance.OnBulletUseFailed.AddListener(StartNoBulletWarning);
        BulletManager.Instance.OnReload.AddListener(StartGaugeReloading);
        BulletManager.Instance.OnCancelReload.AddListener(EndGaugeReloading);
    }

    void Start()
    {
        // ���������θ� ���������� ������ ���� �о��ش�
        foreach (Image go in singleBulletUI)
        {
            go.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distance * (BulletManager.Instance.MaxBullet - 1) / 2.0F, 0);
        }

        gaugeUI.SetActive(false);

        SetBulletNum();
    }

    void Update()
    {
        // ��ġ ������Ʈ
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetWorldObject.position);

        if (gaugeUI.activeSelf) gaugeUI.GetComponent<Image>().fillAmount = gaugeValue;
    }

    // BulletManager�� ���� �Ѿ� ������ ���� UI ������Ʈ
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
        gaugeUI.SetActive(true);
        gaugeValue = 0.0F;
        gaugeTween = DOTween.To(() => gaugeValue, x => gaugeValue = x, 1.0F, BulletManager.Instance.ReloadDuration);
        Invoke("EndGaugeReloading", BulletManager.Instance.ReloadDuration);
    }

    void EndGaugeReloading()
    {
        CancelGaugeReloading();

        gaugeUI.SetActive(false);
    }

    void CancelGaugeReloading()
    {
        if (gaugeTween != null && gaugeTween.IsActive())
        {
            gaugeTween.Kill();
        }
        CancelInvoke("EndGaugeReloading");
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