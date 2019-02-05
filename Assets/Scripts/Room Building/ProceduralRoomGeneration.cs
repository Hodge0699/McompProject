using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoomBuilding
{
    public class ProceduralRoomGeneration : MonoBehaviour
    {
        public Vector2 maxRoomSize = new Vector2(50.0f, 50.0f); // Maximum size of a room.
        public Vector2 minRoomSize = new Vector2(32.0f, 32.0f); // Minimum size of a room .

        [Range(0, 25)]
        public int enemyFrequency = 5; // Amount of enemies to be spawned per 1000 units squared

        [Range(0,15)]
        public int rooms = 5; // Number of rooms player must progress through before boss room spawns.
        private int roomsBeat = 0;

        public GameObject bossRoom; // Room prefab to spawn after player beats sufficient rooms.

        public bool spawnPlayer = true; // Should the player be generated (false if test player used).

        private RoomBuilder rb;
        private EnemySpawner enemySpawner;

        private Player.PlayerController player;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<RoomBuilder>();
            enemySpawner = GetComponent<EnemySpawner>();

            if (spawnPlayer)
            {
                // Liam - Spawn player at start of scene when first room is generated
                GameObject playerObj = Instantiate(Resources.Load("Player")) as GameObject; // Liam
                playerObj.transform.position = Vector3.zero; // Liam

                player = playerObj.GetComponent<Player.PlayerController>();
            }
            else
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.PlayerController>();

            // Create initial room
            Room startRoom = createRoom(null, null);
            player.setRoom(startRoom);

        }

        /// <summary>
        /// Creates a new room either completely randomly or based off a previous room.
        /// </summary>
        /// <param name="lastRoom">The room the player has just beat. Null if first room.</param>
        /// <param name="connectingDoor">Door connecting old and new room.</param>
        /// <returns>The newly created room.</returns>
        private Room createRoom(Room lastRoom, DoorController connectingDoor)
        {
            if (roomsBeat >= rooms)
                return createBossRoom(lastRoom, connectingDoor);

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
            enemySpawner.transform.position = rb.transform.position;

            spawnEnemies(newRoom);

            return newRoom;
        }

        /// <summary>
        /// Spawns a boss room instead of a generating a regular room
        /// </summary>
        /// <param name="lastRoom">The room the player has just beat.</param>
        /// <param name="connectingDoor">Door connecting old and new room.</param>
        /// <returns>The newly created boss room.</returns>
        private Room createBossRoom(Room lastRoom, DoorController connectingDoor)
        {
            GameObject roomObj = Instantiate(bossRoom);
            Room roomScr = roomObj.GetComponent<Room>();

            if (connectingDoor.transform.forward == new Vector3(0.0f, 0.0f, -1.0f)) // South
                roomObj.transform.Rotate(Vector3.up, 90);
            else if (connectingDoor.transform.forward == new Vector3(-1.0f, 0.0f, 0.0f)) // West
                roomObj.transform.Rotate(Vector3.up, 180);
            else if (connectingDoor.transform.forward == new Vector3(0.0f, 0.0f, 1.0f)) // North
                roomObj.transform.Rotate(Vector3.up, 270);

            Vector3 origin = connectingDoor.transform.position;

            for (int i = 0; i < 3; i++)
            {
                origin[i] += connectingDoor.transform.forward[i] * (roomScr.dimensions.x / 2);
                origin[i] -= connectingDoor.transform.forward[i] * (rb.wallThickness / 2);
            }

            origin.y = 0;

            roomObj.transform.position = origin;

            // Flip x and z dimensions in script if room now sideways (for camera bounds)
            if (connectingDoor.transform.forward.z != 0.0f)
            {
                float temp = roomScr.dimensions.x;
                roomScr.dimensions.x = roomScr.dimensions.z;
                roomScr.dimensions.z = temp;
            }
            return roomScr;
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

            Vector3 roomOrigin = rb.dimensions / 2;

            for (int i = 0; i < 3; i++)
            {
                roomOrigin[i] *= connectingDoor.transform.forward[i];
                roomOrigin[i] -= connectingDoor.transform.forward[i] * (rb.wallThickness / 2);
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
                RoomBuilder.Direction dir;
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
            float minEnemyDistance = 7.5f; // How far away from the players must the enemies spawn.

            if (enemyFrequency == 0)
                return;

            float roomSizeSqr = room.dimensions.x * room.dimensions.z;
            int enemyCount = (int)((roomSizeSqr / 1000) * enemyFrequency);

            do
            {
                GameObject enemy = enemySpawner.spawn();

                // If enemy too close to player generate new position
                while ((enemy.transform.position - player.transform.position).magnitude < minEnemyDistance)
                    enemy.transform.position = enemySpawner.generateNewPosition();

                room.addEnemy(enemy.GetComponent<EnemyType.AbstractEnemy>());

                enemyCount--;
            } while (enemyCount > 0);
        }

        /// <summary>
        /// Randomises a direction.
        /// </summary>
        RoomBuilder.Direction getRandomDirection()
        {
            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    return RoomBuilder.Direction.NORTH;
                case 1:
                    return RoomBuilder.Direction.EAST;
                case 2:
                    return RoomBuilder.Direction.SOUTH;
                case 3:
                    return RoomBuilder.Direction.WEST;
                default:
                    return RoomBuilder.Direction.ERROR;
            }
        }

        /// <summary>
        /// Called by rooms to signal they've been beat. Spawns next room
        /// </summary>
        /// <param name="room">Beaten room</param>
        public List<Room> roomBeat(Room room)
        {
            roomsBeat++;

            List<DoorController> doors = room.getDoors();

            List<Room> newRooms = new List<Room>();

            for (int i = 0; i < doors.Count; i++)
                newRooms.Add(createRoom(room, doors[i]));

            return newRooms;
        }
    }
}