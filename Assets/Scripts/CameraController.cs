using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform target;

    public float smoothing = 5f;

    public Vector2 size = new Vector2(8.0f, 4.5f);

    private Vector3 offset;

    private Vector3 roomSize; // Size of the current room

    private Room currentRoom;


    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        offset = transform.position - target.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = getBoundedTargetPos(target.position);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
    
    /// <summary>
    /// Calculates closest centred position of camera over target with offset while remaining in room bounds.
    /// </summary>
    /// <param name="target">Target to try to centre over.</param>
    /// <returns>Closest centred position within room bounds.</returns>
    private Vector3 getBoundedTargetPos(Vector3 target)
    {
        Vector3 targetPos = target;

        Vector3 roomPos = currentRoom.transform.position;
        Vector3 roomSize = currentRoom.dimensions;

        if      (targetPos.x > roomPos.x + (roomSize.x / 2) - size.x)
                 targetPos.x = roomPos.x + (roomSize.x / 2) - size.x;
        else if (targetPos.x < roomPos.x - (roomSize.x / 2) + size.x)
                 targetPos.x = roomPos.x - (roomSize.x / 2) + size.x;

        if      (targetPos.z > roomPos.z + (roomSize.z / 2) - (size.y * 0.5f))
                 targetPos.z = roomPos.z + (roomSize.z / 2) - (size.y * 0.5f);
        else if (targetPos.z < roomPos.z - (roomSize.z / 2) + (size.y * 1.5f))
                 targetPos.z = roomPos.z - (roomSize.z / 2) + (size.y * 1.5f);

        return targetPos + offset;
    }

    /// <summary>
    /// Sets the players current room so the camer doesn't go out of bounds
    /// </summary>
    /// <param name="room">Player's current room.</param>
    public void setRoom(Room room)
    {
        this.currentRoom = room;
    }
}
