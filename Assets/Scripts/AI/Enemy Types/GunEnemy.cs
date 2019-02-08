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

        private bool usePredictiveAiming = true; // Lightweight predictive shooting
                                                  //
                                                  // To do:
                                                  // - Fix issue where bullets fall behind player when moving in straight line far away.
                                                  // - Artificial stupidity.

        protected override void Awake()
        {
            gunController = GetComponentInChildren<GunController>();
            pickUpVisionCone = GetComponents<VisionCone>()[1];

            base.Awake();
        }

        private void Update()
        {
            gunController.switchToBest();

            if (pickUpVisionCone.hasVisibleTargets())
                moveToPickup();
            else if (target != null)
            {
                if (getDistanceToTarget() >= 5.0f)
                    chase();
                else
                {
                    if (usePredictiveAiming)
                        predictiveAim();
                    else
                        turnTo(target);
                }
            }
            else
                wander();

            if (target != null && canShoot)
                shoot();
        }

        protected void shoot()
        {
            gunController.shoot();
        }

        /// <summary>
        /// Moves the agent to a pickup if there is one visible.
        /// </summary>
        protected void moveToPickup()
        {
            Vector3 pickupLocation;

            if (pickUpVisionCone.hasVisibleTargets())
                pickupLocation = pickUpVisionCone.getClosestVisibleTarget().transform.position;
            else
                return;

            if (target != null)
            {
                if (usePredictiveAiming)
                    predictiveAim();
                else
                    turnTo(target);
                directionVector = (pickupLocation - transform.position).normalized;
            }
            else
            {
                turnTo(pickupLocation);
                directionVector = transform.forward;
            }
        }

        /// <summary>
        /// Chases the target with predictive aiming
        /// </summary>
        protected override void chase()
        {
            if (target == null)
                return;

            if (Vector3.Distance(target.transform.position, this.transform.position) > maxDistance)
            {
                if (usePredictiveAiming)
                    predictiveAim();
                else
                    turnTo(target);

                directionVector = transform.forward;
            }
        }

        /// <summary>
        /// Aims at the player taking into consideration distance, 
        /// player velocity and bot velocity.
        /// </summary>
        protected void predictiveAim()
        {
            if (target == null)
                return;

            float bulletSpeed = 10.0f;
            float secondsToImpact = ((target.transform.position - transform.position).magnitude) / bulletSpeed;

            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = target.GetComponent<Player.PlayerInputManager>().getDirectionVector();
            float targetSpeed = target.GetComponent<Player.PlayerController>().moveSpeed;
            targetPos += targetDir * targetSpeed * secondsToImpact;

            turnTo(targetPos);
        }
    }
}