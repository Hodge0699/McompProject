using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoomBuilding
{
    public class ProceduralRoomGeneration : MonoBehaviour
    {
        public Vector2 maxRoomSize = new Vector2(25.0f, 40.0f);
        public Vector2 minRoomSize = new Vector2(15.0f, 30.0f);

        private RoomBuilder rb;

        private Queue<Room> rooms = new Queue<Room>(); // List of all active rooms (should be 1 max).

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<RoomBuilder>();

            createRoom();

            // Liam - Spawn player at start of scene when first room is generated
            GameObject player = Instantiate(Resources.Load("Player")) as GameObject; // Liam
            player.transform.position = Vector3.zero; // Liam

            Camera minimapCamera = Instantiate(Resources.Load("MinimapCamera")) as Camera;
            Image minimapBoarder = Instantiate(Resources.Load("MinimapBoarder")) as Image;
        }

        /// <summary>
        /// Creates a new room either completely randomly or based off a previous room.
        /// </summary>
        public void createRoom()
        {
            rb.startNewRoom();

            Room lastRoom = null;
            
            if (rooms.Count > 0)
                lastRoom = rooms.Dequeue();

            // Randomize new room size
            rb.dimensions = new Vector3(Random.Range(minRoomSize.x, maxRoomSize.x), 5.0f, Random.Range(minRoomSize.y, maxRoomSize.y));


            // Calculate new room origin
            Vector3 roomOrigin;
            if (lastRoom != null)
            {
                Vector3 translationAxis = new Vector3(lastRoom.exit.transform.localPosition.x, 0.0f, lastRoom.exit.transform.localPosition.z).normalized;

                roomOrigin = rb.dimensions / 2;

                for (int i = 0; i < 3; i++)
                {
                    roomOrigin[i] *= translationAxis[i];
                    roomOrigin[i] -= translationAxis[i] * (rb.wallThickness / 2);
                }

                roomOrigin += lastRoom.exit.transform.position;

                roomOrigin.y = 0.0f;
            }
            else
                roomOrigin = Vector3.zero;

            rb.transform.position = roomOrigin;


            // Generate doors
            int maxNewDoors;

            if (lastRoom != null)
            {
                maxNewDoors = 3;

                Vector3 translationAxis = new Vector3(lastRoom.exit.transform.localPosition.x, 0.0f, lastRoom.transform.localPosition.z).normalized;

                if (lastRoom.exit.transform.localPosition.z > 0.0f) // Exit was north, entrance will be south
                    rb.southWall = RoomBuilder.wallType.DOORWAY;
                else if (lastRoom.exit.transform.localPosition.x > 0.0f) // Exit was east, entrance will be west
                    rb.westWall = RoomBuilder.wallType.DOORWAY;
                else if (lastRoom.exit.transform.localPosition.z < 0.0f) // Exit was south, entrance will be north
                    rb.northWall = RoomBuilder.wallType.DOORWAY;
                else if (lastRoom.exit.transform.localPosition.x < 0.0f) // Exit was west, entrance will be east
                    rb.eastWall = RoomBuilder.wallType.DOORWAY;
                else
                    Debug.LogError("Entrance direction not found!");
            }
            else
                maxNewDoors = 4;

            int doorsToGenerate = Random.Range(2, maxNewDoors);

            for (int i = 0; i < doorsToGenerate; i++)
            {
                Direction dir = getRandomDirection();

                if (rb.getWallType(dir) != RoomBuilder.wallType.DOORWAY)
                    rb.setWallType(dir, RoomBuilder.wallType.DOOR);
            }

            // Physically build room and despawn old room.
            Room newRoom = rb.buildRoom();

            if (lastRoom)
                lastRoom.despawn(newRoom);

            rooms.Enqueue(newRoom);
        }

        /// <summary>
        /// Randomises a direction.
        /// </summary>
        Direction getRandomDirection()
        {
            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    return Direction.NORTH;
                case 1:
                    return Direction.EAST;
                case 2:
                    return Direction.SOUTH;
                case 3:
                    return Direction.WEST;
                default:
                    return Direction.ERROR;
            }
        }

    }
}