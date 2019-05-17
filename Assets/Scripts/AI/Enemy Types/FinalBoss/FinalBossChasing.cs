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

        private float colourResetTime = 2.0f;

        // Use this for initialization
        override protected void Start()
        {
            base.Start();

            // Make boss vulnerable to bullets
            healthManager.setGodmode(false);

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

            if (stateDurationCounter <= colourResetTime)
                meshRenderer.material.color = new Color(Mathf.Lerp(1.0f, 0.0f, stateDurationCounter / colourResetTime), Mathf.Lerp(0.0f, 1.0f, stateDurationCounter / colourResetTime), 0.0f);
        }

        protected override void onBehaviourSwitch(AbstractEnemy newBehaviour)
        {
            myTime.enabled = true;
        }
    }
}