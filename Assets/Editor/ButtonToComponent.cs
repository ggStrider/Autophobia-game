using Autophobia.Utilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddSlipperyToChildrenColliders))]

public class ButtonToComponent : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        var addSlipperyComponent = (AddSlipperyToChildrenColliders)target;
        if (GUILayout.Button("Add slippery"))
        {
            addSlipperyComponent.MakeChildrenObjectsSlippery(); 
        }
    }
}
