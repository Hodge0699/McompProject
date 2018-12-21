using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Melee : MonoBehaviour
    {
        public float damage = 1.0f;

        public float attackFrequency = 3.0f;
        private float attackCooldown = 0.0f;

        private List<GameObject> collidingObjects = new List<GameObject>();

        private void Update()
        {
            if (attackCooldown > 0.0f)
                attackCooldown -= Time.deltaTime;

            if (collidingObjects.Count > 0 && attackCooldown <= 0.0f)
                attack();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                collidingObjects.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (collidingObjects.Contains(other.gameObject))
                collidingObjects.Remove(other.gameObject);
        }

        /// <summary>
        /// Attacks all colliding enemies
        /// </summary>
        private void attack()
        {
            for (int i = 0; i < collidingObjects.Count; i++)
                collidingObjects[i].GetComponent<PlayerHealthManager>().HurtPlayer(damage);

            attackCooldown = attackFrequency;
        }
    }
}