using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class GunEnemy : AbstractEnemy
    {
        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;

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
            if (pickUpVisionCone.hasVisibleTargets()) // Can see pickup
            {
                if (visionCone.hasVisibleTargets()) // Can also see player
                {
                    transform.LookAt(visionCone.getClosestVisibleTarget().transform);
                    moveToPickup(true);
                    if (canShoot) // Used for rewind system
                        shoot();
                }
                else
                    moveToPickup();
            }
            else if (target != null) // Can see player
            {
                if (getDistanceToTarget() >= 5.0f)
                    chase();
                else
                    transform.LookAt(target.transform);

                if (canShoot) // Used for rewind system
                    shoot();
            }
            else 
                wander();
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