using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Runway
{
    public class RunwayCameraOrbiter : MonoBehaviour
    {

        public float rotationSpeed = 50.0f;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                transform.RotateAround(Vector3.zero, Vector3.right, -rotationSpeed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                transform.RotateAround(Vector3.zero, Vector3.right, rotationSpeed * Time.deltaTime);
        }
    }
}