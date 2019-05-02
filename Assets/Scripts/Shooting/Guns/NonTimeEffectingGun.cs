using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class NonTimeEffectingGun : AbstractGun
    {
        public void Awake()
        {
            init(10, 10, 2.5f, 30);
        }

        protected override GameObject spawnBullet(Vector3 spawnPos, bool ignoreCooldown = false)
        {
            if (!canFire() && !ignoreCooldown)
                return null;

            GameObject bullet = Instantiate(Resources.Load("IgnoreTimeBullet", typeof(GameObject)), spawnPos, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.init(transform.position + (transform.forward * 100), damage, speed);
            currentCooldown = 1 / fireRate;

            currentAmmo--;

            return bullet;
        }
    }
}
