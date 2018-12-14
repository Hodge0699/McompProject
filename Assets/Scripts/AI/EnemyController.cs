using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody myRB;
    public float moveSpeed;

    public PlayerController player;
    public Room myRoom;

    private bool isAlive = true;

    public float health = 100.0f;
    private float currentHealth;

    private RandomPowerDrop RPD;

    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
        RPD = GetComponent<RandomPowerDrop>();

        currentHealth = health;
    }

    void FixedUpdate()
    {
        if (!isAlive)
            return;

        myRB.velocity = (transform.forward * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        transform.LookAt(player.transform.position);
    }

    /// <summary>
    /// Hurts the enemy
    /// </summary>
    /// <param name="damage">Damage to hurt by</param>
    /// <returns>The remaining health</returns>
    public float hurt(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0.0f;
            die();
        }

        return currentHealth;
    }

    /// <summary>
    /// Kills the enemy.
    /// </summary>
    private void die()
    {
        if (!isAlive)
            return;

        isAlive = false;

        myRoom.enemyKilled(this);

        RPD.CalculateLoot();

        Destroy(gameObject);
    }
}
