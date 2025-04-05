/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  

[CustomEditor(typeof(LevelManager))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager myScript = (LevelManager)target;
        if (GUILayout.Button("Saveobj"))
        {
            myScript.SaveObj();
        }
        if (GUILayout.Button("Save Layout"))
        {
            myScript.SavePos();
        }
    }
}*/