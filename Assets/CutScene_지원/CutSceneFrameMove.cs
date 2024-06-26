using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 지원: 영화마냥 위아래서 검은 프레임 내려오는 그 효과
/// 이미지 파일 이름을 함께 입력하면 이미지도 띄워 준다
/// 
/// 아직 시간 제어는 안됐음
/// </summary>

public class CutSceneFrameMove : MonoBehaviour
{
    public RectTransform cutSceneImage;
    public RectTransform upperFrame;
    public RectTransform lowerFrame;

    // 컷씬이 활성화 되었는가
    bool cutsceneActivated;

    void Start()
    {
        cutsceneActivated = false;
    }

    void Update()
    {
        //// Test

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    EnterCutscene("testCutscene");
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    EnterCutscene("testCutscene2");
        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    EnterCutscene();
        //}

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    ExitCutscene();
        //}
    }

    // 컷씬 시작
    public void EnterCutscene(string fileName = null)
    {
        if (fileName != null)
        {
            Texture2D texture = Resources.Load<Texture2D>(fileName);

            if (texture == null)
            {
                Debug.LogError("지원: Resources 폴더 내 " + fileName + "이라는 이름의 이미지를 찾지 못했어요!");
            }
            else
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                cutSceneImage.gameObject.GetComponent<Image>().sprite = sprite;
                cutSceneImage.gameObject.SetActive(true);
            }
        }
        else
        {
            cutSceneImage.gameObject.SetActive(false);
        }

        cutsceneActivated = true;
        upperFrame.DOAnchorPosY(649.0F, 0.8F, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-640.0F, 0.8F, false).SetUpdate(true);
    }

    // 컷씬 끝
    public void ExitCutscene()
    {
        cutSceneImage.gameObject.SetActive(false);

        cutsceneActivated = false;
        upperFrame.DOAnchorPosY(810.0F, 0.8F, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-810.0F, 0.8F, false).SetUpdate(true);
    }

}
