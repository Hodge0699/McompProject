using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 12.0f;
    public float lifespan = 10.0f;
    public int damage;

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
            other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(damage);
            Destroy(gameObject);
        }
    }

}
