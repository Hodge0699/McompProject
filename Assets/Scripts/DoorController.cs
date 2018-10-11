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

    private float doorWidth;

    // Use this for initialization
    void Start() {
        leftDoor.transform = transform.Find("Left Door");
        leftDoor.width = leftDoor.transform.localScale.x;

        rightDoor.transform = transform.Find("Right Door");
        rightDoor.width = rightDoor.transform.localScale.x;

        currentState = State.CLOSED;
    }

    // Update is called once per frame
    void Update() {
        if (currentState == State.OPENING)
        {
            if (leftDoor.transform.localPosition.x <= -leftDoor.width && rightDoor.transform.localPosition.x >= rightDoor.width)
            {
                currentState = State.OPEN;

                leftDoor.transform.localPosition = new Vector3(-leftDoor.width, 0.0f, 0.0f);
                rightDoor.transform.localPosition = new Vector3(rightDoor.width, 0.0f, 0.0f);
            }
        }
        else if (currentState == State.CLOSING)
        {
            if (leftDoor.transform.localPosition.x >= -leftDoor.width / 2 && rightDoor.transform.localPosition.x <= rightDoor.width / 2)
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

    public void open()
    {
        if (!locked && currentState != State.OPEN)
            currentState = State.OPENING;
    }

    public void close()
    {
        if (!locked && currentState != State.CLOSED)
            currentState = State.CLOSING;
    }

    public void setLocked(bool locked = true)
    {
        this.locked = locked;
    }
}
