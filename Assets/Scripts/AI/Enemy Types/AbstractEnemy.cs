using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class AbstractEnemy : MonoBehaviour
    {
        public int health = 100;
        private int currentHealth;

        private bool isAlive = true;

        public float movementSpeed = 3.0f;

        public Vector3 directionVector; // Direction vector to act on at end of frame

        protected PlayerController player;
        private Room myRoom;

        protected VisionCone visionCone;
        protected PathFollower pathFollower;

        // Use this for initialization
        void Awake()
        {
            currentHealth = health;
            player = FindObjectOfType<PlayerController>();
            visionCone = GetComponent<VisionCone>();
        }

        private void FixedUpdate()
        {
            directionVector.Normalize();
            Vector3 movement = directionVector * movementSpeed * Time.deltaTime;

            transform.Translate(movement, Space.World);

            directionVector = Vector3.zero;

        }

        /// <summary>
        /// Damages this enemy.
        /// </summary>
        /// <param name="damage">Amount of damage to inflict.</param>
        public void hurt(float damage)
        {
            currentHealth -= (int)damage;

            if (currentHealth <= 0)
                die();
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

            Destroy(gameObject);
        }

        /// <summary>
        /// Links this enemy to a room
        /// </summary>
        public void setRoom(Room room)
        {
            myRoom = room;
        }

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
    }
}