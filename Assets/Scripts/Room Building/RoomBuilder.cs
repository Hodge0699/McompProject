﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomBuilding
{
    public class RoomBuilder : MonoBehaviour
    {
        public Vector3 dimensions = new Vector3(25.0f, 5.0f, 20.0f); // Dimensions of the room
        private Vector3 origin;

        public float wallThickness = 1.0f;
        public float doorSize = 6.0f;
        public float doorSpeed = 2.0f;

        // NONE = 2D, SEMI = north and east walls 3D, ALL = all walls 3D
        public preview previewWalls = preview.SEMI;
        public enum preview { NONE, SEMI, ALL };

        // Which walls should have doors?
        public enum wallType { SOLID, DOORWAY, DOOR };
        public wallType westWall;
        public wallType eastWall;
        public wallType northWall;
        public wallType southWall;


        // Manual build button
        public bool build = false;

        private void Start()
        {
            init();
        }

        public void init()
        {
            westWall = wallType.SOLID;
            eastWall = wallType.SOLID;
            northWall = wallType.SOLID;
            southWall = wallType.SOLID;

            origin = Vector3.zero;
    }

        private void Update()
        {
            if (build)
            {
                buildRoom();
                build = false;
            }
        }

        /// <summary>
        /// Builds physical room
        /// </summary>
        public void buildRoom()
        {
            origin = transform.position;

            // Create parent object
            GameObject roomOrigin = new GameObject();
            roomOrigin.name = "Room";
            roomOrigin.transform.position = origin;

            // Create floor
            instantiateCube("Floor", roomOrigin.transform, new Vector3(0.0f, -0.25f, 0.0f), new Vector3(dimensions.x, 0.5f, dimensions.z));

            buildWalls(roomOrigin.transform);
            buildDoors(roomOrigin.transform);
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

            // West
            if (westWall == wallType.SOLID)
                instantiateCube("Left Wall", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z), new Vector3(wallThickness, dimensions.y, dimensions.z));
            else
            {
                float newWallSize = (dimensions.z - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Left Wall (Above door)", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z + offset), new Vector3(wallThickness, dimensions.y, newWallSize));
                instantiateCube("Left Wall (Under door)", walls.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z - offset), new Vector3(wallThickness, dimensions.y, newWallSize));
            }


            // East
            if (eastWall == wallType.SOLID)
                instantiateCube("Right Wall", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z), new Vector3(wallThickness, dimensions.y, dimensions.z));
            else
            {
                float newWallSize = (dimensions.z - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Right Wall (Above door)", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z + offset), new Vector3(wallThickness, dimensions.y, newWallSize));
                instantiateCube("Right Wall (Under door)", walls.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z - offset), new Vector3(wallThickness, dimensions.y, newWallSize));
            }


            // North
            if (northWall == wallType.SOLID)
                instantiateCube("Top Wall", walls.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));
            else
            {
                float newWallSize = (dimensions.x - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Top Wall (Left of door)", walls.transform, new Vector3(origin.x - offset, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
                instantiateCube("Top Wall (Right of door)", walls.transform, new Vector3(origin.x + offset, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            }


            // South
            if (southWall == wallType.SOLID)
                instantiateCube("Bottom Wall", walls.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));
            else
            {
                float newWallSize = (dimensions.x - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Bottom Wall (Left of door)", walls.transform, new Vector3(origin.x - offset, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) + (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
                instantiateCube("Bottom Wall (Right of door)", walls.transform, new Vector3(origin.x + offset, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) + (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            }
        }

        /// <summary>
        /// Places doors into the room
        /// </summary>
        /// <param name="parent">The parent transform to arrange doors around.</param>
        /// <returns>A list of all doors created.</returns>
        private void buildDoors(Transform parent)
        {
            GameObject doorsParent = new GameObject();
            doorsParent.name = "Doors";
            doorsParent.transform.parent = parent;

            if (westWall == wallType.DOOR)
                instantiateDoor("West Wall Door", doorsParent.transform, new Vector3(origin.x - (dimensions.x / 2) + (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z)).transform.Rotate(0.0f, -90.0f, 0.0f);

            if (eastWall == wallType.DOOR)
                instantiateDoor("East Wall Door", doorsParent.transform, new Vector3(origin.x + (dimensions.x / 2) - (wallThickness / 2), origin.y + (dimensions.y / 2), origin.z)).transform.Rotate(0.0f, 90.0f, 0.0f);

            if (northWall == wallType.DOOR)
                instantiateDoor("North Wall Door", doorsParent.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z + (dimensions.z / 2) - (wallThickness / 2)));

            if (southWall == wallType.DOOR)
                instantiateDoor("South Wall Door", doorsParent.transform, new Vector3(origin.x, origin.y + (dimensions.y / 2), origin.z - (dimensions.z / 2) + (wallThickness / 2)));
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
        /// <returns>Door game object</returns>
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
}