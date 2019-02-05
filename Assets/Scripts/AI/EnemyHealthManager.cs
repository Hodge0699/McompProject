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

        if (currentHealth <= 0.0f)
        {
            die();
            return;
        }
        else
        {
            // look at player
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Vector3 targetPos = player.transform.position;
                targetPos.y = this.transform.position.y;

                transform.LookAt(targetPos);
            }
        }
    }


    /// <summary>
    /// Kills the enemy.
    /// </summary>
    private void die()
    {
        EnemyType.AbstractEnemy me = GetComponent<EnemyType.AbstractEnemy>();

        if (me.getRoom() != null)
            me.getRoom().enemyKilled(me);

        gameObject.GetComponent<RandomPowerDrop>().CalculateLoot();

        me.onDeath();

        Destroy(gameObject);
    }
}
