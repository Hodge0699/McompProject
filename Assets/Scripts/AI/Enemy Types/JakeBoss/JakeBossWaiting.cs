using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class JakeBossWaiting : JakeBoss
    {
        protected override void Start()
        {
            base.Start();

            leftPivot.rotateCounterClockwise();

            gunRight.setGun(typeof(Weapon.Gun.MachineGun), true);
            gunLeft.setGun(typeof(Weapon.Gun.MachineGun), true);

            GetComponent<EnemyHealthManager>().lookAtPlayerOnHit = false;
        }

        protected override Type decideState()
        {
            // Player ahead
            if (visionCone.hasVisibleTargets())
                return typeof(JakeBossAttacking);

            // Player in peripheral
            if (peripheralVisionCone.hasVisibleTargets())
                return typeof(JakeBossTurning);

            return this.GetType();
        }

        protected override void stateAction()
        {
            // Do nothing
        }

        protected override void onStateSwitch(JakeBoss newState)
        {
            // Do nothing
        }
    }
}