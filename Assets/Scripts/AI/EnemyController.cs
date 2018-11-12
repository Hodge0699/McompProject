using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody myRB;
    public float moveSpeed;

    public PlayerController thePlayer;
    public Room myRoom;

    private bool isAlive = true;

    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<PlayerController>();
        GetComponent<EnemyHealthManager>().deathCallback = die;
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

        transform.LookAt(thePlayer.transform.position);
    }

    /// <summary>
    /// Kills the enemy (doesn't destroy gameobject, leaves it to be destroyed by room exit)
    /// </summary>
    private void die()
    {
        isAlive = false;

        Vector3 up = transform.position;
        up.y += 10.0f;

        transform.LookAt(up);

        myRoom.enemyKilled(this);
    }
}
