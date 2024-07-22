using DG.Tweening;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 임시로 만든 대화창 이벤트
[CreateAssetMenu(menuName = "DialogueHandler_Test_JW")]
public class DialogueHandler_Test : CustomHandler_Test
{
    public int dialogueIndex;

    // 대사별 정보 가져오기
    public DialoguesSO_Test dialogueSO;

    // 글상자 타입
    public DialogueBoxType boxTypeSO;

    Canvas canvas;
    GameObject UIPrefab;

    // 글상자 인스턴스
    GameObject inst;

    // 대화 끝내기 가능 화살표 이미지
    GameObject nextArrow;
    Tween arrowBounceAnim;

    // 글자 하나씩 나타나게 하는 거
    TypewriterCore typeWriter; // 이게 한국어로도 이쁘게 보일지는 잘 모르겠다
    bool skipped = false;

    public override void Enter()
    {
        base.Enter();
        skipped = false;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        UIPrefab = Resources.Load<GameObject>("DialogueBox");

        if (UIPrefab == null || canvas == null)
        {
            Debug.Log("캔버스와 UI 프리팹을 찾지 못함 -> DialogueHandler_Test 강제종료!");
            lifeCycleBools.isDone_This = true;
            return;
        }

        // 컴포넌트 가져오기
        inst = Object.Instantiate(UIPrefab, canvas.transform);
        TextMeshProUGUI tmp = inst.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        typeWriter = inst.transform.Find("Text (TMP)").gameObject.GetComponent<TypewriterCore>();
        nextArrow = inst.transform.Find("Next").gameObject;

        // 컴포넌트에 설정 적용하기
        DialogueInfo info = dialogueSO.dialogues[dialogueIndex];

        inst.GetComponent<Image>().sprite = boxTypeSO.BoxImage;
        inst.GetComponent<RectTransform>().anchoredPosition = info.rectPosition;

        tmp.fontSize = boxTypeSO.FontSize;
        tmp.gameObject.GetComponent<RectTransform>().anchoredPosition = info.textLocalPosition;
        
        typeWriter.ShowText(info.text); //tmp.text = info.text;
        typeWriter.onTextShowed.AddListener(MakeSkipped);
    }

    public override void Run() // isDone_This를 true로 만드는 조건을 추가
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

        // 화살표 이미지 켜기
        nextArrow.GetComponent<Image>().enabled = true;

        // 화살표 바운스 시키기
        float bounceDistance = 17.0F;
        float bounceSpeed = 0.3F;
        Vector2 bounceTarget = nextArrow.GetComponent<RectTransform>().anchoredPosition;
        bounceTarget.x += bounceDistance;
        arrowBounceAnim = nextArrow.GetComponent<RectTransform>().DOAnchorPos(bounceTarget, bounceSpeed).SetLoops(-1, LoopType.Yoyo);

        skipped = true;
    }

    protected override void Exit() // 나갈때 한번 실행할 코드
    {
        if (arrowBounceAnim != null) arrowBounceAnim.Kill();
        Object.Destroy(inst);

        base.Exit();
        //skipped = false;
    }
}