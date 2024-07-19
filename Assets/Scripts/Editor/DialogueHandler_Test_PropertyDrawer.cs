using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// the unity editor(propertyDrawer) code below shows base propertydrawer before the newly added properties. how can I make it show other way around, showing the base properties in the end?

[CustomPropertyDrawer(typeof(DialogueHandler_Test))]
public class DialogueHandler_Test_PropertyDrawer : CustomHandler_Test_PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new VisualElement();

        container.Add(new PropertyField(property.FindPropertyRelative("dialogueIndex"), "��ȭ �ε���"));
        container.Add(new PropertyField(property.FindPropertyRelative("dialogueSO"), "��ȭ ����Ʈ SO"));
        container.Add(new PropertyField(property.FindPropertyRelative("boxTypeSO"), "�ۻ��� ���� SO"));

        container.Add(base.CreatePropertyGUI(property));

        return container;
    }
}