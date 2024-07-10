using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.ComponentModel;

public class BulletNumberUI : MonoBehaviour
{
    public PlayerUnit playerScript;
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
        for (int i = 0; i < playerScript.MaxBullet; i++)
        {
            GameObject go = Instantiate(singleBulletPrefab);

            go.transform.SetParent(transform, false);
            go.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * distance, 0.0F);

            singleBulletUI.Add(go.GetComponent<Image>());
        }
    }

    void Start()
    {
        // �̺�Ʈ �Լ� ����
        UIManager.Instance.OnBulletNumUpdated.AddListener(SetBulletNum);
        UIManager.Instance.OnBulletUseFailed.AddListener(StartNoBulletWarning);
        UIManager.Instance.OnReload.AddListener(StartGaugeReloading);
        UIManager.Instance.OnCancelReload.AddListener(EndGaugeReloading);

        // ���������θ� ���������� ������ ���� �о��ش�
        foreach (Image go in singleBulletUI)
        {
            go.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distance * (playerScript.MaxBullet - 1) / 2.0F, 0);
        }

        gaugeUI.SetActive(false);

        SetBulletNum();
    }

    void Update()
    {
        if (playerScript == null) gameObject.SetActive(false);
        if (targetWorldObject == null) return;
        // ��ġ ������Ʈ
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetWorldObject.position);

        if (gaugeUI.activeSelf) gaugeUI.GetComponent<Image>().fillAmount = gaugeValue;
    }

    void SetBulletNum()
    {
        for (int i = 0; i < playerScript.MaxBullet; i++)
        {
            if (i < playerScript.NowBullet)
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
        gaugeValue = 0.0F;
        gaugeUI.SetActive(true);

        gaugeTween = DOTween.To(() => gaugeValue, x => gaugeValue = x, 1.0F, playerScript.ReloadTime);
        Invoke("EndGaugeReloading", playerScript.ReloadTime);
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