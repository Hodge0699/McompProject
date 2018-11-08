using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private GameObject myCamera = null;
    public BulletController BC;
    public float moveSpeed;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Rigidbody Rigidbody;

    public bool debugging = false;

    Plane mousePlane; // Plane to track the mouse position on screen.

    //private Camera mainCamera;

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    public float Damage = 100f; // jack 
    public float speed = 12f;

    Vector3 cameraPos = new Vector3(0f, 7f, -10f);

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        Instantiate(myCamera, transform.position + cameraPos, Quaternion.Euler(33, 0, 0));

        mousePlane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));
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
}