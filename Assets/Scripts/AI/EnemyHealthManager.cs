using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthManager : HealthManager {

    [Header("Enemy Health")]
    public Image healthBar;

    public bool lookAtPlayerOnHit = true;

    public override void hurt(float damageAmount)
    {
        base.hurt(damageAmount);

        healthBar.fillAmount = base.currentHealth / base.startingHealth;

        if (currentHealth <= 0.0f)
        {
            die();
            return;
        }
        
        if (lookAtPlayerOnHit)
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

        if (me != null)
        {
            if (me.getRoom() != null)
                me.getRoom().enemyKilled(me);
            
            if (gameObject.GetComponent<RandomPowerDrop>() != null)
                gameObject.GetComponent<RandomPowerDrop>().CalculateLoot();

            me.onDeath();
        }

        Destroy(gameObject);
    }
}
