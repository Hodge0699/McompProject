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

    // Use this for initialization
    void Start () {

        Rigidbody = GetComponent<Rigidbody>();

        mainCamera = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); //check whats better raw or not
        moveVelocity = moveInput * moveSpeed;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        //check if ray hit plane
        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 aimAt = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, aimAt, Color.green);

            transform.LookAt(new Vector3(aimAt.x, transform.position.y, aimAt.z));

        }

        //shooting
        if(Input.GetMouseButtonDown(0))
        { gun.isFiring = true; }
        if(Input.GetMouseButtonUp(0))
        { gun.isFiring = false; }



    }

    void FixedUpdate()
    {
        Rigidbody.velocity = moveVelocity;
    }


}
