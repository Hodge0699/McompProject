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
            /// Not needed anymore because there is no hybird - Nicky
            // Make sure all ammo is empty just in case this enemy is hybrid
            gunController.getGun(typeof(Weapon.Gun.Handgun)).setAmmo(0);
            ///
        }

        private void Update()
        {
            /// Not needed anymore because there is no hybird - Nicky
            // Switch to GunEnemy if has ammo
            //if (usePickups && gunController.hasAmmo(true))
            //    switchToBehaviour(typeof(GunEnemy), true, false);
            ///
            if (attackCooldownCounter >= 0.0f)
                attackCooldownCounter -= Time.deltaTime;

            if (target != null)
            {
                if (getDistanceToTarget() >= 2.0f)
                    chaseTarget();
                if (targetWithinRange() && attackCooldownCounter <= 0.0f)
                {
                    if (anim != null)
                    {
                        //reset animation trigger before starting a new one to prevent being in two stages at once.
                        anim.ResetTrigger("Chasing");
                        anim.ResetTrigger("PlayerDead");
                        anim.SetTrigger("Melee");
                    }
                    target.GetComponent<HealthManager>().hurt(attackDamage);
                    attackCooldownCounter = attackCooldown;
                }
            }
            else
            {
                /// melee won't pick up anymore as we have no hybid - Nicky
                if (usePickups && pickUpVisionCone.hasVisibleTargets())
                    goToPosition(pickUpVisionCone.getClosestVisibleTarget().transform.position);
                wanderForTarget();
            }
        }
        /// <summary>
        /// tells the AI to start chasing a Target
        /// </summary>
        private void chaseTarget()
        {
            if (anim != null)
                anim.SetTrigger("Chasing");
            chase();
        }
        /// <summary>
        /// tells the AI to start wondering around if it cannot find an Enemy
        /// </summary>
        private void wanderForTarget()
        {
            /// This code can be used if we further develope the AI to stop for random amounts of seconds, or if it gets advanced enough to know when a player dies.
            //if (anim != null)
            //{
            //    anim.ResetTrigger("Chasing");
            //    anim.SetTrigger("PlayerDead");
            //}
            if (anim != null)
                anim.SetTrigger("Chasing");
            wander();
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