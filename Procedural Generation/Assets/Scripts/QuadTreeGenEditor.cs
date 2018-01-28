using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(QuadTreeGen))]
public class QuadTreeGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuadTreeGen gen = (QuadTreeGen)target;
        if(GUILayout.Button("Rebuild Area"))
        {
            gen.Rebuild();
        }
    }
}
