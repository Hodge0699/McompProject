using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class HomingGun : AbstractGun
    {
        public void Awake()
        {
            base.init(10, 10, 1.5f); // Infinite Ammo
        }

        protected override GameObject spawnBullet(Vector3 spawnPos, bool ignoreCooldown = false)
        {
            if (!canFire() && !ignoreCooldown)
                return null;

            GameObject bullet = Instantiate(Resources.Load("Missile", typeof(GameObject)), spawnPos, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
            bullet.transform.forward = this.transform.forward;

            MissleScript missileScr = bullet.GetComponent<MissleScript>();
            missileScr.MissleTarget = GameObject.FindGameObjectWithTag("Player").transform;

            currentCooldown = 1 / fireRate;
            currentAmmo--;

            return bullet;
        }
    }
}