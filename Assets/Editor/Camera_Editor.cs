using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(CameraTracking))]
public class Camera_Editor : Editor
{
    public VisualTreeAsset m_InspectorXML;
    public override VisualElement CreateInspectorGUI()
    {
        // Create a new VisualElement to be the root of our inspector UI
        VisualElement myInspector = new VisualElement();

        // Load and clone a visual tree from UXML
        m_InspectorXML.CloneTree(myInspector);

        Vector3Field originPos = myInspector.Q<Vector3Field>("originPos");

        originPos.RegisterValueChangedCallback(evt =>
        {
            CameraTracking component = (CameraTracking)target;
            component.originPos = evt.newValue;
            component.Move();
        });

        // Get a reference to the default inspector foldout control
        VisualElement inspectorFoldout = myInspector.Q("Default_Inspector");
        // Attach a default inspector to the foldout
        InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

        // Return the finished inspector UI
        return myInspector;
    }
}
