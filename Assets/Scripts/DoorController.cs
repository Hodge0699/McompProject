using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    struct Door
    {
        public Transform transform;
        public float width;
    }

    private Door leftDoor;
    private Door rightDoor;

    public State currentState;
    public enum State { CLOSED, OPENING, OPEN, CLOSING };

    public float speed = 2.0f; // Speed the door opens

    private bool triggered = false; // Has the exit been triggered?

    public void init(float width = 6, float height = 4, float thickness = 1, float speed = 2.0f)
    {
        leftDoor.transform = transform.Find("Left Door");
        rightDoor.transform = transform.Find("Right Door");

        currentState = State.CLOSED;

        leftDoor.transform.localScale = new Vector3(width / 2, height, thickness);
        leftDoor.width = leftDoor.transform.localScale.x;

        rightDoor.transform.localScale = new Vector3(width / 2, height, thickness);
        rightDoor.width = rightDoor.transform.localScale.x;

        leftDoor.transform.localPosition = new Vector3(-width / 4, 0, 0);
        rightDoor.transform.localPosition = new Vector3(width / 4, 0, 0);

        BoxCollider trigger = GetComponent<BoxCollider>();
        trigger.size = new Vector3(width, height, thickness / 2);
        trigger.center += new Vector3(0.0f, 0.0f, thickness / 4);

        this.speed = speed;
    }

    // Update is called once per frame
    void Update() {
        
        // Control states
        if (currentState == State.OPENING)
        {
            if (leftDoor.transform.localPosition.x <= -leftDoor.width * 1.525f && rightDoor.transform.localPosition.x >= rightDoor.width * 1.525f)
            {
                currentState = State.OPEN;

                leftDoor.transform.localPosition = new Vector3(-leftDoor.width * 1.525f, 0.0f, 0.0f);
                rightDoor.transform.localPosition = new Vector3(rightDoor.width * 1.525f, 0.0f, 0.0f);
            }
        }
        else if (currentState == State.CLOSING)
        {
            if (leftDoor.transform.localPosition.x >= -(leftDoor.width / 2) && rightDoor.transform.localPosition.x <= rightDoor.width / 2)
            {
                currentState = State.CLOSED;

                leftDoor.transform.localPosition = new Vector3(-leftDoor.width / 2, 0.0f, 0.0f);
                rightDoor.transform.localPosition = new Vector3(rightDoor.width / 2, 0.0f, 0.0f);
            }
        }
    }

    /// <summary>
    /// Moves the doors
    /// </summary>
    private void FixedUpdate()
    {
        if (currentState == State.OPENING)
        {
            leftDoor.transform.localPosition = new Vector3(leftDoor.transform.localPosition.x - (speed * Time.deltaTime), 0.0f, 0.0f);
            rightDoor.transform.localPosition = new Vector3(rightDoor.transform.localPosition.x + (speed * Time.deltaTime), 0.0f, 0.0f);
        }
        else if (currentState == State.CLOSING)
        {
            leftDoor.transform.localPosition = new Vector3(leftDoor.transform.localPosition.x + (speed * Time.deltaTime), 0.0f, 0.0f);
            rightDoor.transform.localPosition = new Vector3(rightDoor.transform.localPosition.x - (speed * Time.deltaTime), 0.0f, 0.0f);
        }
    }

    /// <summary>
    /// Loads next room and despawns current when player walks through
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // If other is not player, or already triggered, or currently closed, don't load next room
        if (other.GetComponent<Player.PlayerController>() == null || triggered || currentState == State.CLOSED)
            return;

        triggered = true;
        GetComponentInParent<Room>().playerExitted(this);

        // Move player into next room
        other.GetComponent<Player.PlayerInputManager>().forceMove(this.transform.forward, 6.5f);

        // Flip to point outwards
        this.transform.Rotate(Vector3.up, 180.0f);
    }

    /// <summary>
    /// Closes the door after the player steps through
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player.PlayerController>() == null)
            return;

        triggered = false;

        close();
    }

    /// <summary>
    /// Opens the door
    /// </summary>
    public void open()
    {
        if (currentState != State.OPEN)
            currentState = State.OPENING;

        // Add an indicator chevron to point to this.
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player.PlayerController>().addIndicator(this.transform.position);
    }

    /// <summary>
    /// Closes the door
    /// </summary>
    public void close()
    {
        if (currentState != State.CLOSED)
            currentState = State.CLOSING;
    }
}
