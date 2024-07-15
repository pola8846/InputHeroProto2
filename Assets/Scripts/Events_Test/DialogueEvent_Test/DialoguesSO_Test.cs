using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueInfo
{
    public string text;
    // 대화 효과는 아직 에셋에 따라 어떻게 적용할지 모른다

    public Vector2 rectPosition;
}

[CreateAssetMenu(menuName = "DialoguesSO_Test")]
public class DialoguesSO_Test : ScriptableObject
{
    public List<DialogueInfo> dialogues = new List<DialogueInfo>();
}