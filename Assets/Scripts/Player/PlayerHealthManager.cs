using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    bool damaged;

    private bool initialised = false;

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
    }

    public void HurtPlayer(int damageAmount)
    {
        damaged = true;
        currentHealth -= damageAmount;
        healthSlider.value = currentHealth;
    }
}