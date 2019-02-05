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
        public bool timeEffected;

        public int maxAmmo;
        protected int currentAmmo;

        public bool debugging = false;

        private float currentCooldown = 0.0f;
        private float unlimitedAmmoTimer = 0.0f;

        /// <summary>
        /// Initialises a gun with unlimited ammo
        /// </summary>
        /// <param name="damage">Damage dealt by each bullet.</param>
        /// <param name="speed">Units travelled per second.</param>
        /// <param name="fireRate">Times this gun can fire per second.</param>
        /// <param name="timeEffected">Is fireRate affected by time manipulation?</param>
        public virtual void init(float damage, float speed, float fireRate, bool timeEffected)
        {
            this.damage = damage;
            this.speed = speed;
            this.fireRate = fireRate;
            this.timeEffected = timeEffected;

            giveUnlimitedAmmo();
        }

        /// <summary>
        /// Initialises a gun with a set ammount of ammo
        /// </summary>
        /// <param name="damage">Damage dealt by each bullet.</param>
        /// <param name="speed">Units travelled per second.</param>
        /// <param name="fireRate">Times this gun can fire per second.</param>
        /// <param name="timeEffected">Is fireRate affected by time manipulation?</param>
        /// <param name="maxAmmo">Maximum amount of ammo that can be stored.</param>
        /// <param name="startingAmmo">Amount of ammo this gun starts with.</param>
        public virtual void init(float damage, float speed, float fireRate, bool timeEffected, int maxAmmo, int startingAmmo = 0)
        {
            this.damage = damage;
            this.speed = speed;
            this.fireRate = fireRate;
            this.timeEffected = timeEffected;

            this.maxAmmo = maxAmmo;
            currentAmmo = startingAmmo;
        }

        /// <summary>
        /// Shoots the gun
        /// </summary>
        /// <param name="spawnPos">Location to spawn bullets from.</param>
        /// <returns>The bullet fired.</returns>
        public virtual GameObject shoot(Vector3 spawnPos)
        {
            return spawnBullet(spawnPos);
        }

        /// <summary>
        /// Spawns a single bullet.
        /// </summary>
        /// <param name="spawnPos">Location to spawn bullets from.</param>
        /// <param name="ignoreCooldown">Should a bullet be allowed to fire even if cooldown not complete?</param>
        /// <returns>The bullet fired.</returns>
        protected GameObject spawnBullet(Vector3 spawnPos, bool ignoreCooldown = false)
        {
            if (!canFire() && !ignoreCooldown)
                return null;

            GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject)), spawnPos, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.init(transform.position + (transform.forward * 100), damage, speed);
            bulletController.timeEffected = timeEffected;
            currentCooldown = 1 / fireRate;

            currentAmmo--;

            return bullet;
        }

        private void Update()
        {
            if (currentCooldown > 0.0f)
                currentCooldown -= Time.deltaTime;

            if (unlimitedAmmoTimer > 0.0f)
            {
                unlimitedAmmoTimer -= Time.deltaTime;

                if (unlimitedAmmoTimer <= 0.0f)
                {
                    setAmmo(maxAmmo);
                    unlimitedAmmoTimer = 0.0f;
                }
            }
        }

        /// <summary>
        /// Can this gun fire?
        /// </summary>
        /// <returns>True if able to fire</returns>
        protected bool canFire()
        {
            return currentCooldown <= 0.0f && currentAmmo > 0;
        }

        /// <summary>
        /// Returns how many ammo this gun has.
        /// </summary>
        public int getCurrentAmmo()
        {
            return currentAmmo;
        }

        /// <summary>
        /// Returns how many ammo this gun has as a percentage of maximum storage.
        /// </summary>
        public float getCurrentAmmoPerc()
        {
            return currentAmmo / maxAmmo;
        }

        /// <summary>
        /// Adds more ammo to the gun.
        /// </summary>
        /// <param name="ammo">Ammo to add.</param>
        public void addAmmo(int ammo)
        {
            currentAmmo += ammo;

            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;
        }

        /// <summary>
        /// Sets the current ammo count.
        /// </summary>
        /// <param name="ammo">New ammo count.</param>
        public void setAmmo(int ammo = 0)
        {
            currentAmmo = ammo;
        }

        /// <summary>
        /// Gives the gun unlimited ammo
        /// </summary>
        /// <param name="duration">Seconds to have unlimited ammo for (leave blank or 0 for infinite).</param>
        public void giveUnlimitedAmmo(float duration = 0.0f)
        {
            currentAmmo = int.MaxValue;

            unlimitedAmmoTimer = duration;
        }
    }
}