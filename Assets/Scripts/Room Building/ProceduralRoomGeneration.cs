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

        public int enemiesPer1000UnitsSqrd = 10;

        private RoomBuilder rb;
        private Enemy.EnemiesSpawn enemySpawner;

        private Queue<Room> rooms = new Queue<Room>(); // List of all active rooms (should be 1 max).

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<RoomBuilder>();
            enemySpawner = GetComponent<Enemy.EnemiesSpawn>();

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
            rb.transform.position = calculateRoomOrigin(lastRoom);

            enemySpawner.size = new Vector3(rb.dimensions.x - (rb.wallThickness * 4), 0.0f, rb.dimensions.z - (rb.wallThickness * 4));
            enemySpawner.center = rb.transform.position;
            enemySpawner.center.y += 1.0f;

            // Generate doors
            generateDoors(lastRoom);

            // Physically build room and despawn old room.
            Room newRoom = rb.buildRoom();

            if (lastRoom)
                lastRoom.despawn(newRoom);


            spawnEnemies(newRoom);

            rooms.Enqueue(newRoom);
        }

        /// <summary>
        /// Calculates the origin of the next room to be built.
        /// </summary>
        /// <param name="lastRoom">The last room that was built (or null if this is first room).</param>
        /// <returns>A suitable origin based on the last room and current dimensions of this room.</returns>
        private Vector3 calculateRoomOrigin(Room lastRoom)
        {
            if (lastRoom == null)
                return Vector3.zero;

            Vector3 translationAxis = new Vector3(lastRoom.exit.transform.localPosition.x, 0.0f, lastRoom.exit.transform.localPosition.z).normalized;

            Vector3 roomOrigin = rb.dimensions / 2;

            for (int i = 0; i < 3; i++)
            {
                roomOrigin[i] *= translationAxis[i];
                roomOrigin[i] -= translationAxis[i] * (rb.wallThickness / 2);
            }

            roomOrigin += lastRoom.exit.transform.position;

            roomOrigin.y = 0.0f;

            return roomOrigin;
        }

        /// <summary>
        /// Randomises doors and sends them to the room builder.
        /// </summary>
        /// <param name="lastRoom">The previous room in order to determine which direction needs an empty doorway.</param>
        private void generateDoors(Room lastRoom)
        {
            int minNewDoors = 1;
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

            int doorCount = Random.Range(minNewDoors, maxNewDoors + 1);

            for (int i = 0; i < doorCount; i++)
            {
                Direction dir;
                do
                    dir = getRandomDirection();
                while (rb.getWallType(dir) != RoomBuilder.wallType.SOLID);
                
                rb.setWallType(dir, RoomBuilder.wallType.DOOR);
            }
        }

        private void spawnEnemies(Room room)
        {
            int roomSizeUnitsSqrt = (int)(room.dimensions.x * room.dimensions.z);
            int enemyCount = (roomSizeUnitsSqrt / 1000) * enemiesPer1000UnitsSqrd;

            for (int i = 0; i < enemyCount; i++)
                room.addEnemy(enemySpawner.Spawn().GetComponent<EnemyController>());
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