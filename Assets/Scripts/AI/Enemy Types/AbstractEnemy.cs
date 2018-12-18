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

        // Use this for initialization
        void Start()
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

            RandomPowerDrop RPD = gameObject.AddComponent<RandomPowerDrop>();
            RPD.CalculateLoot();

            Destroy(gameObject);
        }

        /// <summary>
        /// Links this enemy to a room
        /// </summary>
        public void setRoom(Room room)
        {
            myRoom = room;
        }
    }
}