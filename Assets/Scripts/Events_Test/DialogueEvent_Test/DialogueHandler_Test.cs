using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler_Test : CustomHandler_Test
{
    public int dialogueIndex;

    // ��纰 ���� ��������
    public DialoguesSO_Test dialogueSO;

    // �ۻ��� Ÿ��
    public DialogueBoxType boxTypeSO;

    Canvas canvas;
    GameObject UIPrefab;

    // �ۻ��� �ν��Ͻ�
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

    public override void Run() // isDone_This�� true�� ����� ������ �߰�
    {
        base.Run();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDone_This = true;
        }
    }

    protected override void Exit() // ������ �ѹ� ������ �ڵ�
    {
        GameObject.Destroy(inst);

        base.Exit();
    }
}
