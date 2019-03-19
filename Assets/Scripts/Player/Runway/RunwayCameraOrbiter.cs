using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runway
{
    public class RunwayCameraOrbiter : MonoBehaviour
    {
        public float rotationSpeed = 50.0f;

        private float minValue = -13.5f;
        private float maxValue = 67.0f;
        private float tolerance = 5.0f;
        
        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.localRotation.eulerAngles.x < maxValue || transform.localRotation.eulerAngles.x > 360 + minValue - tolerance) // Return if gone too far
                    transform.RotateAround(Vector3.zero, Vector3.right, -rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (transform.localRotation.eulerAngles.x > 360 + minValue || transform.localRotation.eulerAngles.x < maxValue + tolerance) // Return if gone too far
                    transform.RotateAround(Vector3.zero, Vector3.right, rotationSpeed * Time.deltaTime);
            }
        }
    }
}