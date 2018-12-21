using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public abstract class AbstractGun : MonoBehaviour
    {
        public float damage;
        public float speed;
        public float fireRate;

        public bool debugging = false;

        private float currentCooldown = 0.0f;

        public virtual void init(float damage, float speed, float fireRate)
        {
            this.damage = damage;
            this.speed = speed;
            this.fireRate = fireRate;
        }

        public virtual GameObject shoot(Vector3 spawnPos)
        {
            return spawnBullet(spawnPos);
        }

        protected GameObject spawnBullet(Vector3 spawnPos, bool ignoreCooldown = false)
        {
            if (!canFire() && !ignoreCooldown)
                return null;

            GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject)), spawnPos, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.init(transform.position + (transform.forward * 100), damage, speed);

            currentCooldown = 1 / fireRate;

            return bullet;
        }

        private void Update()
        {
            if (currentCooldown > 0.0f)
                currentCooldown -= Time.deltaTime;
        }

        protected bool canFire()
        {
            return !(currentCooldown > 0.0f);
        }
    }
}