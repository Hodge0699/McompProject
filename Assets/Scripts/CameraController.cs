using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform target;

    public float smoothing = 5f;

    public Vector2 size = new Vector2(8.0f, 4.5f);

    private Vector3 offset;

    private Vector3 roomSize; // Size of the current room

    private Player.PlayerController player;


    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        player = target.GetComponent<Player.PlayerController>();
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

        if (player.getCurrentRoom() == null)
            return targetPos + offset;

        Vector3 roomPos = player.getCurrentRoom().transform.position;
        Vector3 roomSize = player.getCurrentRoom().dimensions;

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
}
