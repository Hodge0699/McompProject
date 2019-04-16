using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float damage; 
    public float speed = 12.0f;
    public float lifespan = 10.0f; // Seconds before despawning

    public bool timeEffected = true;

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
    protected virtual void FixedUpdate () {
        if(!timeEffected)
            transform.Translate(Vector3.forward * speed * Time.unscaledDeltaTime);
        else
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }


    void OnCollisionEnter(Collision other)
    {
        if (!ignoreTags.Contains(other.gameObject.tag))
        {
            HealthManager health = other.gameObject.GetComponent<HealthManager>();

            if (health != null)
                health.hurt(damage);
        }

        Destroy(gameObject);
    }
}
