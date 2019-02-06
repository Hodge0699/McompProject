using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomBuilding
{
    public class RoomBuilder : MonoBehaviour
    {
        public enum Direction { NORTH, EAST, SOUTH, WEST, ERROR };

        // Liam: setting floor texture for generated room floor
        public Texture floorTexture; // Liam

        public Vector3 dimensions = new Vector3(25.0f, 5.0f, 20.0f); // Dimensions of the room

        // Manual build button
        public bool build = false;

        [Header("Doors")]
        public float doorSize = 6.0f;
        public float doorSpeed = 2.0f;

        [Header("Walls")]
        public float wallThickness = 1.0f;
        // NONE = 2D, SEMI = north and east walls 3D, ALL = all walls 3D
        public preview previewWalls = preview.SEMI;
        public enum preview { NONE, SEMI, ALL };

        // Which walls should have doors?
        public enum wallType { SOLID, DOORWAY, DOOR };
        public wallType westWall;
        public wallType eastWall;
        public wallType northWall;
        public wallType southWall;

        private void Start()
        {
            startNewRoom();
        }
        
        /// <summary>
        /// Resets all wall values to solid.
        /// </summary>
        public void startNewRoom()
        {
            westWall = wallType.SOLID;
            eastWall = wallType.SOLID;
            northWall = wallType.SOLID;
            southWall = wallType.SOLID;
        }

        /// <summary>
        /// Allows manual use of build button.
        /// </summary>
        private void Update()
        {
            if (build)
            {
                buildRoom();
                build = false;
            }
        }

        /// <summary>
        /// Gets the type of a specific wall.
        /// </summary>
        /// <param name="dir">The direction of the wall from the room's origin.</param>
        /// <returns>Type of wall (SOLID, DOORWAY, DOOR).</returns>
        public wallType getWallType(Direction dir)
        {
            switch (dir)
            {
                case Direction.NORTH:
                    return northWall;
                case Direction.EAST:
                    return eastWall;
                case Direction.SOUTH:
                    return southWall;
                case Direction.WEST:
                    return westWall;
                default:
                    Debug.LogError("Invalid Dir!");
                    return wallType.SOLID;
            }
        }

        /// <summary>
        /// Gets the type of a specific wall.
        /// </summary>
        /// <param name="dir">The direction of the wall from the room's origin.</param>
        /// <param name="type">Type of wall (SOLID, DOORWAY, DOOR).</param>
        public void setWallType(Direction dir, wallType type)
        {
            switch (dir)
            {
                case Direction.NORTH:
                    northWall = type;
                    break;
                case Direction.EAST:
                     eastWall = type;
                    break;
                case Direction.SOUTH:
                    southWall = type;
                    break;
                case Direction.WEST:
                    westWall = type;
                    break;
                default:
                    Debug.LogError("Invalid Dir!");
                    break;
            }
        }

        /// <summary>
        /// Builds physical room
        /// </summary>
        public Room buildRoom()
        {
            // Create parent object
            GameObject roomOrigin = new GameObject();
            roomOrigin.name = "Room";
            roomOrigin.transform.position = transform.position;

            Room room = roomOrigin.AddComponent<Room>();
            room.dimensions = dimensions;

            // Create floor
            GameObject floor = instantiateCube("Floor", roomOrigin.transform, new Vector3(0.0f, -0.25f, 0.0f), new Vector3(dimensions.x, 0.5f, dimensions.z));
            // Liam - applying set texture to the floor on generation
            floor.GetComponent<Renderer>().material.mainTexture = floorTexture; // Liam

            buildWalls(roomOrigin.transform);
            room.setDoors(buildDoors(roomOrigin.transform));
            room.createGeometryShaders();

            return room;
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
            walls.transform.localPosition = Vector3.zero;
            // West
            if (westWall == wallType.SOLID)
                instantiateCube("Left Wall", walls.transform, new Vector3(-(dimensions.x / 2) + (wallThickness / 2), dimensions.y / 2, 0.0f), new Vector3(dimensions.z, dimensions.y, wallThickness), 270);
            else
            {
                float newWallSize = (dimensions.z - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Left Wall (Above door)", walls.transform, new Vector3(-(dimensions.x / 2) + (wallThickness / 2), dimensions.y / 2, offset), new Vector3(newWallSize, dimensions.y, wallThickness), 270);
                instantiateCube("Left Wall (Under door)", walls.transform, new Vector3(-(dimensions.x / 2) + (wallThickness / 2), dimensions.y / 2, -offset), new Vector3(newWallSize, dimensions.y, wallThickness), 270);
            }


            // East
            if (eastWall == wallType.SOLID)
                instantiateCube("Right Wall", walls.transform, new Vector3((dimensions.x / 2) - (wallThickness / 2), dimensions.y / 2, 0.0f), new Vector3(dimensions.z, dimensions.y, wallThickness), 90);
            else
            {
                float newWallSize = (dimensions.z - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Right Wall (Above door)", walls.transform, new Vector3((dimensions.x / 2) - (wallThickness / 2), dimensions.y / 2, offset), new Vector3(newWallSize, dimensions.y, wallThickness), 90);
                instantiateCube("Right Wall (Under door)", walls.transform, new Vector3((dimensions.x / 2) - (wallThickness / 2), dimensions.y / 2, -offset), new Vector3(newWallSize, dimensions.y, wallThickness), 90);
            }


            // North
            if (northWall == wallType.SOLID)
                instantiateCube("Top Wall", walls.transform, new Vector3(0.0f, dimensions.y / 2, (dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness));
            else
            {
                float newWallSize = (dimensions.x - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Top Wall (Left of door)", walls.transform, new Vector3(-offset, dimensions.y / 2, (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
                instantiateCube("Top Wall (Right of door)", walls.transform, new Vector3(offset, dimensions.y / 2, (dimensions.z / 2) - (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness));
            }


            // South
            if (southWall == wallType.SOLID)
                instantiateCube("Bottom Wall", walls.transform, new Vector3(0.0f, dimensions.y / 2, -(dimensions.z / 2) - (wallThickness / 2)), new Vector3(dimensions.x, dimensions.y, wallThickness), 180);
            else
            {
                float newWallSize = (dimensions.x - doorSize) / 2;
                float offset = (newWallSize / 2) + (doorSize / 2);

                instantiateCube("Bottom Wall (Left of door)", walls.transform, new Vector3(-offset, dimensions.y / 2, -(dimensions.z / 2) + (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness), 180);
                instantiateCube("Bottom Wall (Right of door)", walls.transform, new Vector3(offset, dimensions.y / 2, -(dimensions.z / 2) + (wallThickness / 2)), new Vector3(newWallSize, dimensions.y, wallThickness), 180);
            }
        }

        /// <summary>
        /// Places doors into the room
        /// </summary>
        /// <param name="parent">The parent transform to arrange doors around.</param>
        /// <returns>A list of all doors created.</returns>
        private List<DoorController> buildDoors(Transform parent)
        {
            GameObject doorsParent = new GameObject();
            doorsParent.name = "Doors";
            doorsParent.transform.parent = parent;
            doorsParent.transform.localPosition = Vector3.zero;
            List<DoorController> doors = new List<DoorController>();

            if (westWall == wallType.DOOR)
            {
                GameObject door = instantiateDoor("West Wall Door", doorsParent.transform, new Vector3(-(dimensions.x / 2) + (wallThickness / 2), dimensions.y / 2, 0.0f));
                door.transform.Rotate(Vector3.up, -90.0f);
                doors.Add(door.GetComponent<DoorController>());
            }

            if (eastWall == wallType.DOOR)
            {
                GameObject door = instantiateDoor("East Wall Door", doorsParent.transform, new Vector3((dimensions.x / 2) - (wallThickness / 2), dimensions.y / 2, 0.0f));
                door.transform.Rotate(Vector3.up, 90.0f);
                doors.Add(door.GetComponent<DoorController>());
            }

            if (northWall == wallType.DOOR)
            {
                GameObject door = instantiateDoor("North Wall Door", doorsParent.transform, new Vector3(0.0f, dimensions.y / 2, (dimensions.z / 2) - (wallThickness / 2)));
                doors.Add(door.GetComponent<DoorController>());
            }
            if (southWall == wallType.DOOR)
            {
                GameObject door = instantiateDoor("South Wall Door", doorsParent.transform, new Vector3(0.0f, dimensions.y / 2, -(dimensions.z / 2) + (wallThickness / 2)));
                door.transform.Rotate(Vector3.up, 180.0f);

                doors.Add(door.GetComponent<DoorController>());
            }

            return doors;
        }

        /// <summary>
        /// Instantiates a cube in the level.
        /// </summary>
        /// <param name="parent">The parent object to attach this cube to.</param>
        /// <param name="localPosition">Position of this cube in relation to parent.</param>
        /// <param name="localScale">Scale of this cube in relation to parent.</param>
        /// <returns></returns>
        private GameObject instantiateCube(string name, Transform parent, Vector3 localPosition, Vector3 localScale, float rotateDegrees = 0.0f)
        {
            GameObject cube = Instantiate(Resources.Load("Room Components\\Cube")) as GameObject;
            cube.transform.name = name;
            cube.transform.parent = parent;
            cube.transform.localPosition = localPosition;
            cube.transform.localScale = localScale;

            cube.transform.Rotate(Vector3.up, rotateDegrees);

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