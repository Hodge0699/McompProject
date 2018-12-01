using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public Vector3 dimensions;

    public List<DoorController> doors = new List<DoorController>(); // List of all doors connected to this room

    private List<Room> childRooms = new List<Room>();

    private int enemyCount; // Number of alive enemies in this room.
    private GameObject enemies; // Parent of all enemies in this room.

    public float lifetimeAfterBeaten = 2.0f;
    private float lifetimeCounter = 0.0f;
    private bool roomBeaten = false;

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

            if (lifetimeCounter >= lifetimeAfterBeaten)
                despawn();
        }
    }

    /// <summary>
    /// Sets an enemy's room to this and creates enemies GameObject if needed.
    /// </summary>
    /// <param name="enemy">Instantiated enemy.</param>
    public void addEnemy(EnemyController enemy)
    {
        if (enemies == null)
        {
            enemies = new GameObject();
            enemies.name = "Enemies";
            enemies.transform.parent = this.transform;
        }

        enemy.myRoom = this;
        enemy.transform.parent = enemies.transform;

        enemyCount++;
    }

    /// <summary>
    /// Called when an enemy is killed to decrease enemyCount
    /// </summary>
    /// <param name="enemy">Enemy that was killed.</param>
    public void enemyKilled(EnemyController enemy)
    {
        enemyCount--;

        if (enemyCount == 0)
        {
            openAllDoors();


            RoomBuilding.ProceduralRoomGeneration builder = FindObjectOfType<RoomBuilding.ProceduralRoomGeneration>();

            for (int i = 0; i < doors.Count; i++)
                childRooms.Add(builder.createRoom(this, doors[i]));
        }
    }
    
    /// <summary>
    /// Stores all doors connected to this room in a list.
    /// </summary>
    public void setDoors(List<DoorController> doors)
    {
        this.doors = doors;
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

    public void addChildRoom(Room room)
    {
        childRooms.Add(room);
    }

    /// <summary>
    /// Opens all doors connected to this room (used when all enemies are killed).
    /// </summary>
    private void openAllDoors()
    {
        for (int i = 0; i < doors.Count; i++)
            doors[i].open();
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

    public void playerExitted(DoorController exit)
    {
        bool roomFound = false;
        int i = 0;

        do
        {
            if (doors[i] == exit)
            {
                childRooms[i].addDoor(exit);

                player.GetComponent<PlayerController>().myCamera.GetComponent<CameraController>().setRoom(childRooms[i]);

                childRooms.RemoveAt(i);
                doors.RemoveAt(i);

                roomFound = true;
            }

            i++;
        } while (!roomFound);

        roomBeaten = true;
    }
}
