using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class FinalBossWaiting : FinalBoss
    {
        protected override Type decideState()
        {
            if (visionCone.hasVisibleTargets()) // Can see player
                return typeof(FinalBossChasing);
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            // Do nothing
        }
    }
}