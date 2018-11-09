using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun.Powerups
{
    public class AbstractPowerup : MonoBehaviour
    {
        public float duration; // Length of time this gun will be active

        protected System.Type gun; // Type of gun to switch to

        /// <summary>
        /// Switch players gun when colliding.
        /// </summary>
        /// <param name="other">Colliding object (player).</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GunController GC = other.GetComponentInChildren<GunController>();

                GC.setGun(gun, duration);

                Destroy(gameObject);
            }
        }
    }
}
