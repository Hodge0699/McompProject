using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthManager
{
    public class HealthManager : MonoBehaviour
    {
        public float startingHealth;
        [SerializeField]
        protected float currentHealth;

        protected CapsuleCollider cCollider;
        protected Rigidbody rigid;
        protected Animator anim;

        public bool godmode = false;
        private float godmodeTimer = -1.0f;

        public bool debugging = false;

        public bool isAlive { get; private set; }

        private void Awake()
        {
            cCollider = this.GetComponent<CapsuleCollider>();
            rigid = this.GetComponent<Rigidbody>();
            currentHealth = startingHealth;
            isAlive = currentHealth > 0;
            anim = GetComponent<Animator>();
        }

        public void Update()
        {
            if (godmodeTimer > 0.0f)
            {
                godmodeTimer -= Time.deltaTime;

                if (godmodeTimer <= 0.0f)
                    setGodmode(false);
            }
        }

        /// <summary>
        /// Damages the character by a set amount
        /// </summary>
        /// <param name="damageAmount">Damage to inflict</param>
        public virtual void hurt(float damageAmount, bool ignoreGodmode = false)
        {
            if ((godmode && !ignoreGodmode) || !isAlive)
                return;

            currentHealth -= damageAmount;

            isAlive = currentHealth > 0;
        }

        /// <summary>
        /// Makes the player invincible (useful for when player controls are taken away)
        /// </summary>
        /// <param name="duration">Seconds invincibility will last for</param>
        public void setGodmode(bool godmode = true, float duration = 0)
        {
            // Don't set a timed godmode if unlimited is already on
            if (this.godmode && godmodeTimer == -1.0f)
                return;

            this.godmode = godmode;

            godmodeTimer = duration;

            if (debugging)
                Debug.Log("Godmode set to " + godmode);
        }


        /// <summary>
        /// Sets the health of this character.
        /// </summary>
        /// <param name="health">The value to set it to.</param>
        /// <param name="force">True to force the value, false to let it cap if too high.</param>
        public virtual void setHealth(float health, bool force = false)
        {
            if (force)
                currentHealth = health;
            else
                currentHealth = startingHealth;

            isAlive = currentHealth > 0;
        }

        public void addHealth(float health)
        {
            if (currentHealth + health > 200)
            {
                float temp;
                temp = 200 - currentHealth;
                currentHealth = currentHealth + temp;
            }
            currentHealth = currentHealth + health;
        }

        /// <summary>
        /// Returns the current health of the character
        /// </summary>
        public float getHealth()
        {
            return currentHealth;
        }

        /// <summary>
        /// Returns 
        /// </summary>
        public float getStartingHealth()
        {
            return startingHealth;
        }

        /// <summary>
        /// Returns the current health as a percentage of the starting health
        /// </summary>
        /// <returns></returns>
        public float getHealthPercentage()
        {
            return currentHealth / startingHealth;
        }
    }
}