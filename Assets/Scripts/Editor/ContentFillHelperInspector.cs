using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ContentFillHelper))]
public class ContentFillHelperInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ContentFillHelper obj = (ContentFillHelper)target;
        if (GUILayout.Button("Populate UI"))
        {
            obj.ReloadObjects();
        }
    }
}
