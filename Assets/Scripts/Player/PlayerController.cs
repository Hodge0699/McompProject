using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject myCamera = null;
    public float moveSpeed;
    public Vector3 movement;
    private Vector3 moveInput;
    private Rigidbody Rigidbody;

    public bool debugging = false;
    public bool useController;
    Plane mousePlane; // Plane to track the mouse position on screen.

    Vector3 directionVector;

    public bool allowPlayerControl = true;
    private float forceMoveDistanceCounter = 0.0f;
    private float forceMoveDistanceTarget = 0.0f;

    private Vector3 cameraOffset = new Vector3(0f, 7f, -10f);

    private Room currentRoom;

    private Transform firePoint;

    public enum EMOTION { HAPPY, ANGRY };
    private GameObject smile;
    private GameObject angry;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        firePoint = transform.Find("GunPrimary").transform.Find("Body");

        myCamera = Instantiate(myCamera, transform.position + cameraOffset, Quaternion.Euler(33, 0, 0));

        mousePlane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));

        GameObject playerUI = Instantiate(Resources.Load("PlayerUI")) as GameObject;

        GetComponent<PlayerHealthManager>().init(playerUI);

        smile = transform.Find("SmileFace").gameObject;
        angry = transform.Find("AngryFace").gameObject;
    }

    void FixedUpdate()
    {
        Move();

        if (useController)
            TurningWithController();
        else
            Turning();
    }

    /// <summary>
    /// Sets the direction vector to be acted upon at the end of the frame
    /// </summary>
    /// <param name="dir">Normalised vector</param>
    public void setDirectionVector(Vector3 dir)
    {
        dir.Normalize();
        this.directionVector = dir;
    }

    /// <summary>
    /// Moves the player based on its current direction vector
    /// </summary>
    void Move()
    {
        movement = directionVector.normalized * moveSpeed * Time.deltaTime;

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

                GetComponent<PlayerHealthManager>().setGodmode(true, 1.5f);
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

        GetComponent<PlayerHealthManager>().setGodmode(true);
    }

    /// <summary>
    /// Turns player to look at mouse pos
    /// </summary>
    void Turning()
    {
        Vector3 target = getMousePos() - transform.position; // Get direction vector from player to target

        if (target.magnitude < 1.5f) // If direction vector too short, extend it 
        {
            target.Normalize();
            target *= 1.5f;
        }

        target += transform.position; // Apply this position to altered direction vector to produce new target position

        Vector3 gunToTarget = target - firePoint.transform.position; // Get direction vector from gun to target
        gunToTarget.y = 0;
        gunToTarget.Normalize();

        float turnAngle = Vector3.SignedAngle(transform.forward, gunToTarget, Vector3.up); // Calculate angle between gun forward and target dir

        if (debugging)
        {
            Debug.DrawLine(getMousePos() - Vector3.up * 5, getMousePos() + Vector3.up * 5, Color.red);
            Debug.DrawRay(firePoint.transform.position, transform.forward);
            Debug.DrawRay(firePoint.transform.position, gunToTarget);
        }

        transform.Rotate(Vector3.up, turnAngle);
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

    /// <summary>
    /// Sets Darren's face to an emotion
    /// </summary>
    /// <param name="e">Happy or angry.</param>
    public void setFace(EMOTION e)
    {
        smile.SetActive(e == EMOTION.HAPPY);
        angry.SetActive(e == EMOTION.ANGRY);
    }
}