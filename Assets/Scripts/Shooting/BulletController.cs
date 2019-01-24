using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float damage; 
    public float speed = 12.0f;
    public float lifespan = 10.0f; // Seconds before despawning

    public List<string> ignoreTags = new List<string>();

    /// <summary>
    /// Initialises damage, speed and lifespan
    /// </summary>
    public void init(Vector3 forward, float damage, float speed = 12.0f, float lifespan = 10.0f)
    {
        transform.LookAt(forward);

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
        if (other.gameObject.tag == "Enemy" && !ignoreTags.Contains("Enemy"))
            other.gameObject.GetComponent<EnemyType.AbstractEnemy>().hurt(damage, this.transform);
        else if (other.gameObject.tag == "Player" && !ignoreTags.Contains("Player"))
            other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damage);
        else if (other.gameObject.tag == "Boss" && !ignoreTags.Contains("Boss"))
            other.gameObject.GetComponent<EnemyType.AbstractEnemy>().hurt(damage, this.transform);
        Destroy(gameObject);
    }
}
