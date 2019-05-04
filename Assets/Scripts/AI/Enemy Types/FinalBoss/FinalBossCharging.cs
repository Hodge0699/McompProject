using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossCharging : FinalBoss
    {
        // How fast the boss can turn whilst charging
        private float chargingTurnSpeed = 10;
        private float regularTurnSpeed;

        // How fast the boss can charge
        private float chargingMovementSpeed = 15;
        private float regularMovementSpeed;

        private float sprintTime;
        private float sprintCounter = 0.0f;

        // Use this for initialization
        override protected void Start()
        {
            base.Start();

            regularTurnSpeed = turnSpeed;
            turnSpeed = chargingTurnSpeed;

            regularMovementSpeed = movementSpeed;
            movementSpeed = chargingMovementSpeed;

            sprintTime = calculateSprintTime();
        }

        protected override Type decideState()
        {
            if (sprintCounter >= sprintTime) // Completed sprint
                return typeof(FinalBossChasing);
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            // Sprint counter runs off global delta to allow boss to overshoot if sped up
            sprintCounter += Time.deltaTime;

            turnTo(player);
            directionVector = transform.forward;

        }

        protected override void onBehaviourSwitch(AbstractEnemy newState)
        {
            turnSpeed = regularTurnSpeed;
            movementSpeed = regularMovementSpeed;
        }

        /// <summary>
        /// Calculates the time it will take for the boss to reach the wall infront of it
        /// </summary>
        /// <returns>Time in seconds</returns>
        private float calculateSprintTime()
        {
            // Ray forward 
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 100.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            int maxRayAttempts = 10;

            while (hitInfo.collider.tag != "Geometry") // Keep raycasting until wall hit
            {
                Physics.Raycast(hitInfo.point + (transform.forward * 0.1f), transform.forward, out hitInfo, 100.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

                maxRayAttempts--;

                if (maxRayAttempts <= 0)
                {
                    Debug.LogError("Final boss could not find geometry");
                    throw new Exception("Final boss could not find geometry");
                }
            }

            float distanceToWall = (hitInfo.point - transform.position).magnitude;

            // Stop at distance twice the diameter of boss. Should work well for scaling
            float stopDistance = transform.lossyScale.x * 2;

            return (distanceToWall - stopDistance) / movementSpeed;
        }
    }
}