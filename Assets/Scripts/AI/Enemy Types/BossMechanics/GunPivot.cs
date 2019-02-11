using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPivot : MonoBehaviour {

    public float angle = 180.0f;
    public float speed = 10.0f;

    private float startAngle;
    private float relativeAngle;
    private bool turningClockwise = true;

    private enum State { STOPPED, RUNNING, STOPPING, GOING_TO_BOUND };
    
    [SerializeField]
    private State state = State.STOPPED;

    // Use this for initialization
    void Start()
    {
        startAngle = convertToSignedAngle(transform.localRotation.eulerAngles.y);
        relativeAngle = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.STOPPED)
            return;

        turn();

        switch (state)
        {
            case State.STOPPING:
                if (isAtCentre())
                {
                    state = State.STOPPED;
                    transform.Rotate(Vector3.up, -relativeAngle);
                    relativeAngle = 0.0f;
                }
                break;

            case State.GOING_TO_BOUND:
                if (getRotationPercentage() >= 0.9f)
                    state = State.STOPPED;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Starts pivot movement
    /// </summary>
    public void startPivot()
    {
        state = State.RUNNING;
    }

    /// <summary>
    /// Ends pivot movement
    /// </summary>
    /// <param name="returnToCentre">Should the pivot return to its starting rotation.</param>
    public void stopPivot(bool returnToCentre = false)
    {
        if (state == State.STOPPED && (!returnToCentre || isAtCentre()))
            return;

        if (returnToCentre)
        {
            if ((turningClockwise && relativeAngle > 0) || (!turningClockwise && relativeAngle < 0))
                turningClockwise = !turningClockwise;

            state = State.STOPPING;
        }
        else
            state = State.STOPPED;
    }

    /// <summary>
    /// Sets rotation direction
    /// </summary>
    public void rotateClockwise()
    {
        turningClockwise = true;
    }

    /// <summary>
    /// Sets rotation direction
    /// </summary>
    public void rotateCounterClockwise()
    {
        turningClockwise = false;
    }

    public float getRotationPercentage()
    {
        return relativeAngle / (angle / 2);
    }

    /// <summary>
    /// Turns the pivot to one of its bounds
    /// </summary>
    /// <param name="turnClockwise">True to rotate pivot to right bound, false to send it to left.</param>
    public void goToBound(bool turnClockwise)
    {
        turningClockwise = turnClockwise;
        state = State.GOING_TO_BOUND;
    }

    /// <summary>
    /// Decides direction and rotates pivot
    /// </summary>
    private void turn()
    {
        float turnDistance = speed * Time.deltaTime;

        checkBounds();

        if (turningClockwise)
            transform.Rotate(Vector3.up, turnDistance);
        else
            transform.Rotate(Vector3.up, -turnDistance);

        relativeAngle = convertToSignedAngle(transform.localRotation.eulerAngles.y) - startAngle;
    }

    /// <summary>
    /// Flips pivot direction if out of bounds
    /// </summary>
    /// <returns>True if out of bounds.</returns>
    private bool checkBounds()
    {
        if (relativeAngle > angle / 2)
        {
            rotateCounterClockwise();
            return true;
        }
        else if (relativeAngle < -angle / 2)
        {
            rotateClockwise();
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns true if the pivot is back at its starting position.
    /// </summary>
    public bool isAtCentre()
    {
        return (Mathf.Abs(relativeAngle) <= 1.0f);
    }

    /// <summary>
    /// Converts a positive angle into a signed angle (-179 to 180)
    /// </summary>
    /// <param name="angle">Angle to convert</param>
    /// <returns>Signed angle</returns>
    private float convertToSignedAngle(float angle)
    {
        angle %= 360;

        if (angle > 180)
            angle -= 360;

        return angle;
    }
}
