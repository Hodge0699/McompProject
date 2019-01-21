﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using sceneTransitions;

namespace EnemyType
{
    public class AbstractEnemy : MonoBehaviour
    {
        public float health = 100;
        public float currentHealth;

        private bool isAlive = true;

        public float movementSpeed = 3.0f;

        public Vector3 directionVector; // Direction vector to act on at end of frame

        protected GameObject target; // The GameObject this agent is currently attacking

        private Room myRoom;

        protected VisionCone visionCone;

        public float maxDistance = 1.6f;

        // Use this for initialization
        protected virtual void Awake()
        {
            currentHealth = health;
            visionCone = GetComponent<VisionCone>();
        }

        private void FixedUpdate()
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
        /// Damages this enemy.
        /// </summary>
        /// <param name="damage">Amount of damage to inflict.</param>
        public void hurt(float damage, Transform attacker = null)
        {
            currentHealth -= (int)damage;

            if (currentHealth <= 0)
            {
                die();
                return;
            }

            // Look at player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        }

        /// <summary>
        /// Kills the enemy.
        /// </summary>
        private void die()
        {
            if (!isAlive)
                return;

            isAlive = false;

            myRoom.enemyKilled(this);
            
            gameObject.GetComponent<RandomPowerDrop>().CalculateLoot();

            onDeath();

            Destroy(gameObject);
        }

        /// <summary>
        /// Override if a specific enemy should do something special on death
        /// </summary>
        protected virtual void onDeath() { }

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


        //
        // Behaviours
        //

        /// <summary>
        /// Randomly sets the direction vector
        /// </summary>
        protected void wander()
        {
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 3.0f);

            if (hitInfo.collider)
                transform.Rotate(Vector3.up, Random.Range(140.0f, 220.0f));
            else
            {
                float rotation = Random.Range(-90.0f, 90.0f);
                transform.Rotate(Vector3.up, Mathf.Lerp(0.0f, rotation, Time.deltaTime));
            }

            directionVector = transform.forward;
        }

        /// <summary>
        /// Chases the target if there is one
        /// </summary>
        protected void chase()
        {
            if (target == null)
                return;

            if (Vector3.Distance(target.transform.position, this.transform.position) > maxDistance)
            {
                transform.LookAt(target.transform);
                directionVector = transform.forward;
            }
        }
    }
}