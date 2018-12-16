using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestMyInpector))]
public class TestMyInpectorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        TestMyInpector myTarget = (TestMyInpector)target;
        
        EditorGUILayout.HelpBox("MyInspector", MessageType.Warning);
        myTarget.Color = EditorGUILayout.ColorField("Color", myTarget.Color);
        myTarget.Int = EditorGUILayout.IntField("Int", myTarget.Int);
        myTarget.String = EditorGUILayout.TextField("String", myTarget.String);
    }
}
