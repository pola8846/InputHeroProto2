using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    bool initialized = false;
    Type[] inheritedTypes;
    string[] typePopupNameArray;
    string[] typeFullNameArray;
    int currentTypeIndex;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference) return;

        if (!initialized)
        {
            Initialize(property);
            initialized = true;
        }

        GetCurrentTypeIndex(property.managedReferenceFullTypename);
        int selectedTypeIndex = EditorGUI.Popup(GetPopupPosition(position), currentTypeIndex, typePopupNameArray);
        UpdatePropertyToSelectedTypeIndex(property, selectedTypeIndex);
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }

    private void Initialize(SerializedProperty property)
    {
        GetAllInheritedTypes(GetFieldType(property));
        GetInheritedTypeNameArrays();
    }

    private void GetCurrentTypeIndex(string typeFullName)
    {
        currentTypeIndex = Array.IndexOf(typeFullNameArray, typeFullName);
    }

    private void GetAllInheritedTypes(Type baseType)
    {
        inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p != baseType && baseType.IsAssignableFrom(p) && p.IsClass)
            .Prepend(null)
            .ToArray();
    }

    private void GetInheritedTypeNameArrays()
    {
        //typePopupNameArray = inheritedTypes.Select(type => type == null ? "null" : type.ToString()).ToArray();
        typePopupNameArray = new string[inheritedTypes.Length];
        for (int i = 0; i < inheritedTypes.Length; i++)
        {
            if (inheritedTypes[i] == null)
            {
                typePopupNameArray[i] = "null";
            }
            else
            {
                string typeString = inheritedTypes[i].ToString();

                switch (typeString)
                {
                    case "DialogueHandler_Test":
                        typePopupNameArray[i] = "다이얼로그 실행부";
                        break;
                    case "CameraFocusHandler_Test":
                        typePopupNameArray[i] = "카메라 시선변경 실행부";
                        break;
                    default:
                        typePopupNameArray[i] = inheritedTypes[i].ToString();
                        break;
                }
            }
        }

        typeFullNameArray = inheritedTypes.Select(type => type == null ? "" : string.Format("{0} {1}", type.Assembly.ToString().Split(',')[0], type.FullName)).ToArray();
    }

    public static Type GetFieldType(SerializedProperty property)
    {
        string[] fieldTypename = property.managedReferenceFieldTypename.Split(' ');
        var assembly = Assembly.Load(fieldTypename[0]);
        return assembly.GetType(fieldTypename[1]);
    }

    private void UpdatePropertyToSelectedTypeIndex(SerializedProperty property, int selectedTypeIndex)
    {
        if (currentTypeIndex == selectedTypeIndex) return;
        currentTypeIndex = selectedTypeIndex;
        Type selectedType = inheritedTypes[selectedTypeIndex];
        property.managedReferenceValue =
            selectedType == null ? null : Activator.CreateInstance(selectedType);
    }

    private Rect GetPopupPosition(Rect currentPosition)
    {
        Rect popupPosition = new Rect(currentPosition);
        popupPosition.width -= EditorGUIUtility.labelWidth;
        popupPosition.x += EditorGUIUtility.labelWidth;
        popupPosition.height = EditorGUIUtility.singleLineHeight;
        return popupPosition;
    }
}