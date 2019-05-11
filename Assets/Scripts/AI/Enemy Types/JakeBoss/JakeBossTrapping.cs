using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class JakeBossTrapping : JakeBoss
    {
        protected override void Start()
        {
            base.Start();

            mainPivot.stopPivot(true);

            gunRight.setGun(typeof(Weapon.Gun.EXDHandgun), true);
            gunLeft.setGun(typeof(Weapon.Gun.EXDHandgun), true);
        }

        protected override System.Type decideState()
        {
            // Target in peripheral but not main vision, start turning
            if (!visionCone.hasVisibleTargets() && peripheralVisionCone.hasVisibleTargets())
                return typeof(JakeBossTurning);

            // Pivot has completed 90% of it's movement, switch back to regular attack
            if (rightPivot.getRotationPercentage() <= -0.6f)
                return typeof(JakeBossAttacking);

            return this.GetType();
        }

        protected override void stateAction()
        {
            if (mainPivot.isAtCentre())
            {
                rightPivot.goToBound(false);
                leftPivot.goToBound(true);
            }

            shoot();
        }

        protected override void onBehaviourSwitch(AbstractEnemy newBehaviour)
        {
            rightPivot.stopPivot(true);
            leftPivot.stopPivot(true);
        }
    }
}