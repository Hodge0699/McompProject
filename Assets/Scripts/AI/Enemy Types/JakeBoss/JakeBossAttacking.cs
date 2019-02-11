using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class JakeBossAttacking : JakeBoss
    {
        private float basicAttackDuration = 10.0f;
        private float basicAttackTimer;

        protected override void Start()
        {
            base.Start();

            mainPivot.startPivot();

            rightPivot.stopPivot(true);
            leftPivot.stopPivot(true);

            basicAttackTimer = basicAttackDuration;
        }

        protected override System.Type decideState()
        {
            // Target in peripheral but not main vision
            if (!visionCone.hasVisibleTargets() && peripheralVisionCone.hasVisibleTargets())
                return typeof(JakeBossTurning);


            if (rightPivot.isAtCentre())
            { 
                basicAttackTimer -= Time.deltaTime;

                if (basicAttackTimer <= 0.0f)
                    return typeof(JakeBossTrapping);
            }

            return this.GetType();
        }

        protected override void stateAction()
        {
            if (rightPivot.getRotationPercentage() < 0.0f)
                return;

            shoot();
        }

        protected override void onStateSwitch(JakeBoss newState)
        {
            mainPivot.stopPivot(true);

            base.onStateSwitch(newState);
        }
    }
}