using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
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

        public virtual void shoot(Vector3 spawnPos, Vector3 target)
        {
            spawnBullet(spawnPos, target);
        }

        protected GameObject spawnBullet(Vector3 spawnPos, Vector3 target, bool ignoreCooldown = false)
        {
            if (!canFire() && !ignoreCooldown)
                return null;

            GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject)), spawnPos, Quaternion.Euler(Vector3.zero)) as GameObject;
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.speed = speed;

            bullet.transform.LookAt(target); //point bullet at target

            if (debugging)
                Debug.DrawLine(spawnPos, target, Color.red, 2.0f);

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