using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Powerups
{
    public class AbstractPowerup : MonoBehaviour
    {
        public int ammo; // Amount of ammo to replenish
        protected float addHealth; // amount of health to replenish
        protected System.Type gun; // Type of gun to replenish

        /// <summary>
        /// Add ammo to gun when collided
        /// </summary>
        /// <param name="other">Colliding object (player).</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.GetComponent<HealthManager.HealthManager>() != null)
            {
                // Player/Enemy dead
                if (!other.GetComponent<HealthManager.HealthManager>().isAlive)
                    return;
            }

            if(addHealth > 0)
            {
                other.GetComponent<HealthManager.HealthManager>().addHealth(addHealth);
            }

            GunController gunController = other.GetComponentInChildren<GunController>();

            if (gunController != null)
            {
                gunController.addAmmo(gun, ammo);
                
            }
            Destroy(gameObject);
        }
    }
}
