using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;
using Weapon.Gun;

namespace Powerups
{
    public class AbstractPowerup : MonoBehaviour
    {
        public int ammo; // Amount of ammo to replenish

        protected System.Type gun; // Type of gun to replenish

        /// <summary>
        /// Add ammo to gun when collided
        /// </summary>
        /// <param name="other">Colliding object (player).</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            GunController gunController = other.GetComponentInChildren<GunController>();

            if (gunController != null && other.GetComponent<GunEnemy>() != null || gunController != null && other.GetComponent<MeleeEnemy>() != null)
            {
                //gunController.setGun(gun, duration);
                gunController.addAmmo(gun, ammo);
                Destroy(gameObject);
            }
        }


    }
}
