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

    // How soon after the player steps through the door can the door be used again (room generation)
    private float triggerCooldown = 5.0f;
    private float triggerCooldownCounter = 0.0f;

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

        this.speed = speed;
    }

    // Update is called once per frame
    void Update() {
        // Check if door can be triggered again
        if (triggerCooldownCounter > 0.0f)
        {
            triggerCooldownCounter += Time.fixedDeltaTime;

            if (triggerCooldownCounter >= triggerCooldown)
                triggerCooldownCounter = 0.0f;
        }

        // Control states
        else if (currentState == State.OPENING)
        {
            if (leftDoor.transform.localPosition.x <= -leftDoor.width * 1.5f && rightDoor.transform.localPosition.x >= rightDoor.width * 1.5f)
            {
                currentState = State.OPEN;

                leftDoor.transform.localPosition = new Vector3(-leftDoor.width * 1.5f, 0.0f, 0.0f);
                rightDoor.transform.localPosition = new Vector3(rightDoor.width * 1.5f, 0.0f, 0.0f);
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
        if (triggerCooldownCounter == 0.0f)
        {
            triggerCooldownCounter = Time.deltaTime;
            GetComponentInParent<Room>().exit = this;
            FindObjectOfType<RoomBuilding.ProceduralRoomGeneration>().createRoom();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        close();
    }

    /// <summary>
    /// Opens the door
    /// </summary>
    public void open()
    {
        if (currentState != State.OPEN)
            currentState = State.OPENING;
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
