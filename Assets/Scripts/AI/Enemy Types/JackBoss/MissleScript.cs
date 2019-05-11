using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleScript : BulletController
{
    public Transform MissleTarget;
    public Rigidbody MissleRigidbody;
    public float turn;
    public float MissleVelocity;
    public float LifeSpan = 10.0f; //time till destorying 

    protected override void FixedUpdate()
    {
        MissleRigidbody.velocity = transform.forward * MissleVelocity; //sets the velocity of the missle
        // sets up varible that will determin the rotation of the missile on the target location 
        var MissleTargetRotation = Quaternion.LookRotation(MissleTarget.position - transform.position);
        // sets rotation of missle 
        MissleRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, MissleTargetRotation, turn));

        lifetime += myTime.getDelta();
    }
}