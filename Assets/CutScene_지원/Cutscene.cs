using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// ����: �ƾ��� ���� �簢�� ����
/// Show�� ���� �簢���� ��������, Hide�� �ٽ� ġ����
/// </summary>

public class Cutscene : MonoBehaviour
{
    bool activated;

    // ������ ��������
    public RectTransform upperFrame;
    public RectTransform lowerFrame;

    // �̹��� ����
    Image cutsceneImage;

    public float originalAnchorPosY = 810.0F;
    public float frameMoveDistance = 170.0F;
    public float frameMoveTime = 0.8F;

    public bool Activated
    {
        get { return activated; }
    }

    void Start()
    {
        upperFrame.anchoredPosition = new Vector2(0.0F, originalAnchorPosY);
        lowerFrame.anchoredPosition = new Vector2(0.0F, -originalAnchorPosY);
        cutsceneImage = GetComponent<Image>();
        cutsceneImage.enabled = false;
    }

    public void Show(string fileName = null)
    {
        activated = true;

        //// ������ �����ֱ�
        upperFrame.DOAnchorPosY(originalAnchorPosY - frameMoveDistance, frameMoveTime, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-(originalAnchorPosY - frameMoveDistance), frameMoveTime, false).SetUpdate(true);

        // �̻��ϰ� SetSpeedBased �۵��� �ʹ� ���� ��ĥ���� ����
        //upperFrame.DOAnchorPosY(originalAnchorPosY - frameMoveDistance, frameMoveSpeed).SetSpeedBased(true);
        //lowerFrame.DOAnchorPosY(-(originalAnchorPosY - frameMoveDistance), frameMoveSpeed).SetSpeedBased(true);

        // �̹��������� �Է��ߴٸ� �̹��� ����
        if (fileName != null) ShowImage(fileName);
    }

    public void Hide()
    {
        activated = false;

        //// ������ �����
        upperFrame.DOAnchorPosY(originalAnchorPosY, frameMoveTime, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-originalAnchorPosY, frameMoveTime, false).SetUpdate(true);

        // �̻��ϰ� SetSpeedBased �۵��� �ʹ� ���� ��ĥ���� ����
        //upperFrame.DOAnchorPosY(originalAnchorPosY, frameMoveSpeed).SetSpeedBased(true).SetUpdate(true);
        //lowerFrame.DOAnchorPosY(-originalAnchorPosY, frameMoveSpeed).SetSpeedBased(true).SetUpdate(true);

        if (cutsceneImage.enabled) { cutsceneImage.enabled = false; }
    }

    // ���ҽ����� �̹��� ã�ƿͼ� ����
    void ShowImage(string fileName)
    {
        Texture2D texture = Resources.Load<Texture2D>(fileName);

        if (texture)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0.0F, 0.0F, texture.width, texture.height), new Vector2(0.5F, 0.5F));
            cutsceneImage.sprite = sprite;
            cutsceneImage.enabled = true;
        }
        else
        {
            Debug.LogError("����: Resources ���� �� " + fileName + "�̶�� �̸��� �̹����� ã�� ���߾��!");
        }
    }

    void Update()
    {
        //// Test
        //if (Input.GetKeyDown(KeyCode.O)) EnterCutscene();
        //if (Input.GetKeyDown(KeyCode.K)) EnterCutscene("testCutscene");
        //if (Input.GetKeyDown(KeyCode.P)) EnterCutscene("testCutscene2");
        //if (Input.GetKeyDown(KeyCode.L)) ExitCutscene();
    }
}
