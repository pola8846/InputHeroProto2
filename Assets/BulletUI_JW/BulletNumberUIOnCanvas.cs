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

    // 총알UI 한칸 사이 간격
    float distance = 10F;

    int maxBullet = 8;  // 최대 총알개수
    int currentBullet;  // 현재 총알개수

    float warningDuration = 0.3F;

    bool isReloading = false;
    float reloadingDuration = 1.0F;
    float gaugeValue = 1.0F;

    private void Start()
    {
        // maxBullet 개수만큼 총알 UI 생성
        for (int i = 0; i < maxBullet; i++)
        {
            GameObject go = Instantiate(singleBulletPrefab);

            go.transform.SetParent(transform, false);
            go.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * distance, 0.0F);

            singleBulletUI.Add(go.GetComponent<Image>());
        }

        // 오른쪽으로만 생성했으니 옆으로 반쯤 밀어준다
        foreach (Image go in singleBulletUI)
        {
            go.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distance * maxBullet / 2.0F, 0);
        }

        ReloadBulletNum();
    }

    void Update()
    {
        // 위치 업데이트
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetWorldObject.position);

        //test
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DecreaseOneBulletNum();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isReloading)
        {
            StartCoroutine(GaugeReloading());
        }

        gaugeUI.GetComponent<Image>().fillAmount = gaugeValue;
    }

    void SetBulletNum(int num) // 특정 숫자로 총알개수를 바꿔줍니다
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

    public void ReloadBulletNum() // 총알을 최대숫자로 채워줍니다
    {
        foreach (Image go in singleBulletUI)
        {
            go.sprite = fullBullet;
        }

        currentBullet = maxBullet;
    }

    public void DecreaseOneBulletNum() // 총알을 한개함수 이름 죄송..
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
        isReloading = true;

        float elapsedTime = 0.0F;

        while (elapsedTime < reloadingDuration)
        {
            elapsedTime += Time.deltaTime;

            gaugeValue = Mathf.Lerp(0.0F, 1.0F, elapsedTime / reloadingDuration);

            yield return null;
        }

        gaugeValue = 1.0F;

        ReloadBulletNum();

        isReloading = false;
    }
}
