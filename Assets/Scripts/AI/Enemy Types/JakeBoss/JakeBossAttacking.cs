using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class JakeBossAttacking : JakeBoss
    {
        protected override void Start()
        {
            base.Start();

            mainPivot.startPivot();

            rightPivot.stopPivot(true);
            leftPivot.stopPivot(true);
        }

        protected override System.Type decideState()
        {
            // Target in peripheral but not main vision
            if (!visionCone.hasVisibleTargets() && peripheralVisionCone.hasVisibleTargets())
                return typeof(JakeBossTurning);
            else
                return this.GetType();
        }

        protected override void stateAction()
        {
            shoot();
        }
    }
}