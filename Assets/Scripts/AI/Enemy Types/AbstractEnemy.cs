using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using sceneTransitions;

namespace EnemyType
{
    public class AbstractEnemy : MonoBehaviour
    {
        public float movementSpeed = 3.0f;

        public Vector3 directionVector; // Direction vector to act on at end of frame

        protected GameObject target; // The GameObject this agent is currently attacking

        protected Room myRoom;

        protected VisionCone visionCone;

        public float maxDistance = 1.6f; // Distance enemy should get to the player

        public float turnSpeed = 90.0f; // Maximum angle enemy can turn in one second

        // Use this for initialization
        protected virtual void Awake()
        {
            visionCone = GetComponent<VisionCone>();
        }

        private void LateUpdate()
        {
            directionVector.Normalize();
            Vector3 movement = directionVector * movementSpeed * Time.deltaTime;

            transform.Translate(movement, Space.World);

            directionVector = Vector3.zero;

            if (visionCone.hasVisibleTargets())
                target = visionCone.getClosestVisibleTarget();
            else
                target = null;
        }

        /// <summary>
        /// Override if a specific enemy should do something special on death
        /// </summary>
        public virtual void onDeath() { }

        /// <summary>
        /// Links this enemy to a room
        /// </summary>
        public void setRoom(Room room)
        {
            myRoom = room;
        }

        /// <summary>
        /// Calculates the distance to the target.
        /// </summary>
        /// <returns>Infinity if target null.</returns>
        protected float getDistanceToTarget()
        {
            if (target == null)
                return Mathf.Infinity;

            return (transform.position - target.transform.position).magnitude;
        }

        /// <summary>
        /// Returns the room this enemy has been assigned to 
        /// </summary>
        public Room getRoom()
        {
            return myRoom;
        }

        //
        // Behaviours
        //

        /// <summary>
        /// Randomly sets the direction vector
        /// </summary>
        protected virtual void wander()
        {
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 3.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            if (hitInfo.collider) // Wall ahead, turn
            {
                float rotation = Random.Range(70.0f, 110.0f);
                int sign = Random.Range(0, 1); // rotation or -rotation (left or right turn)

                if (sign == 0)
                    transform.Rotate(Vector3.up, rotation);
                else
                    transform.Rotate(Vector3.up, -rotation);
            }
            else
            {
                float rotation = Random.Range(-turnSpeed, turnSpeed);
                turnTo(rotation);
            }

            directionVector = transform.forward;
        }

        /// <summary>
        /// Chases the target if there is one
        /// </summary>
        protected virtual void chase()
        {
            if (target == null)
                return;

            if (Vector3.Distance(target.transform.position, this.transform.position) > maxDistance)
            {
                turnTo(target);
                directionVector = transform.forward;
            }
        }
        
        //
        // turnTo overrides
        //

        /// <summary>
        /// Turns to look at a GameObject.
        /// </summary>
        /// <param name="target">Target gameobject</param>
        protected void turnTo(GameObject target)
        {
            turnTo(target.transform.position);
        }

        /// <summary>
        /// Turns to look at a position.
        /// </summary>
        /// <param name="position">Target position</param>
        protected void turnTo(Vector3 position)
        {
            float angle = Vector3.SignedAngle(transform.forward, (position - transform.position).normalized, Vector3.up);
            turnTo(angle);
        }

        /// <summary>
        /// Turns a specified angle.
        /// </summary>
        protected void turnTo(float angle)
        {
            if (Mathf.Abs(angle) < turnSpeed * Time.deltaTime)
                transform.Rotate(Vector3.up, angle);
            else
            {
                if (angle > 0)
                    angle = turnSpeed * Time.deltaTime;
                else
                    angle = -turnSpeed * Time.deltaTime;

                transform.Rotate(Vector3.up, angle);
            }
        }
    }
}