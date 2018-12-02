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
    public GameObject damageImageFinder;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    bool damaged;
    public GameObject healthUI;

    // Use this for initialization
    void Start()
    {
        healthUI = GameObject.FindGameObjectWithTag("HealthUI");
        currentHealth = startingHealth;
        healthSlider = healthUI.GetComponentInChildren<Slider>();
        damageImageFinder = GameObject.FindGameObjectWithTag("DamageImage");
        damageImage = damageImageFinder.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }


    }

    public void HurtPlayer(int damageAmount)
    {
        damaged = true;
        currentHealth -= damageAmount;
        healthSlider.value = currentHealth;
    }


}

