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
            if (visionCone.hasVisibleTargets())
            {
                GameObject target = visionCone.getClosestVisibleTarget();
                transform.LookAt(target.transform);
                directionVector = transform.forward;

                if ((target.transform.position - this.transform.position).magnitude <= attackRange && attackCooldownCounter == 0.0f)
                {
                    target.GetComponent<PlayerHealthManager>().HurtPlayer(attackDamage);
                }
            }
        }
    }
}