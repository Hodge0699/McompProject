using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
<<<<<<< HEAD
    public class PlayerHealthManager : HealthManager
    {
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        public GameObject deathScene;
=======
    public float startingHealth;
    public float currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    bool damaged;

    private bool initialised = false;
>>>>>>> parent of ba59760... Merge branch 'master' into Jack-Branch

        private bool damaged;
        private Image damageImage;
        private Slider healthSlider;

        private bool initialised = false;

        /// <summary>
        /// Finds ui elements 
        /// </summary>
        /// <param name="ui">UI Canvas object</param>
        public void init(GameObject ui)
        {
            healthSlider = ui.transform.Find("HealthUI").transform.Find("HealthSlider").GetComponent<Slider>();

            damageImage = ui.transform.Find("DamageImage").GetComponent<Image>();
            deathScene = ui.transform.Find("DeathScene").gameObject;

<<<<<<< HEAD
            initialised = true;
        }
=======
        damageImage = ui.transform.Find("DamageImage").GetComponent<Image>();
>>>>>>> parent of ba59760... Merge branch 'master' into Jack-Branch

        // Update is called once per frame
        new void Update()
        {
            if (!initialised)
                return;

            base.Update();

            if (damaged)
                damageImage.color = flashColour;
            else
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

            damaged = false;

<<<<<<< HEAD
            if (isDead())
            {
                gameObject.SetActive(false);
                deathScene.SetActive(true);
            }
        }
=======
        if (currentHealth <= 0)
            gameObject.SetActive(false);
>>>>>>> parent of ba59760... Merge branch 'master' into Jack-Branch

        /// <summary>
        /// Inflicts damage and sets new value on health slider.
        /// </summary>
        /// <param name="damageAmount">Damage to inflict.</param>
        public override void hurt(float damageAmount)
        {
            base.hurt(damageAmount);

            damaged = true;
            healthSlider.value = currentHealth;
        }
<<<<<<< HEAD

        /// <summary>
        /// Sets the health of the player and updates UI.
        /// </summary>
        /// <param name="health">The value to set it to.</param>
        /// <param name="force">True to force the value, false to let it cap if too high.</param>
        public override void setHealth(float health, bool force = false)
        {
            base.setHealth(health, force);
=======
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
>>>>>>> parent of ba59760... Merge branch 'master' into Jack-Branch

            healthSlider.value = currentHealth;
        }
    }
}