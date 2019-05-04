using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class SawBlade : Melee
    {
        public float maxSpeed = 1000.0f;

        public float acceleration = 80.0f;
        public float deceleration = 100.0f;

        private float currentSpeed = 0.0f;

        public bool isAccelerating = false;

        TimeMechanic.LocalTimeDilation myTime;

        private void Start()
        {
            myTime = GetComponent<TimeMechanic.LocalTimeDilation>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isAccelerating && currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * myTime.getDelta();

                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;
            }
            else if (!isAccelerating && currentSpeed > 0)
            {
                currentSpeed -= deceleration * myTime.getDelta();

                if (currentSpeed < 0)
                    currentSpeed = 0;
            }

            transform.Rotate(Vector3.up, currentSpeed * myTime.getDelta());
        }
    }
}