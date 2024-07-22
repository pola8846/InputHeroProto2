using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueInfo
{
    public string text; // Text Animator 이펙트 정보는 텍스트 내부에 태그(ex. <shake>text</shake>)로 같이들어가게 된다

    // 월드 포지션
    public Vector2 rectPosition;

    // 글상자 내에서의 오프셋 값
    public Vector2 textLocalPosition;
}

//[CreateAssetMenu(menuName = "DialoguesSO_Test_JW")]
public class DialoguesSO_Test : ScriptableObject
{
    public List<DialogueInfo> dialogues = new List<DialogueInfo>();
}