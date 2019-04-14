using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealthManager : HealthManager
    {
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        public GameObject deathScene;

        private bool damaged;
        private Image damageImage;
        private Image healthSlider;

        private bool initialised = false;

        /// <summary>
        /// Finds ui elements 
        /// </summary>
        /// <param name="ui">UI Canvas object</param>
        public void init(GameObject ui)
        {
            healthSlider = ui.transform.Find("Elite").transform.Find("Bars").transform.Find("Healthbar").GetComponent<Image>();

            damageImage = ui.transform.Find("DamageImage").GetComponent<Image>();
            deathScene = ui.transform.Find("DeathScene").gameObject;

            initialised = true;
        }

        // Update is called once per frame
        new void Update()
        {
            if (!initialised)
                return;

            base.Update();

            if (damaged)
                damageImage.color = flashColour;
            else
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.unscaledDeltaTime);

            damaged = false;

            if (!isAlive)
            {
                gameObject.SetActive(false);
                deathScene.SetActive(true);
            }
        }

        /// <summary>
        /// Inflicts damage and sets new value on health slider.
        /// </summary>
        /// <param name="damageAmount">Damage to inflict.</param>
        public override void hurt(float damageAmount)
        {
            base.hurt(damageAmount);

            if (!godmode)
            {
                damaged = true;
                healthSlider.fillAmount = currentHealth /100;
                //Debug.Log("health slider amount: " + healthSlider.fillAmount);
            }
        }

        /// <summary>
        /// Sets the health of the player and updates UI.
        /// </summary>
        /// <param name="health">The value to set it to.</param>
        /// <param name="force">True to force the value, false to let it cap if too high.</param>
        public override void setHealth(float health, bool force = false)
        {
            base.setHealth(health, force);

            healthSlider.fillAmount = currentHealth;
        }
    }
}