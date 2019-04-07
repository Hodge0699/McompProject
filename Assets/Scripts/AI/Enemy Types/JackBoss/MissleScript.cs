using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleScript : MonoBehaviour
{
    public Transform target;

    public float speed = 5f;
    public float rotateSpeed = 200f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        Vector3 direction = target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    public void Shoot()
    {

    }

<<<<<<< HEAD
   void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.gameObject)
        {
            Destroy(gameObject);
        }
    }


=======
>>>>>>> parent of b8666c7... homing missle 0.2
}