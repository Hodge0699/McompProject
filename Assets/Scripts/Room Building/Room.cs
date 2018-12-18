using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public Vector3 dimensions;

    private List<DoorController> doors = new List<DoorController>(); // List of all doors connected to this room
    private List<Room> childRooms = new List<Room>();

    public int enemyCount; // Number of alive enemies in this room.
    public GameObject enemies; // Parent of all enemies in this room.

    private GameObject powerUpDrops; // Parent of all drops in this room.

    public float lifetimeAfterExit = 2.0f; // How many seconds the room lasts after the player exits
    private float lifetimeCounter = 0.0f; 
    private bool roomBeaten = false; // Whether the room has been beaten

    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void Update()
    {
        if (roomBeaten)
        {
            lifetimeCounter += Time.deltaTime;

            if (lifetimeCounter >= lifetimeAfterExit)
                despawn();
        }
    }


    /// <summary>
    /// Adds a room to the rooms array
    /// </summary>
    /// <param name="room">New room to add.</param>
    public void addChildRoom(Room room)
    {
        childRooms.Add(room);
    }

    /// <summary>
    /// Despawns current room.
    /// </summary>
    /// <param name="newRoom">New room to pass the exit door to.</param>
    public void despawn()
    {
        for (int i = 0; i < childRooms.Count; i++)
            Destroy(childRooms[i].gameObject);

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Tests if a position is within room bounds.
    /// </summary>
    /// <param name="pos">Vector3 position to test for.</param>
    /// <returns>Boolean whether pos is in room</returns>
    public bool isInRoom(Vector3 pos)
    {
        return ((Mathf.Abs(pos.x) < transform.position.x + dimensions.x) && (Mathf.Abs(pos.y) < transform.position.y + dimensions.y));
    }

    /// <summary>
    /// Called when player exits this room.
    /// </summary>
    /// <param name="exit">The exit door used.</param>
    public void playerExitted(DoorController exit)
    {
        bool roomFound = false;
        int i = 0;

        do
        {   // Find door/room index
            if (doors[i] == exit)
            {
                childRooms[i].addDoor(exit);

                player.GetComponent<PlayerController>().setRoom(childRooms[i]);

                childRooms.RemoveAt(i);
                doors.RemoveAt(i);

                roomFound = true;
            }

            i++;
        } while (!roomFound);

        roomBeaten = true;
    }



    //
    // Doors
    //

    /// <summary>
    /// Stores all doors connected to this room in a list.
    /// </summary>
    public void setDoors(List<DoorController> doors)
    {
        this.doors = doors;
    }

    /// <summary>
    /// Returns list of doors.
    /// </summary>
    public List<DoorController> getDoors()
    {
        return doors;
    }

    /// <summary>
    /// Adds a single door to the doors list.
    /// </summary>
    /// <param name="door">Exit from previous room.</param>
    public void addDoor(DoorController door)
    {
        door.transform.parent = transform.Find("Doors").transform;
        doors.Add(door);
    }

    /// <summary>
    /// Opens all doors connected to this room (used when all enemies are killed).
    /// </summary>
    private void openAllDoors()
    {
        for (int i = 0; i < doors.Count; i++)
            doors[i].open();
    }



    //
    // Enemies
    //

    /// <summary>
    /// Sets an enemy's room to this and creates enemies GameObject if needed.
    /// </summary>
    /// <param name="enemy">Instantiated enemy.</param>
    public void addEnemy(EnemyType.AbstractEnemy enemy)
    {
        if (enemies == null)
        {
            enemies = new GameObject();
            enemies.name = "Enemies";
            enemies.transform.parent = this.transform;
        }

        enemy.setRoom(this);
        enemy.transform.parent = enemies.transform;

        enemyCount++;
    }

    /// <summary>
    /// Called when an enemy is killed to decrease enemyCount
    /// </summary>
    /// <param name="enemy">Enemy that was killed.</param>
    public void enemyKilled(EnemyType.AbstractEnemy enemy)
    {
        enemyCount--;

        if (enemyCount == 0)
        {
            openAllDoors();

            RoomBuilding.ProceduralRoomGeneration builder = FindObjectOfType<RoomBuilding.ProceduralRoomGeneration>();

            childRooms = builder.roomBeat(this);
        }
    }



    //
    // Power up drops
    //

    /// <summary>
    /// Attaches a power up drop to this room so it can be destroyed on despawn.
    /// </summary>
    /// <param name="powerUpDrop">Drop to attach.</param>
    public void addPowerUpDrop(GameObject powerUpDrop)
    {
        if (powerUpDrops == null)
        {
            powerUpDrops = new GameObject();
            powerUpDrops.name = "Power Up Drops";
            powerUpDrops.transform.parent = this.transform;
        }

        powerUpDrop.transform.parent = powerUpDrops.transform;
    }
}
