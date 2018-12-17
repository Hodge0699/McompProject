using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class Room_GUI : Editor
{
    private void OnSceneGUI()
    {
        Room room = (Room)target;

        Handles.color = Color.blue;

        Vector3 origin = room.transform.position;
        origin.y += room.dimensions.y / 2;

        Handles.DrawWireCube(origin, room.dimensions);
    }
}
