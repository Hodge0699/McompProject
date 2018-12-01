using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    public GameObject myCamera = null;
    public float moveSpeed;
    private Vector3 moveInput;
    private Rigidbody Rigidbody;

    public bool debugging = false;
    public bool useController;
    Plane mousePlane; // Plane to track the mouse position on screen.

    Vector3 directionVector;

    public bool allowPlayerControl = true;
    private float forceMoveDistanceCounter = 0.0f;
    private float forceMoveDistanceTarget = 0.0f;

    Vector3 cameraPos = new Vector3(0f, 7f, -10f);

    private Room currentRoom;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        myCamera = Instantiate(myCamera, transform.position + cameraPos, Quaternion.Euler(33, 0, 0));

        mousePlane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));
    }

    void FixedUpdate()
    {

        if (Input.GetKeyDown("p"))
        { useController = !useController; }


        if (allowPlayerControl)
        {
            directionVector.x = Input.GetAxisRaw("Horizontal");
            directionVector.z = Input.GetAxisRaw("Vertical");
        }

        Move();

        if (useController)
            TurningWithController();
        else
            Turning();
    }

    /// <summary>
    /// Moves the player based on its current direction vector
    /// </summary>
    void Move()
    {
        Vector3 movement = directionVector.normalized * moveSpeed * Time.deltaTime;

        Rigidbody.MovePosition(transform.position + movement);
        Rigidbody.velocity = movement;

        if (!allowPlayerControl)
        {
            forceMoveDistanceCounter += movement.magnitude;

            if (forceMoveDistanceCounter >= forceMoveDistanceTarget)
            {
                forceMoveDistanceCounter = 0.0f;
                forceMoveDistanceTarget = 0.0f;
                allowPlayerControl = true;
            }
        }
        else
            directionVector = Vector3.zero;
    }

    /// <summary>
    /// Forces a player to move in a direction for a set distance
    /// </summary>
    /// <param name="dir">Direction to move in.</param>
    /// <param name="distance">Distance to travel before stopping.</param>
    public void forceMove(Vector3 dir, float distance)
    {
        if (dir == Vector3.zero)
            return;

        allowPlayerControl = false;
        forceMoveDistanceTarget = distance;

        directionVector = dir;
    }

    /// <summary>
    /// Turns player to look at mouse pos
    /// </summary>
    void Turning()
    {
        Vector3 mousePos = getMousePos();

        // Create a vector from the player to the point on the floor the raycast from the mouse hit.
        Vector3 playerToMouse = mousePos - transform.position;

        // Ensure the vector is entirely along the floor plane.
        playerToMouse.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

        // Set the player's rotation to this new rotation.
        Rigidbody.MoveRotation(newRotation);
    }

    /// <summary>
    /// Gets the position of the mouse in world space.
    /// </summary>
    /// <returns>Position of mouse in world space.</returns>
    public Vector3 getMousePos()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float intersect = 0.0f;
        mousePlane.Raycast(camRay, out intersect);

        if (debugging)
            Debug.DrawLine(Camera.main.transform.position, camRay.GetPoint(intersect), Color.red);

        return camRay.GetPoint(intersect);
    }

    void TurningWithController()
    {
        Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("ShootHorizontal") + Vector3.forward * -Input.GetAxisRaw("ShootVertical");
        //check if vector tree registered a movement
        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }
    }

    /// <summary>
    /// Sets the player's current room for reference.
    /// </summary>
    /// <param name="room">Player's current room.</param>
    public void setRoom(Room room)
    {
        this.currentRoom = room;
    }

    /// <summary>
    /// Gets the current room the player is in
    /// </summary>
    public Room getCurrentRoom()
    {
        return currentRoom;
    }
}