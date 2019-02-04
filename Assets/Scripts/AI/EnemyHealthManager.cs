using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthManager : HealthManager {

    [Header("Enemy Health")]
    public Image healthBar;

    public override void hurt(float damageAmount)
    {
        base.hurt(damageAmount);

        healthBar.fillAmount = base.currentHealth / base.startingHealth;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            transform.LookAt(player.transform);

    }
}
