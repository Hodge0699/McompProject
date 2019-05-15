using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float damage; 
    public float speed = 6.0f;
    public float lifespan = 10.0f; // Seconds before despawning
    protected float lifetime = 0.0f;

    public List<string> ignoreTags = new List<string>();

    protected TimeMechanic.LocalTimeDilation myTime;

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

    protected void Start()
    {
        myTime = GetComponent<TimeMechanic.LocalTimeDilation>();
    }

    protected void Update()
    {
        if (lifetime >= lifespan)
            Destroy(gameObject);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate ()
    {
        transform.Translate(Vector3.forward * speed * myTime.getDelta());

        lifetime += myTime.getDelta();
    }

    /// <summary>
    /// Bounces the bullet off an object
    /// </summary>
    /// <param name="otherPos">Location of other object</param>
    private void bounce(Vector3 otherPos)
    {
        Vector3 bulletToEnemy = otherPos - transform.position;

        float dotProd = Mathf.Abs(Vector3.Dot(transform.forward, -bulletToEnemy));

        Vector3 newBulletForward = (dotProd * -bulletToEnemy) + ((1 - dotProd) * transform.forward);
        newBulletForward.Normalize();

        transform.LookAt(transform.position + (newBulletForward * 100));
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!ignoreTags.Contains(other.gameObject.tag))
        {
            HealthManager.HealthManager health = other.gameObject.GetComponent<HealthManager.HealthManager>();

            // Hit player or AI
            if (health != null)
            {
                health.hurt(damage);

                if (health.godmode) // Other in god mode, bounce
                {
                    bounce(other.transform.position);
                    return;
                }
            }
        }

        Destroy(gameObject);
    }
}
