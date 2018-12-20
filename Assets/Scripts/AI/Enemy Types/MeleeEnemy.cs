using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class MeleeEnemy : AbstractEnemy
    {
        public float attackCooldown = 3.0f;
        private float attackCooldownCounter = 0.0f;

        public float attackRange = 1.0f;

        public float attackDamage = 1.0f;

        private void Update()
        {
            if (attackCooldownCounter >= 0.0f)
                attackCooldownCounter -= Time.deltaTime;

            if (target == null)
                wander();
            else
            {
                chase();

                if (targetWithinRange() && attackCooldownCounter <= 0.0f)
                    target.GetComponent<PlayerHealthManager>().HurtPlayer(attackDamage);
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