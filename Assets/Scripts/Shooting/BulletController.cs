using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float damage; 
    public float speed = 12.0f;
    public float lifespan = 10.0f; // Seconds before despawning

    /// <summary>
    /// Initialises damage, speed and lifespan
    /// </summary>
    public void init(float damage, float speed = 12.0f, float lifespan = 10.0f)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifespan = lifespan;
    }

    private void Awake()
    {
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().hurt(damage);
            Destroy(gameObject);
        }
    }
}
