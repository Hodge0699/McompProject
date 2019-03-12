using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class MeleeEnemy : AbstractEnemy
    {
        public float attackCooldown = 1.0f;
        private float attackCooldownCounter = 0.0f;

        public float attackRange = 2.0f;

        public float attackDamage = 10.0f;

        public bool usePickups = false; // Should this enemy try to find a gun?

        protected override void Awake()
        {
            base.Awake();

            movementSpeed = 6.0f;

            // Make sure all ammo is empty just in case this enemy is hybrid
            gunController.getGun(typeof(Weapon.Gun.Handgun)).setAmmo(0);
        }

        private void Update()
        {
            // Switch to GunEnemy if has ammo
            if (usePickups && gunController.hasAmmo(true))
                switchToBehaviour(typeof(GunEnemy), true, false);

            if (attackCooldownCounter >= 0.0f)
                attackCooldownCounter -= Time.deltaTime;

            if (target == null)
            {
                if (usePickups && pickUpVisionCone.hasVisibleTargets())
                    goToPosition(pickUpVisionCone.getClosestVisibleTarget().transform.position);
                else
                    wander();
            }
            else
            {
                chase();

                if (targetWithinRange() && attackCooldownCounter <= 0.0f)
                {
                    target.GetComponent<HealthManager>().hurt(attackDamage);
                    attackCooldownCounter = attackCooldown;
                }
            }
        }

        /// <summary>
        /// Tests if the target is within range
        /// </summary>
        /// <returns>True if distance to target is less than attackRange.</returns>
        private bool targetWithinRange()
        {
            if (target == null)
                return false;

            return getDistanceToTarget() <= attackRange;
        }
    }
}