using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomBuilding.EnemySpawner))]
public class EnemySpawner_GUI : Editor
{
    public override void OnInspectorGUI()
    {
        RoomBuilding.EnemySpawner scr = target as RoomBuilding.EnemySpawner;

        scr.size = EditorGUILayout.Vector3Field("Size", scr.size);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Debugging", EditorStyles.boldLabel);
        scr.debugging = EditorGUILayout.Toggle("Enabled", scr.debugging);

        // Only show debug menu when debugging
        if (scr.debugging)
        {
            scr.OGEnemies = EditorGUILayout.Toggle("OG Enemies", scr.OGEnemies);

            SerializedProperty debugBehaviour = serializedObject.FindProperty("debugBehaviour");
            EditorGUILayout.PropertyField(debugBehaviour);
            serializedObject.ApplyModifiedProperties();
        }
    }

}