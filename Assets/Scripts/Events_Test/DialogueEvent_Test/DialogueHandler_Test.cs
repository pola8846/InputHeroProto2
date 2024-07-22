using DG.Tweening;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �ӽ÷� ���� ��ȭâ �̺�Ʈ
[CreateAssetMenu(menuName = "DialogueHandler_Test_JW")]
public class DialogueHandler_Test : CustomHandler_Test
{
    public int dialogueIndex;

    // ��纰 ���� ��������
    public DialoguesSO_Test dialogueSO;

    // �ۻ��� Ÿ��
    //public DialogueBoxType boxTypeSO;

    Canvas canvas;
    public GameObject UIPrefab;

    // UIPrefab�� �ν��Ͻ�
    GameObject UIInst;

    // ��ȭ ������ ���� ȭ��ǥ �̹���
    GameObject nextArrow;
    Tween arrowBounceAnim;

    // ���� �ϳ��� ��Ÿ���� �ϴ� ��
    TypewriterCore typeWriter;
    bool skipped = false;

    public override void Enter()
    {
        base.Enter();
        skipped = false;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        if (UIPrefab == null || canvas == null)
        {
            Debug.Log("ĵ������ UI �������� ã�� ���� -> DialogueHandler_Test ��������!");
            lifeCycleBools.isDone_This = true;
            return;
        }

        // ������Ʈ ��������
        UIInst = Object.Instantiate(UIPrefab, canvas.transform);
        TextMeshProUGUI tmp = UIInst.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        typeWriter = UIInst.transform.Find("Text (TMP)").gameObject.GetComponent<TypewriterCore>();
        nextArrow = UIInst.transform.Find("Next").gameObject;

        // ������Ʈ�� ���� �����ϱ�
        DialogueInfo info = dialogueSO.dialogues[dialogueIndex];

        UIInst.GetComponent<RectTransform>().anchoredPosition = info.rectPosition;

        tmp.gameObject.GetComponent<RectTransform>().anchoredPosition = info.textLocalPosition;
        
        typeWriter.ShowText(info.text); //tmp.text = info.text;
        typeWriter.onTextShowed.AddListener(MakeSkipped);
    }

    public override void Run() // isDone_This�� true�� ����� ������ �߰�
    {
        base.Run();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!skipped)
            {
                typeWriter.SkipTypewriter();
                MakeSkipped();
            }
            else
            {
                lifeCycleBools.isDone_This = true;
            }
        }
    }

    void MakeSkipped()
    {
        if (skipped) return;

        // ȭ��ǥ �̹��� �ѱ�
        if (nextArrow != null)
        {
            nextArrow.GetComponent<Image>().enabled = true;

            // ȭ��ǥ �ٿ ��Ű��
            float bounceDistance = 17.0F;
            float bounceSpeed = 0.3F;
            Vector2 bounceTarget = nextArrow.GetComponent<RectTransform>().anchoredPosition;
            bounceTarget.x += bounceDistance;
            arrowBounceAnim = nextArrow.GetComponent<RectTransform>().DOAnchorPos(bounceTarget, bounceSpeed).SetLoops(-1, LoopType.Yoyo);
        }

        skipped = true;
    }

    protected override void Exit() // ������ �ѹ� ������ �ڵ�
    {
        if (arrowBounceAnim != null) arrowBounceAnim.Kill();
        Object.Destroy(UIInst);

        base.Exit();
        //skipped = false;
    }
}