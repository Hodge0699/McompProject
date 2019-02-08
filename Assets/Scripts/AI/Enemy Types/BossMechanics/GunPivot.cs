using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPivot : MonoBehaviour {

    public float angle = 180.0f;
    public float speed = 10.0f;

    private float startAngle;
    private float relativeAngle;
    private bool turningClockwise = true;
	
    // Use this for initialization
	void Start () {
        startAngle = this.transform.rotation.y;
	}
	
	// Update is called once per frame
	void Update () {
        checkBounds();
        turn();
    }

    private void turn()
    {
        float turnDistance = speed * Time.deltaTime;

        if (turningClockwise)
            transform.Rotate(Vector3.up, turnDistance);
        else
            transform.Rotate(Vector3.up, -turnDistance);

        relativeAngle = convertToSignedAngle(transform.localRotation.eulerAngles.y - startAngle);
    }

    private void checkBounds()
    {
        if (relativeAngle > angle / 2)
            turningClockwise = false;
        else if (relativeAngle < -angle / 2)
            turningClockwise = true;
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
