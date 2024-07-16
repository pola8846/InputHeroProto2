using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueInfo
{
    public string text; // Text Animator ����Ʈ ������ �ؽ�Ʈ ���ο� �±�(ex. <shake>text</shake>)�� ���̵��� �ȴ�

    // ���� ������
    public Vector2 rectPosition;

    // �ۻ��� �������� ������ ��
    public Vector2 textLocalPosition;
}

[CreateAssetMenu(menuName = "DialoguesSO_Test_JW")]
public class DialoguesSO_Test : ScriptableObject
{
    public List<DialogueInfo> dialogues = new List<DialogueInfo>();
}