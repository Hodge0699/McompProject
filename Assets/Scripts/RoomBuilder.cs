using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour {

    public Vector3 dimensions = new Vector3(25.0f, 5.0f, 20.0f); // Dimensions of the room
    private Vector3 origin;

    public float wallThickness = 1.0f;
    public float doorSize = 6.0f;

    // NONE = 2D, SEMI = top and right walls 3D, ALL = all walls 3D
    public preview previewWalls = preview.SEMI;
    public enum preview { NONE, SEMI, ALL }; 

    // Which walls should have doors?
    public bool doorLeft = false;
    public bool doorTop = false;
    public bool doorRight = false;
    public bool doorBottom = false;

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
        GameObject floor = instantiateCube("Floor", room.transform, new Vector3(0.0f, -0.25f, 0.0f), new Vector3(dimensions.x, 0.5f, dimensions.z));

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
        if (doorLeft)
        {
            float newWallSize = (dimensions.z - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Left Wall (Above door)", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z + offset), new Vector3(wallThickness, dimensions.y, newWallSize));
            instantiateCube("Left Wall (Under door)", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z - offset), new Vector3(wallThickness, dimensions.y, newWallSize));
        }
        else
            instantiateCube("Left Wall", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z), new Vector3(wallThickness, dimensions.y, dimensions.z));


        // Right
        if (doorLeft)
        {
            float newWallSize = (dimensions.z - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Right Wall (Above door)", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z + offset), new Vector3(wallThickness, dimensions.y, newWallSize));
            instantiateCube("Right Wall (Under door)", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z - offset), new Vector3(wallThickness, dimensions.y, newWallSize));
        }
        else
            instantiateCube("Right Wall", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z), new Vector3(wallThickness, dimensions.y, dimensions.z));


        // Top
        if (doorTop)
        {
            float newWallSize = (dimensions.x - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Top Wall (Left of door)",  walls.transform, new Vector3(origin.x - offset, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            instantiateCube("Top Wall (Right of door)", walls.transform, new Vector3(origin.x + offset, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
        }
        else
            instantiateCube("Top Wall", walls.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));


        // Bottom
        if (doorBottom)
        {
            float newWallSize = (dimensions.x - doorSize) / 2;
            float offset = (newWallSize / 2) + (doorSize / 2);

            instantiateCube("Bottom Wall (Left of door)", walls.transform, new Vector3(origin.x - offset, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            instantiateCube("Bottom Wall (Right of door)", walls.transform, new Vector3(origin.x + offset, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
        }
        else
            instantiateCube("Bottom Wall", walls.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));
    }

    /// <summary>
    /// Places doors into the room
    /// </summary>
    /// <param name="parent">The parent transform to arrange doors around.</param>
    private void buildDoors(Transform parent)
    {

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
}
