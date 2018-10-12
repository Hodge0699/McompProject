using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour {

    public Vector3 dimensions = new Vector3(25.0f, 5.0f, 20.0f); // Dimensions of the room
    private Vector3 origin;

    public float wallThickness = 1.0f;
    public float doorSize = 6.0f;
    public float doorSpeed = 2.0f;

    // NONE = 2D, SEMI = top and right walls 3D, ALL = all walls 3D
    public preview previewWalls = preview.SEMI;
    public enum preview { NONE, SEMI, ALL };

    // Which walls should have doors?
    public enum wallType { SOLID, DOORWAY, DOOR };
    public wallType leftWall;
    public wallType rightWall;
    public wallType topWall;
    public wallType bottomWall;


    // Manual build button
    public bool build = false;

    private void Update()
    {
        if (build)
        {
            buildRoom();
            build = false;
        }
    }

    /// <summary>
    /// Builds entire physical room
    /// </summary>
    public void buildRoom()
    {
        origin = transform.position;

        // Create parent object
        GameObject room = new GameObject();
        room.name = "Room";
        room.transform.position = origin;

        // Create floor
        instantiateCube("Floor", room.transform, new Vector3(0.0f, -0.25f, 0.0f), new Vector3(dimensions.x, 0.5f, dimensions.z));

        buildWalls(room.transform);
        buildDoors(room.transform);
    }

    /// <summary>
    /// Places walls around room
    /// </summary>
    /// <param name="parent">The parent transform to arrange walls around.</param>
    private void buildWalls(Transform parent)
    {
        GameObject walls = new GameObject();
        walls.name = "Walls";
        walls.transform.parent = parent;

        // Left
        if (leftWall == wallType.SOLID)
            instantiateCube("Left Wall", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z), new Vector3(wallThickness, dimensions.y, dimensions.z));
        else
        {
            float newWallSize = (dimensions.z - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Left Wall (Above door)", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z + offset), new Vector3(wallThickness, dimensions.y, newWallSize));
            instantiateCube("Left Wall (Under door)", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z - offset), new Vector3(wallThickness, dimensions.y, newWallSize));
        }


        // Right
        if (rightWall == wallType.SOLID)
            instantiateCube("Right Wall", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z), new Vector3(wallThickness, dimensions.y, dimensions.z));
        else
        {
            float newWallSize = (dimensions.z - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Right Wall (Above door)", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z + offset), new Vector3(wallThickness, dimensions.y, newWallSize));
            instantiateCube("Right Wall (Under door)", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z - offset), new Vector3(wallThickness, dimensions.y, newWallSize));
        }


        // Top
        if (topWall == wallType.SOLID)
            instantiateCube("Top Wall", walls.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));
        else
        {
            float newWallSize = (dimensions.x - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Top Wall (Left of door)",  walls.transform, new Vector3(origin.x - offset, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            instantiateCube("Top Wall (Right of door)", walls.transform, new Vector3(origin.x + offset, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
        }


        // Bottom
        if (bottomWall == wallType.SOLID)
            instantiateCube("Bottom Wall", walls.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));
        else
        {
            float newWallSize = (dimensions.x - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Bottom Wall (Left of door)",  walls.transform, new Vector3(origin.x - offset, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) + (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            instantiateCube("Bottom Wall (Right of door)", walls.transform, new Vector3(origin.x + offset, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) + (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
        }
    }

    /// <summary>
    /// Places doors into the room
    /// </summary>
    /// <param name="parent">The parent transform to arrange doors around.</param>
    private void buildDoors(Transform parent)
    {
        GameObject doors = new GameObject();
        doors.name = "Doors";
        doors.transform.parent = parent;

        if (leftWall == wallType.DOOR)
            instantiateDoor("Left Wall Door", doors.transform,    new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2),    origin.y + (dimensions.y / 2),    origin.z)).transform.Rotate(0.0f, -90.0f, 0.0f);

        if (rightWall == wallType.DOOR)
            instantiateDoor("Right Wall Door", doors.transform,   new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2),    origin.y + (dimensions.y / 2),    origin.z)).transform.Rotate(0.0f, 90.0f, 0.0f);

        if (topWall == wallType.DOOR)
            instantiateDoor("Top Wall Door", doors.transform,     new Vector3(origin.x,                                               origin.y + (dimensions.y / 2),    origin.z + (dimensions.z / 2) - (wallThickness / 2)));

        if (bottomWall == wallType.DOOR)
            instantiateDoor("Bottom Wall Door", doors.transform,  new Vector3(origin.x,                                               origin.y + (dimensions.y / 2),    origin.z - (dimensions.z / 2) + (wallThickness / 2)));

    }

    /// <summary>
    /// Instantiates a cube in the level.
    /// </summary>
    /// <param name="parent">The parent object to attach this cube to.</param>
    /// <param name="localPosition">Position of this cube in relation to parent.</param>
    /// <param name="localScale">Scale of this cube in relation to parent.</param>
    /// <returns></returns>
    private GameObject instantiateCube(string name, Transform parent, Vector3 localPosition, Vector3 localScale)
    {
        GameObject cube = Instantiate(Resources.Load("Room Components\\Cube")) as GameObject;
        cube.transform.name = name;
        cube.transform.parent = parent;
        cube.transform.localPosition = localPosition;
        cube.transform.localScale = localScale;

        return cube;
    }

    /// <summary>
    /// Instantiates a cube in the level.
    /// </summary>
    /// <param name="parent">The parent object to attach this cube to.</param>
    /// <param name="localPosition">Position of this cube in relation to parent.</param>
    /// <returns></returns>
    private GameObject instantiateDoor(string name, Transform parent, Vector3 localPosition)
    {
        GameObject door = Instantiate(Resources.Load("Room Components\\Door")) as GameObject;
        door.transform.name = name;
        door.transform.parent = parent;
        door.transform.localPosition = localPosition;

        door.GetComponent<DoorController>().init(doorSize, dimensions.y, wallThickness * 0.9f, doorSpeed);

        return door;
    }
}
