using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossReturning : FinalBoss
    {
        private float centreSensitivity = 2.0f;

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("Returning");
            base.Start();

            sawBlade.isAccelerating = false;
        }

        protected override Type decideState()
        {
            float distanceToCentre = (myRoom.transform.position - transform.position).magnitude;

            if (distanceToCentre <= centreSensitivity) // Reached centre
                return typeof(FinalBossTracking); 
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            turnTo(myRoom.transform.position);
            directionVector = transform.forward;
        }

    }
}