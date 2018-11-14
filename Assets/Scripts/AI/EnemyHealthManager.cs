using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public float health;
    private float currentHealth;

    public RandomPowerDrop RPD;
    public Action deathCallback;

    // Use this for initialization
    void Start()
    {

        currentHealth = health;

    }

    public void HurtEnemy(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            deathCallback();
            Destroy(gameObject);
            RPD.CalculateLoot();
            
    }
}
