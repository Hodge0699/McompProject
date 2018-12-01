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

        private PlayerController player;

        private float minEnemyDistance = 7.5f; // How far away from the players must the enemies spawn.

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<RoomBuilder>();
            enemySpawner = GetComponent<Enemy.EnemiesSpawn>();

            // Liam - Spawn player at start of scene when first room is generated
            GameObject playerObj = Instantiate(Resources.Load("Player")) as GameObject; // Liam
            playerObj.transform.position = Vector3.zero; // Liam

            player = playerObj.GetComponent<PlayerController>();

            Room startRoom = createRoom(null, null);

            player.myCamera.GetComponent<CameraController>().setRoom(startRoom);

            //for (int i = 0; i < startRoom.doors.Count; i++)
            //    startRoom.addChildRoom(createRoom(startRoom, startRoom.doors[i]));

            Camera minimapCamera = Instantiate(Resources.Load("MinimapCamera")) as Camera;
            Image minimapBoarder = Instantiate(Resources.Load("MinimapBoarder")) as Image;
        }

        /// <summary>
        /// Creates a new room either completely randomly or based off a previous room.
        /// </summary>
        public Room createRoom(Room lastRoom, DoorController connectingDoor)
        {
            rb.startNewRoom();

            // Randomize new room size
            rb.dimensions = new Vector3(Random.Range(minRoomSize.x, maxRoomSize.x), 5.0f, Random.Range(minRoomSize.y, maxRoomSize.y));

            // Calculate new room origin
            rb.transform.position = calculateRoomOrigin(lastRoom, connectingDoor);

            // Create doors
            generateDoors(lastRoom, connectingDoor);

            // Physically build room
            Room newRoom = rb.buildRoom();

            enemySpawner.size = new Vector3(rb.dimensions.x - (rb.wallThickness * 4), 0.0f, rb.dimensions.z - (rb.wallThickness * 4));
            enemySpawner.center = rb.transform.position;
            enemySpawner.center.y += 1.0f;
            spawnEnemies(newRoom);

            return newRoom;
        }

        /// <summary>
        /// Calculates the origin of the next room to be built.
        /// </summary>
        /// <param name="lastRoom">The last room that was built (or null if this is first room).</param>
        /// <returns>A suitable origin based on the last room and current dimensions of this room.</returns>
        private Vector3 calculateRoomOrigin(Room lastRoom, DoorController connectingDoor)
        {
            if (lastRoom == null)
                return Vector3.zero;

            Vector3 translationAxis = new Vector3(connectingDoor.transform.localPosition.x, 0.0f, connectingDoor.transform.localPosition.z).normalized;

            Vector3 roomOrigin = rb.dimensions / 2;

            for (int i = 0; i < 3; i++)
            {
                roomOrigin[i] *= translationAxis[i];
                roomOrigin[i] -= translationAxis[i] * (rb.wallThickness / 2);
            }

            roomOrigin += connectingDoor.transform.position;

            roomOrigin.y = 0.0f;

            return roomOrigin;
        }

        /// <summary>
        /// Randomises doors and sends them to the room builder.
        /// </summary>
        /// <param name="lastRoom">The previous room in order to determine which direction needs an empty doorway.</param>
        private void generateDoors(Room lastRoom, DoorController connectingDoor)
        {
            int minNewDoors = 1;
            int maxNewDoors;

            if (lastRoom != null)
            {
                maxNewDoors = 3;

                Vector3 translationAxis = new Vector3(connectingDoor.transform.localPosition.x, 0.0f, connectingDoor.transform.localPosition.z).normalized;

                if (connectingDoor.transform.localPosition.z > 0.0f) // Exit was north, entrance will be south
                    rb.southWall = RoomBuilder.wallType.DOORWAY;
                else if (connectingDoor.transform.localPosition.x > 0.0f) // Exit was east, entrance will be west
                    rb.westWall = RoomBuilder.wallType.DOORWAY;
                else if (connectingDoor.transform.localPosition.z < 0.0f) // Exit was south, entrance will be north
                    rb.northWall = RoomBuilder.wallType.DOORWAY;
                else if (connectingDoor.transform.localPosition.x < 0.0f) // Exit was west, entrance will be east
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

        /// <summary>
        /// Spawns all enemies inside the room.
        /// </summary>
        private void spawnEnemies(Room room)
        {
            int roomSizeUnitsSqrt = (int)(room.dimensions.x * room.dimensions.z);
            int enemyCount = (roomSizeUnitsSqrt / 1000) * enemiesPer1000UnitsSqrd;

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = enemySpawner.Spawn();

                // If enemy too close to player generate new position
                while ((enemy.transform.position - player.transform.position).magnitude < minEnemyDistance)
                    enemy.transform.position = enemySpawner.generateNewPosition();

                room.addEnemy(enemy.GetComponent<EnemyController>());
            }
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