using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public Vector3 dimensions;

    public DoorController exit; // Door player used to exit

    public void despawn(Room newRoom)
    {
        // Pass exit door to new room so it doesn't despawn
        exit.transform.parent = newRoom.transform.Find("Doors").transform;

        Destroy(this.gameObject);
    }
}
