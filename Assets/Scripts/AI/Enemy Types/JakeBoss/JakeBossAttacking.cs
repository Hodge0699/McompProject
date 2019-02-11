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
            // Target in peripheral vision
            if (peripheralVisionCone.hasVisibleTargets())
            {
                // Directly ahead
                if (visionCone.hasVisibleTargets())
                {
                    // Ready to start firing
                    if (rightPivot.isAtCentre())
                        basicAttackTimer -= Time.deltaTime;

                    // Basic attack finished
                    if (basicAttackTimer <= 0.0f)
                        return typeof(JakeBossTrapping);
                    else
                        return this.GetType();
                }
                else // Not directly ahead
                    return typeof(JakeBossTurning);
            }
            else // Not in peripheral vision
                return typeof(JakeBossWaiting);
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