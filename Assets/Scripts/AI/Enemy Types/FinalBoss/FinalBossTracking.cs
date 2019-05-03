using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossTracking : FinalBoss
    {
        private float stateDuration = 8.0f;
        private float stateDurationCounter = 0.0f;

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("Tracking");
            base.Start();

            sawBlade.isAccelerating = true;
        }

        protected override Type decideState()
        {
            if (stateDurationCounter >= stateDuration) // Timer ran out
                return typeof(FinalBossCharging);
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            stateDurationCounter += myTime.getDelta();

            turnTo(player);
        }
    }
}