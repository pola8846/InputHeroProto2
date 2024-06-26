using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����: ��ȭ���� ���Ʒ��� ���� ������ �������� �� ȿ��
/// �̹��� ���� �̸��� �Բ� �Է��ϸ� �̹����� ��� �ش�
/// 
/// ���� �ð� ����� �ȵ���
/// </summary>

public class CutSceneFrameMove : MonoBehaviour
{
    public RectTransform cutSceneImage;
    public RectTransform upperFrame;
    public RectTransform lowerFrame;

    // �ƾ��� Ȱ��ȭ �Ǿ��°�
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

    // �ƾ� ����
    public void EnterCutscene(string fileName = null)
    {
        if (fileName != null)
        {
            Texture2D texture = Resources.Load<Texture2D>(fileName);

            if (texture == null)
            {
                Debug.LogError("����: Resources ���� �� " + fileName + "�̶�� �̸��� �̹����� ã�� ���߾��!");
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

    // �ƾ� ��
    public void ExitCutscene()
    {
        cutSceneImage.gameObject.SetActive(false);

        cutsceneActivated = false;
        upperFrame.DOAnchorPosY(810.0F, 0.8F, false).SetUpdate(true);
        lowerFrame.DOAnchorPosY(-810.0F, 0.8F, false).SetUpdate(true);
    }

}
