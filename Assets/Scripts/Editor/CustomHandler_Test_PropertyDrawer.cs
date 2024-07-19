using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(CustomHandler_Test))]
public class CustomHandler_Test_PropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new VisualElement();

        container.Add(new PropertyField(property.FindPropertyRelative("handlers"), "�ڽ� �����"));
        //InspectorElement.FillDefaultInspector(container, property.serializedObject, null);

        var buttonContainer = new VisualElement();
        buttonContainer.style.flexDirection = FlexDirection.Row;

        Button addDialogueButton = new Button(() =>
        {
            SerializedProperty simpleEventsProperty = property.FindPropertyRelative("handlers");
            simpleEventsProperty.arraySize++;
            SerializedProperty newElement = simpleEventsProperty.GetArrayElementAtIndex(simpleEventsProperty.arraySize - 1);
            newElement.managedReferenceValue = new DialogueHandler_Test();
            property.serializedObject.ApplyModifiedProperties();
        })
        {
            text = "��ȭ"
        };
        buttonContainer.Add(addDialogueButton);

        container.Add(buttonContainer);

        return container; 
    }
}

