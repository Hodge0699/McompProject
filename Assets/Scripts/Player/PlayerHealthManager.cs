using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{

    public int startingHealth;
    public int currentHealth;


    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }


    }

    public void HurtPlayer(int damageAmount)
    {
        currentHealth -= damageAmount;
    }


}

