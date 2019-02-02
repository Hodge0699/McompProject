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

        protected override void Awake()
        {
            base.Awake();

            movementSpeed *= 2;
        }

        private void Update()
        {
            Debug.Log(attackCooldownCounter);
            if (attackCooldownCounter >= 0.0f)
                attackCooldownCounter -= Time.deltaTime;

            if (target == null)
                wander();
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