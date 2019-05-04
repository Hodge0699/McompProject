using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossWaiting : FinalBoss
    {
        float stateDuration = 4.0f;
        float stateDurationCounter = 0.0f;

        protected override Type decideState()
        {
            if (stateDurationCounter >= stateDuration) 
                return typeof(FinalBossChasing);
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            turnTo(player);

            if (visionCone.hasVisibleTargets())
            {
                stateDurationCounter += myTime.getDelta();

                if (!sawBlade.isAccelerating)
                    sawBlade.isAccelerating = true;
            }
        }
    }
}