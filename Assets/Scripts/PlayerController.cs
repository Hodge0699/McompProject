using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private GameObject myCamera = null;

    public float moveSpeed;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Rigidbody Rigidbody;
    //private Camera mainCamera;
    float rayLength;


    Vector3 movement;                   // The vector to store the direction of the player's movement.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    Vector3 cameraPos = new Vector3(0f, 7f, -10f);

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        Rigidbody = GetComponent<Rigidbody>();


        Instantiate (myCamera, transform.position + cameraPos, Quaternion.Euler(33,0,0));
    }

    void FixedUpdate()
    {
        Rigidbody.velocity = moveVelocity;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
    }

    /// <summary>
    /// Moves player in world space.
    /// </summary>
    /// <param name="h">Horizontal movement.</param>
    /// <param name="v">Vertical movement.</param>
    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        Rigidbody.MovePosition(transform.position + movement);
    }

    /// <summary>
    /// Turns player to look at mouse pos
    /// </summary>
    void Turning()
    {
        Vector3? mousePos = getMousePos();

        // Return early if invalid mouse position
        if (mousePos == null)
            return;

        // Create a vector from the player to the point on the floor the raycast from the mouse hit.
        Vector3 playerToMouse = mousePos.Value - transform.position;

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
    /// <returns>Position of mouse or null if mouse not over floor.</returns>
    public Vector3 ? getMousePos()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            return floorHit.point;
        else
            return null;
    }
}
