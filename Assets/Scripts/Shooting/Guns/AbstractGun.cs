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

        public int maxAmmo;
        protected int currentAmmo;

        public bool debugging = false;

        protected float currentCooldown = 0.0f;
        protected float unlimitedAmmoTimer = 0.0f;

        protected LocalTimeDilation myTime;
        protected TimeStop timeStop;

        private void Start()
        {
            myTime = GetComponentInParent<LocalTimeDilation>();
            timeStop = GetComponentInParent<TimeStop>();
        }

        /// <summary>
        /// Initialises a gun with unlimited ammo
        /// </summary>
        /// <param name="damage">Damage dealt by each bullet.</param>
        /// <param name="speed">Units travelled per second.</param>
        /// <param name="fireRate">Times this gun can fire per second.</param>
        public virtual void init(float damage, float speed, float fireRate)
        {
            this.damage = damage;
            this.speed = speed;
            this.fireRate = fireRate;

            giveInfiniteAmmo();
        }

        /// <summary>
        /// Initialises a gun with a set ammount of ammo
        /// </summary>
        /// <param name="damage">Damage dealt by each bullet.</param>
        /// <param name="speed">Units travelled per second.</param>
        /// <param name="fireRate">Times this gun can fire per second.</param>
        /// <param name="maxAmmo">Maximum amount of ammo that can be stored.</param>
        /// <param name="startingAmmo">Amount of ammo this gun starts with.</param>
        public virtual void init(float damage, float speed, float fireRate, int maxAmmo, int startingAmmo = 0)
        {
            this.damage = damage;
            this.speed = speed;
            this.fireRate = fireRate;

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
        protected virtual GameObject spawnBullet(Vector3 spawnPos, bool ignoreCooldown = false)
        {
            if (!canFire() && !ignoreCooldown)
                return null;

            GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject)), spawnPos, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.init(transform.position + (transform.forward * 100), damage, speed);
            currentCooldown = 1 / fireRate;

            // Time stop
            if (timeStop != null)
                bullet.GetComponent<LocalTimeDilation>().setDilation(timeStop.isStopped ? 0.0f : 1.0f);
            else
                bullet.GetComponent<LocalTimeDilation>().setDilation(myTime.getDilation());


            currentAmmo--;

            return bullet;
        }

        private void Update()
        {
            if (timeStop != null && timeStop.isStopped)
                return;

            if (currentCooldown > 0.0f)
                currentCooldown -= myTime.getDelta();

            if (unlimitedAmmoTimer > 0.0f)
            {
                unlimitedAmmoTimer -= myTime.getDelta();

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
        /// Tests if the gun has any ammo
        /// </summary>
        /// <returns>True if ammo left</returns>
        public bool hasAmmo()
        {
            return currentAmmo > 0;
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
        /// Gives the gun infinite ammo
        /// </summary>
        /// <param name="duration">Seconds to have infinite ammo for (leave blank or 0 for infinite).</param>
        public void giveInfiniteAmmo(float duration = 0.0f)
        {
            currentAmmo = int.MaxValue;

            unlimitedAmmoTimer = duration;
        }
    }
}