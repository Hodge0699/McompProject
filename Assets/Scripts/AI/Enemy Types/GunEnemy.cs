using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class GunEnemy : AbstractEnemy
    {
        private GunController gunController;
        private VisionCone pickUpVisionCone;

        protected override void Awake()
        {
            gunController = GetComponentInChildren<GunController>();
            pickUpVisionCone = GetComponents<VisionCone>()[1];

            base.Awake();
        }

        private void Update()
        {
            if (pickUpVisionCone.hasVisibleTargets())
            {
                if (visionCone.hasVisibleTargets())
                {
                    transform.LookAt(visionCone.getClosestVisibleTarget().transform);
                    moveToPickup(true);
                    shoot();
                }
                else
                    moveToPickup();
            }
            else if (target == null)
                wander();
            else
            {
                chase();
                shoot();
            }
        }

        private void shoot()
        {
            gunController.shoot();
        }

        /// <summary>
        /// Moves the agent to a pickup if there is one visible.
        /// </summary>
        /// <param name="strafe">Set as true if rotation is handled elsewhere.</param>
        private void moveToPickup(bool strafe = false)
        {
            GameObject target = pickUpVisionCone.getClosestVisibleTarget();

            if (target == null)
                return;

            if (strafe)
                directionVector = (target.transform.position - transform.position).normalized;
            else
            {
                transform.LookAt(target.transform);
                directionVector = transform.forward;
            }
        }
    }
}