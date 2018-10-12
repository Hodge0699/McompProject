using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Door
{
    public Transform transform;
    public float width;
}


public class DoorController : MonoBehaviour {

    private Door leftDoor;
    private Door rightDoor;

    public State currentState;
    public enum State { CLOSED, OPENING, OPEN, CLOSING };

    public bool locked = true;
    public float speed = 2.0f;

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

        this.speed = speed;
    }

    // Update is called once per frame
    void Update() {
        if (currentState == State.OPENING)
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
    /// Opens the door
    /// </summary>
    public void open()
    {
        if (!locked && currentState != State.OPEN)
            currentState = State.OPENING;
    }

    /// <summary>
    /// Closes the door
    /// </summary>
    public void close()
    {
        if (!locked && currentState != State.CLOSED)
            currentState = State.CLOSING;
    }

    /// <summary>
    /// Locks/unlocks the door
    /// </summary>
    /// <param name="locked">True to lock, false to unlock.</param>
    public void setLocked(bool locked = true)
    {
        this.locked = locked;
    }
}
