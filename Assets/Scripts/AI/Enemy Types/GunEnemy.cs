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
            

            if (pickUpVisionCone.hasVisibleTargets())
                moveToPickup();
            else if (target != null && getDistanceToTarget() >= 5.0f)
                chase();
            else
                wander();

            if (target != null && canShoot)
                shoot();
        }

        private void shoot()
        {
            gunController.shoot();
        }

        /// <summary>
        /// Moves the agent to a pickup if there is one visible.
        /// </summary>
        private void moveToPickup()
        {
            Vector3 pickupLocation;

            if (pickUpVisionCone.hasVisibleTargets())
                pickupLocation = pickUpVisionCone.getClosestVisibleTarget().transform.position;
            else
                return;

            if (target != null)
            {
                transform.LookAt(target.transform);
                directionVector = (pickupLocation - transform.position).normalized;
            }
            else
            {
                transform.LookAt(pickupLocation);
                directionVector = transform.forward;
            }
        }
    }
}