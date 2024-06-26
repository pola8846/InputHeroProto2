using UnityEngine;
using UnityEngine.UI;

public class DialogueGenerate : MonoBehaviour
{
    public Transform Canvas;
    public GameObject DialogueBoxPrefab;

    void Start()
    {
        Instantiate(DialogueBoxPrefab, Canvas);
    }

    public GameObject CreateDialogueBox(string text, Transform target, Vector2 offset)
    {
        return new GameObject("DialogueBox");
    }

    public GameObject CreateDialogueBox(string text, Vector2 UIPos)
    {
        return new GameObject("DialogueBox");
    }

    void Update()
    {
        
    }
}
