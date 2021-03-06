﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using EnemyType;

namespace HealthManager
{
    public class EnemyHealthManager : HealthManager
    {

        [Header("Enemy Health")]
        public Image healthBar;


        public bool lookAtPlayerOnHit = true;
        protected float deathAnimationDuration = 6.0f;

        new void Update()
        {
            if (anim != null)
            {
                if (deathAnimationDuration <= 2)
                    deathAnimationDuration -= Time.deltaTime;

                if (deathAnimationDuration <= 0)
                    Destroy(gameObject);
            }
        }

        /// <summary>
        /// Enemy takes damage
        /// </summary>
        /// <param name="damageAmount"></param>
        public override void hurt(float damageAmount, bool ignoreGodemode = false)
        {
            if (!isAlive)
                return;

            base.hurt(damageAmount, ignoreGodemode);

            healthBar.fillAmount = currentHealth / startingHealth;

            if (!isAlive)
            {
                die();
                return;
            }

            if (lookAtPlayerOnHit)
            {
                // look at player
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    Vector3 targetPos = player.transform.position;
                    targetPos.y = this.transform.position.y;

                    transform.LookAt(targetPos);
                }
            }
        }


        /// <summary>
        /// Kills the enemy.
        /// </summary>
        private void die()
        {
            EnemyType.AbstractEnemy me = GetComponent<EnemyType.AbstractEnemy>();

            if (me != null)
            {
                if (me.getRoom() != null)
                    me.getRoom().enemyKilled(me);

                if (gameObject.GetComponent<RandomPowerDrop>() != null)
                    gameObject.GetComponent<RandomPowerDrop>().CalculateLoot();
                cCollider.enabled = false;
                rigid.useGravity = false;
                me.onDeath();
            }

            if (anim != null)
            {
                anim.SetTrigger("Dead");
                deathAnimationDuration = 2;
                Destroy(GetComponent<AbstractEnemy>());
            }
            else
                Destroy(gameObject);
        }
    }
}