using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using EnemyType;

public class EnemyHealthManager : HealthManager {

    [Header("Enemy Health")]
    public Image healthBar;


    public bool lookAtPlayerOnHit = true;
    private float deathAnimationDuration = 6;


    new void Update()
    {

        if (deathAnimationDuration <= 2)
        {
            Debug.Log("Timer: " + deathAnimationDuration);
            deathAnimationDuration -= Time.deltaTime;
        }
        if (deathAnimationDuration <= 0)
            destroyGameObject();
    }

    /// <summary>
    /// Enemy takes damage
    /// </summary>
    /// <param name="damageAmount"></param>
    public override void hurt(float damageAmount)
    {
        if (!isAlive)
            return;

        base.hurt(damageAmount);

        healthBar.fillAmount = base.currentHealth / base.startingHealth;

        if (!isAlive)
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
        anim.SetTrigger("Dead");
        EnemyType.AbstractEnemy me = GetComponent<EnemyType.AbstractEnemy>();
        
        if(GetComponent<GunEnemy>() != null)
            Destroy(GetComponent<GunEnemy>());
        if(GetComponent<MeleeEnemy>() != null)
            Destroy(GetComponent<MeleeEnemy>());


        if (me != null)
        {
            if (me.getRoom() != null)
                me.getRoom().enemyKilled(me);
            
            if (gameObject.GetComponent<RandomPowerDrop>() != null)
                gameObject.GetComponent<RandomPowerDrop>().CalculateLoot();

            me.onDeath();
        }

        deathAnimationDuration = 2;
    }

    private void destroyGameObject()
    {
        Debug.Log("something");
        Destroy(gameObject);
    }
}
