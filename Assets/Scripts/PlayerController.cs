using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Rigidbody Rigidbody;
    private Camera mainCamera;
    public GunController gun;
    float rayLength;

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Default");
        Rigidbody = GetComponent<Rigidbody>();

        mainCamera = FindObjectOfType<Camera>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); //check whats better raw or not
        //moveVelocity = moveInput * moveSpeed;


        //Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        ////check if ray hit plane
        //if (groundPlane.Raycast(cameraRay, out rayLength))
        //{
        //    Vector3 aimAt = cameraRay.GetPoint(rayLength);
        //    Debug.DrawLine(cameraRay.origin, aimAt, Color.green);

        //    transform.LookAt(new Vector3(aimAt.x, transform.position.y, aimAt.z));

        //}

        //shooting
        if (Input.GetMouseButtonDown(0))
        { gun.isFiring = true; }
        if(Input.GetMouseButtonUp(0))
        { gun.isFiring = false; }
        if(Input.GetButtonDown("Right Mouse"))
        {
            gun.timeMechanic();
        }
    }
    void FixedUpdate()
    {
        Rigidbody.velocity = moveVelocity;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        Rigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            Rigidbody.MoveRotation(newRotation);
        }
    }


}
