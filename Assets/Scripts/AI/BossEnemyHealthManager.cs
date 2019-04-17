using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnemyType;

public class BossEnemyHealthManager : HealthManager
{
    [Header("Enemy Health")]
    public Image healthBar;
    [SerializeField]
    private Image healthSlider;

    public bool lookAtPlayerOnHit = true;
    private float deathAnimationDuration = 6.0f;


    // Player UI GameObjects
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private GameObject BossUI;


    new void Update()
    {
        if (playerUI == null)
        {
            playerUI = GameObject.Find("PlayerUI(Clone)");
            BossUI = playerUI.transform.Find("BossHealth").gameObject;
            BossUI.SetActive(true);
            healthSlider = BossUI.transform.Find("Bars").transform.Find("Healthbar").GetComponent<Image>();
        }
        if (anim != null)
        {
            if (deathAnimationDuration <= 2)
            {
                deathAnimationDuration -= Time.deltaTime;
            }
            if (deathAnimationDuration <= 0)
                Destroy(gameObject);
        }
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

        //healthBar.fillAmount = base.currentHealth / base.startingHealth;
        healthSlider.fillAmount = currentHealth / 100;
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
        EnemyType.AbstractEnemy me = GetComponent<EnemyType.AbstractEnemy>();

        if (me != null)
        {
            if (me.getRoom() != null)
                me.getRoom().enemyKilled(me);

            if (gameObject.GetComponent<RandomPowerDrop>() != null)
                gameObject.GetComponent<RandomPowerDrop>().CalculateLoot();

            me.onDeath();
        }

        if (anim != null)
        {
            anim.SetTrigger("Dead");
            deathAnimationDuration = 2;
            Destroy(GetComponent<AbstractEnemy>());
        }
        else
            Destroy(gameObject);
    }
}
