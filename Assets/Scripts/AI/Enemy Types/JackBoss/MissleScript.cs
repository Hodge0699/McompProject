using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleScript : MonoBehaviour
{
    public Transform MissleTarget;
    public Rigidbody MissleRigidbody;


    public float turn;
    public float MissleVelocity;
    public float LifeSpan = 10.0f; //time till destorying 


    private void FixedUpdate()
    {
        MissleRigidbody.velocity = transform.forward * MissleVelocity; //sets the velocity of the missle

        var MissleTargetRotation = Quaternion.LookRotation(MissleTarget.position - transform.position); // sets up varible that will determin the rotation of the missile on the target location 

        MissleRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, MissleTargetRotation, turn)); // sets rotation of missle 
        
    }

   void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.gameObject)
        {
            Destroy(gameObject);
        }
    }


}