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
            base.Start();

            sawBlade.isAccelerating = true;

            healthManager.setGodmode(true);
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

            meshRenderer.material.color = new Color(Mathf.Lerp(0.0f, 1.0f, stateDurationCounter / stateDuration), Mathf.Lerp(1.0f, 0.0f, stateDurationCounter / stateDuration), 0.0f);
        }
    }
}