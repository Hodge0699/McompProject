using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public Vector3 dimensions;

    public DoorController exit; // Door player used to exit

    private List<DoorController> doors = new List<DoorController>(); // List of all doors connected to this room 

    private int enemyCount; // Number of alive enemies in this room.
    private GameObject enemies; // Parent of all enemies in this room.

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
            openAllDoors();
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
    public void despawn(Room newRoom)
    {
        // Destroy all enemies
        for (int i = 0; i < enemies.transform.childCount; i++)
            Destroy(enemies.transform.GetChild(i).gameObject);

        // Pass exit door to new room so it doesn't despawn
        newRoom.addDoor(exit);

        Destroy(this.gameObject);
    }
}
