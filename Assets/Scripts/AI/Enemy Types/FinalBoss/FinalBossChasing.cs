using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossChasing : FinalBoss
    {
        private float stateDuration = 15.0f;
        private float stateDurationCounter = 0.0f;

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("Chasing");
            base.Start();

            // Make boss vulnerable to bullets
            GetComponent<HealthManager.HealthManager>().setGodmode(false);

            sawBlade.isAccelerating = true;
            myTime.enabled = false;
        }

        protected override Type decideState()
        {
            if (stateDurationCounter >= stateDuration) // Timer ran out
                return typeof(FinalBossReturning);
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            chase(player);

            // Use unscaled time since bubbles can't affect chasing boss
            stateDurationCounter += Time.unscaledDeltaTime;
        }

        protected override void onStateSwitch(FinalBoss newState)
        {
            myTime.enabled = true;
            GetComponent<HealthManager.HealthManager>().setGodmode(true);
        }
    }
}