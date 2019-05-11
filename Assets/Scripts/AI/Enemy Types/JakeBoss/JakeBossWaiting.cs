using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Bosses
{
    public class JakeBossWaiting : JakeBoss
    {
        private float stateDuration = 5.0f;
        private float stateTimer = 0.0f;

        protected override void Start()
        {
            base.Start();

            leftPivot.rotateCounterClockwise();

            gunRight.setGun(typeof(Weapon.Gun.MachineGun), true);
            gunLeft.setGun(typeof(Weapon.Gun.MachineGun), true);
        }

        protected override Type decideState()
        {
            // Player ahead
            if (stateTimer >= stateDuration)
                return typeof(JakeBossAttacking);

            // Player in peripheral
            if (!visionCone.hasVisibleTargets() && peripheralVisionCone.hasVisibleTargets())
                return typeof(JakeBossTurning);

            return this.GetType();
        }

        protected override void stateAction()
        {
            if (visionCone.hasVisibleTargets())
            {
                stateTimer += myTime.getDelta();
                gunLeft.shoot();
                gunRight.shoot();
            }
        }

    }
}