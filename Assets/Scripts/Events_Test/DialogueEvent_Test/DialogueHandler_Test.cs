using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public override void Enter()
    {
        base.Enter();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        UIPrefab = Resources.Load<GameObject>("DialogueBox");

        if (UIPrefab == null)
        {
            isDone_This = true;
            return;
        }

        inst = GameObject.Instantiate(UIPrefab, canvas.transform);
        inst.GetComponent<Image>().sprite = boxTypeSO.BoxImage;
        inst.GetComponent<RectTransform>().anchoredPosition = dialogueSO.dialogues[dialogueIndex].rectPosition;

        TextMeshProUGUI tmp = inst.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();

        tmp.fontSize = boxTypeSO.FontSize;
        tmp.text = dialogueSO.dialogues[dialogueIndex].text;
    }

    public override void Run() // isDone_This를 true로 만드는 조건을 추가
    {
        base.Run();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDone_This = true;
        }
    }

    protected override void Exit() // 나갈때 한번 실행할 코드
    {
        GameObject.Destroy(inst);

        base.Exit();
    }
}
