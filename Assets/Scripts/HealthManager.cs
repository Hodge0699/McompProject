using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float startingHealth;
    protected float currentHealth;

    public bool godmode = false;
    private float godmodeTimer = -1.0f;

    public bool debugging = false;

    private void Awake()
    {
        currentHealth = startingHealth;
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
    public virtual void hurt(float damageAmount)
    {
        if (godmode)
            return;

        currentHealth -= damageAmount;
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
    /// Returns true if the character is dead
    /// </summary>
    public bool isDead()
    {
        return (currentHealth <= 0);
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
