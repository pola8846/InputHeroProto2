using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletNumberUIOnCanvas : MonoBehaviour
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

    int maxBullet = 8;  // �ִ� �Ѿ˰���
    int currentBullet;  // ���� �Ѿ˰���

    float warningDuration = 0.3F;

    float reloadingDuration = 1.0F;
    float gaugeValue = 1.0F;

    private void Start()
    {
        // maxBullet ������ŭ �Ѿ� UI ����
        for (int i = 0; i < maxBullet; i++)
        {
            GameObject go = Instantiate(singleBulletPrefab);

            go.transform.SetParent(transform, false);
            go.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * distance, 0.0F);

            singleBulletUI.Add(go.GetComponent<Image>());
        }

        // ���������θ� ���������� ������ ���� �о��ش�
        foreach (Image go in singleBulletUI)
        {
            go.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distance * maxBullet / 2.0F, 0);
        }

        ReloadBulletNum();
    }

    void Update()
    {
        // ��ġ ������Ʈ
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetWorldObject.position);

        //test
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DecreaseOneBulletNum();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GaugeReloading());
        }

        gaugeUI.GetComponent<Image>().fillAmount = gaugeValue;
    }

    void SetBulletNum(int num) // Ư�� ���ڷ� �Ѿ˰����� �ٲ��ݴϴ�
    {
        if (num < 0) num = 0;
        else if (num > maxBullet) num = maxBullet;

        for (int i = 0; i < maxBullet; i++)
        {
            if (i < num)
            {
                singleBulletUI[i].sprite = fullBullet;
            }
            else
            {
                singleBulletUI[i].sprite = emptyBullet;
            }
        }

        currentBullet = num;
    }

    public void ReloadBulletNum() // �Ѿ��� �ִ���ڷ� ä���ݴϴ�
    {
        foreach (Image go in singleBulletUI)
        {
            go.sprite = fullBullet;
        }

        currentBullet = maxBullet;
    }

    public void DecreaseOneBulletNum() // �Ѿ��� �Ѱ��Լ� �̸� �˼�..
    {
        if (currentBullet > 0)
        {
            currentBullet--;
            singleBulletUI[currentBullet].sprite = emptyBullet;
        }
        else
        {
            StartCoroutine(NoBulletWarning());
        }
    }

    IEnumerator NoBulletWarning()
    {
        foreach (Image go in singleBulletUI)
        {
            go.sprite = warningBullet;
        }

        yield return new WaitForSeconds(warningDuration);

        SetBulletNum(currentBullet);
    }

    IEnumerator GaugeReloading()
    {
        float elapsedTime = 0.0F;

        while (elapsedTime < reloadingDuration)
        {
            elapsedTime += Time.deltaTime;

            gaugeValue = Mathf.Lerp(0.0F, 1.0F, elapsedTime / reloadingDuration);

            yield return null;
        }

        gaugeValue = 1.0F;

        ReloadBulletNum();
    }
}
