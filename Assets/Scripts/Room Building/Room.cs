using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public Vector3 dimensions;

    protected List<DoorController> doors = new List<DoorController>(); // List of all doors connected to this room
    protected List<Room> childRooms = new List<Room>();

    public int enemyCount; // Number of alive enemies in this room.
    public GameObject enemies; // Parent of all enemies in this room.

    private GameObject powerUpDrops; // Parent of all drops in this room.

    public float lifetimeAfterExit = 2.0f; // How many seconds the room lasts after the player exits
    private float lifetimeCounter = 0.0f;
    protected bool roomBeaten = false; // Whether the room has been beaten

    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<Player.PlayerController>().gameObject;
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
        if (exit.transform.Find("Left Door").transform.Find("GeometryFader(Clone)"))
            Destroy(exit.transform.Find("Left Door").transform.Find("GeometryFader(Clone)").gameObject);

        if (exit.transform.Find("Right Door").transform.Find("GeometryFader(Clone)"))
            Destroy(exit.transform.Find("Right Door").transform.Find("GeometryFader(Clone)").gameObject);

        bool roomFound = false;
        int i = 0;

        do
        {   // Find door/room index
            if (doors[i] == exit)
            {
                childRooms[i].gameObject.SetActive(true);
                childRooms[i].addDoor(exit);
                childRooms[i].enableEnemies();

                player.GetComponent<Player.PlayerController>().setRoom(childRooms[i]);

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
        foreach (DoorController door in doors)
            addDoor(door);
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
    public virtual void addDoor(DoorController door)
    {
        door.transform.parent = transform.Find("Doors").transform;
        doors.Add(door);

        // If the door is now the bottom door, create a fader
        if (door.transform.position.z - transform.position.z < 0)
        {
            instantiateFader(door.transform.Find("Left Door"));
            instantiateFader(door.transform.Find("Right Door"));
        }
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

    public void enableEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
            enemies.transform.GetChild(i).GetComponent<EnemyType.AbstractEnemy>().enabled = true;
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

        powerUpDrop.transform.SetParent(powerUpDrops.transform);
    }


    //
    // Geometry Fader
    //

    /// <summary>
    /// Adds a geometry fader to the wall(s) (and door if applicable) with the lowest z value 
    /// 
    /// Should ONLY be called AFTER doors and walls are added.
    /// </summary>
    public void createGeometryShaders()
    {
        // Walls
        List<Transform> lowerWalls = getCameraLowerWalls();

        for (int i = 0; i < lowerWalls.Count; i++)
            instantiateFader(lowerWalls[i]);
    }

    /// <summary>
    /// Returns a list of walls that have the lowest z value
    /// </summary>
    /// <returns>The transform of the walls</returns>
    private List<Transform> getCameraLowerWalls()
    {
        Transform walls = transform.Find("Walls");

        List<Transform> lowerWalls = new List<Transform>();

        for (int i = 0; i < walls.childCount; i++)
        {
            Transform currentWall = walls.GetChild(i);

            // Don't do this for vertical (from top down) walls
            if (Mathf.Abs(currentWall.rotation.eulerAngles.y) % 360 != 90 && Mathf.Abs(currentWall.rotation.eulerAngles.y) % 360 != 270)
            {
                // If wall lower (z distance) locally from camera
                if (currentWall.transform.position.z < transform.position.z)
                    lowerWalls.Add(currentWall);
            }
        }

        return lowerWalls;
    }

    /// <summary>
    /// Creates a trigger that fades the parent option
    /// </summary>
    /// <param name="parent">Object to fade.</param>
    /// <returns>The fader created.</returns>
    protected GameObject instantiateFader(Transform parent)
    {
        GameObject fader = Instantiate(Resources.Load("Room Components\\GeometryFader")) as GameObject;

        fader.transform.parent = parent;
        fader.transform.localPosition = new Vector3(0.0f, 0.0f, -4.0f);
        fader.transform.localScale = Vector3.one;
        fader.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        BoxCollider trigger = fader.GetComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(1.0f, 1.0f, 8.0f);
        return fader;
    }
}
