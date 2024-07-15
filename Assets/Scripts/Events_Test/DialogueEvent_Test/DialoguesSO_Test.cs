using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueInfo
{
    public string text;
    // ��ȭ ȿ���� ���� ���¿� ���� ��� �������� �𸥴�

    public Vector2 rectPosition;
}

[CreateAssetMenu(menuName = "DialoguesSO_Test")]
public class DialoguesSO_Test : ScriptableObject
{
    public List<DialogueInfo> dialogues = new List<DialogueInfo>();
}