using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon.Gun;

namespace Powerups
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
            GunController gunController = other.GetComponentInChildren<GunController>();

            if (gunController != null)
            {
                gunController.setGun(gun, duration);

                Destroy(gameObject);
            }
        }
    }
}
