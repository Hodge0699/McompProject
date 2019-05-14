using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossReturning : FinalBoss
    {
        private float centreSensitivity = 2.0f;

        // How fast the boss can charge
        private float returningMovementSpeed = 4;
        private float regularMovementSpeed;

        // Use this for initialization
        override protected void Start()
        {
            base.Start();

            sawBlade.isAccelerating = false;

            regularMovementSpeed = movementSpeed;
            movementSpeed = returningMovementSpeed;

            centreSensitivity *= transform.lossyScale.x;
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
            if (!isFacingCentre())
                turnTo(myRoom.transform.position);

            directionVector = transform.forward;
        }

        protected override void onBehaviourSwitch(AbstractEnemy newBehaviour)
        {
            movementSpeed = regularMovementSpeed;
        }

        private bool isFacingCentre()
        {
            float precision = 0.8f;

            Vector3 toCentre = (myRoom.transform.position - transform.position).normalized;

            return Vector3.Dot(transform.forward, toCentre) >= precision;
        }
    }
}