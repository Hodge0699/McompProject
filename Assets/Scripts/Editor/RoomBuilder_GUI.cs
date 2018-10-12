using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(RoomBuilder))]
public class RoomBuilder_GUI : Editor {

    private RoomBuilder room; // The planned room to draw.
    private Vector3 origin; // The Centre of the room.

    private Color defaultColour = Color.white;
    private Color doorwayColour = Color.cyan;
    private Color doorColour = Color.green;

    private float semiWallsPreview = 0.0f; // Acts as boolean whether to draw top and right walls.
    private float allWallsPreview = 0.0f; // Acts as boolean whether to draw bottom and left walls.

    private void OnSceneGUI()
    {
        room = (RoomBuilder)target;
        origin = room.transform.position;

        Handles.color = defaultColour;

        Handles.DrawWireCube(origin, new Vector3(room.dimensions.x, 0.0f, room.dimensions.z));

        Handles.SphereHandleCap(0, origin, Quaternion.Euler(0.0f, 0.0f, 0.0f), 1, EventType.Repaint);

        string previewWalls = room.previewWalls.ToString();

        switch (previewWalls)
        {
            case ("SEMI"):
                semiWallsPreview = 1.0f;
                allWallsPreview = 0.0f;
                break;
            case ("ALL"):
                semiWallsPreview = 1.0f;
                allWallsPreview = 1.0f;
                break;
            default:
                semiWallsPreview = 0.0f;
                allWallsPreview = 0.0f;
                break;
        }


        drawWalls();
        drawDoors();
    }

    /// <summary>
    /// Draws wireframe walls.
    /// </summary>
    private void drawWalls()
    {
        //                    _______________________________________________________________________________________________________________________________________________________________________________________________________      _____________________________________________________________________________________________
        //                   || Origin ||                        X                                     |                           Y                               |                                     Z                           |    ||  Size  ||         X         |                     Y                  |            Z        |
        Handles.DrawWireCube(new Vector3(origin.x - (room.dimensions.x / 2) + (room.wallThickness / 2),    origin.y + ((room.dimensions.y / 2) * allWallsPreview),    origin.z),                                                          new Vector3(room.wallThickness,    room.dimensions.y * allWallsPreview,     room.dimensions.z)); // Left
        Handles.DrawWireCube(new Vector3(origin.x + (room.dimensions.x / 2) - (room.wallThickness / 2),    origin.y + ((room.dimensions.y / 2) * semiWallsPreview),   origin.z),                                                          new Vector3(room.wallThickness,    room.dimensions.y * semiWallsPreview,    room.dimensions.z)); // Right

        Handles.DrawWireCube(new Vector3(origin.x,                                                         origin.y + ((room.dimensions.y / 2) * allWallsPreview),    origin.z - (room.dimensions.z / 2) + (room.wallThickness / 2)),     new Vector3(room.dimensions.x,     room.dimensions.y * allWallsPreview,     room.wallThickness)); // Bottom
        Handles.DrawWireCube(new Vector3(origin.x,                                                         origin.y + ((room.dimensions.y / 2) * semiWallsPreview),   origin.z + (room.dimensions.z / 2) - (room.wallThickness / 2)),     new Vector3(room.dimensions.x,     room.dimensions.y * semiWallsPreview,    room.wallThickness)); // Top
    }

    /// <summary>
    /// Draws wireframe doors
    /// </summary>
    private void drawDoors()
    {
        //                        _______________________________________________________________________________________________________________________________________________________________________________________________________     ______________________________________________________________________________________________
        //                       || Origin ||                            X                                 |                            Y                             |                                     Z                            |    ||  Size  ||         X         |                     Y                  |            Z        |
        if (room.leftWall != RoomBuilder.wallType.SOLID)
        {
            if (room.leftWall == RoomBuilder.wallType.DOOR)
                Handles.color = doorColour;
            else if (room.leftWall == RoomBuilder.wallType.DOORWAY)
                Handles.color = doorwayColour;

            Handles.DrawWireCube(new Vector3(origin.x - (room.dimensions.x / 2) + (room.wallThickness / 2),    origin.y + ((room.dimensions.y / 2) * allWallsPreview),    origin.z),                                                          new Vector3(room.wallThickness,    room.dimensions.y * allWallsPreview,     room.doorSize));

        }

        if (room.rightWall != RoomBuilder.wallType.SOLID)
        {
            if (room.rightWall == RoomBuilder.wallType.DOOR)
                Handles.color = doorColour;
            else if (room.rightWall == RoomBuilder.wallType.DOORWAY)
                Handles.color = doorwayColour;

            Handles.DrawWireCube(new Vector3(origin.x + (room.dimensions.x / 2) - (room.wallThickness / 2),    origin.y + ((room.dimensions.y / 2) * semiWallsPreview),   origin.z),                                                          new Vector3(room.wallThickness,    room.dimensions.y * semiWallsPreview,    room.doorSize)); 
        }

        if (room.topWall != RoomBuilder.wallType.SOLID)
        {
            if (room.topWall == RoomBuilder.wallType.DOOR)
                Handles.color = doorColour;
            else if (room.topWall == RoomBuilder.wallType.DOORWAY)
                Handles.color = doorwayColour;

            Handles.DrawWireCube(new Vector3(origin.x,                                                         origin.y + ((room.dimensions.y / 2) * semiWallsPreview),   origin.z + (room.dimensions.z / 2) - (room.wallThickness / 2)),     new Vector3(room.doorSize,         room.dimensions.y * semiWallsPreview,    room.wallThickness));
        }

        if (room.bottomWall != RoomBuilder.wallType.SOLID)
        {
            if (room.bottomWall == RoomBuilder.wallType.DOOR)
                Handles.color = doorColour;
            else if (room.bottomWall == RoomBuilder.wallType.DOORWAY)
                Handles.color = doorwayColour;

            Handles.DrawWireCube(new Vector3(origin.x,                                                         origin.y + ((room.dimensions.y / 2) * allWallsPreview),    origin.z - (room.dimensions.z / 2) + (room.wallThickness / 2)),     new Vector3(room.doorSize,         room.dimensions.y * allWallsPreview,     room.wallThickness));
        }

        Handles.color = defaultColour;
    }
}
