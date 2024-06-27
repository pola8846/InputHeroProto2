using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 지원: 컷씬용 검은 사각형 제어
/// Show로 검은 사각형이 내려오고, Hide로 다시 치워짐
/// </summary>

public class Cutscene : MonoBehaviour
{
    bool activated;

    // 프레임 내려오기
    public RectTransform upperFrame;
    public RectTransform lowerFrame;

    // 이미지 띄우기
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

        //// 프레임 보여주기
        upperFrame.DOAnchorPosY(originalAnchorPosY - frameMoveDistance, frameMoveTime, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-(originalAnchorPosY - frameMoveDistance), frameMoveTime, false).SetUpdate(true);

        // 이상하게 SetSpeedBased 작동이 너무 느려 고칠수가 없다
        //upperFrame.DOAnchorPosY(originalAnchorPosY - frameMoveDistance, frameMoveSpeed).SetSpeedBased(true);
        //lowerFrame.DOAnchorPosY(-(originalAnchorPosY - frameMoveDistance), frameMoveSpeed).SetSpeedBased(true);

        // 이미지파일을 입력했다면 이미지 띄우기
        if (fileName != null) ShowImage(fileName);
    }

    public void Hide()
    {
        activated = false;

        //// 프레임 숨기기
        upperFrame.DOAnchorPosY(originalAnchorPosY, frameMoveTime, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-originalAnchorPosY, frameMoveTime, false).SetUpdate(true);

        // 이상하게 SetSpeedBased 작동이 너무 느려 고칠수가 없다
        //upperFrame.DOAnchorPosY(originalAnchorPosY, frameMoveSpeed).SetSpeedBased(true).SetUpdate(true);
        //lowerFrame.DOAnchorPosY(-originalAnchorPosY, frameMoveSpeed).SetSpeedBased(true).SetUpdate(true);

        if (cutsceneImage.enabled) { cutsceneImage.enabled = false; }
    }

    // 리소스에서 이미지 찾아와서 띄우기
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
            Debug.LogError("지원: Resources 폴더 내 " + fileName + "이라는 이름의 이미지를 찾지 못했어요!");
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
