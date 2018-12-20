using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    bool damaged;

    private bool initialised = false;

    private bool godmode = false;
    private float godmodeTimer = 0.0f;

    public bool debugging = false;

    /// <summary>
    /// Finds ui elements 
    /// </summary>
    /// <param name="ui">UI Canvas object</param>
    public void init(GameObject ui)
    {
        currentHealth = startingHealth;

        healthSlider = ui.transform.Find("HealthUI").transform.Find("HealthSlider").GetComponent<Slider>();

        damageImage = ui.transform.Find("DamageImage").GetComponent<Image>();

        initialised = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised)
            return;

        if(damaged)
            damageImage.color = flashColour;
        else
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

        damaged = false;

        if (currentHealth <= 0)
            gameObject.SetActive(false);

        if (godmodeTimer > 0.0f)
        {
            godmodeTimer -= Time.deltaTime;

            if (godmodeTimer <= 0.0f)
                setGodmode(false);
        }
    }

    /// <summary>
    /// Damages the player by a set amount
    /// </summary>
    /// <param name="damageAmount">Damage to inflict</param>
    public void HurtPlayer(float damageAmount)
    {
        if (godmode)
            return;

        damaged = true;
        currentHealth -= damageAmount;
        healthSlider.value = currentHealth;
    }

    /// <summary>
    /// Makes the player invincible (useful for when player controls are taken away)
    /// </summary>
    /// <param name="duration">Seconds invincibility will last for</param>
    public void setGodmode(bool godmode = true, float duration = 0)
    {
        this.godmode = godmode;

        godmodeTimer = duration;

        if (debugging)
            Debug.Log("Godmode set to " + godmode);
    }
}