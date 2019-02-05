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

        private bool usePredictiveAiming = false; // Barebones lightweight predictive shooting (not ready)
                                                  //
                                                  // To do:
                                                  // - Fix tracking issue when player sharp turns at close range (enemy predicts movement 
                                                  //   correctly but looking at it puts player out of FOV).
                                                  // - Fix issue where bullets fall behind player when moving in straight line far away.
                                                  // - Artificial stupidity.
                                                  // - Smooth aiming when player is using KB+M (player's direction vector changing from 
                                                  //   (1,0,0) to (-1,0,0) produces big jump in aiming, potentially cap turn speed?).

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
            else if (target != null)
            {
                if (getDistanceToTarget() >= 5.0f)
                {
                    if (usePredictiveAiming)
                        chaseP();
                    else
                        chase();
                }
                else
                {
                    if (usePredictiveAiming)
                        predictiveAim();
                    else
                        transform.LookAt(target.transform);
                }
            }
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
                if (usePredictiveAiming)
                    predictiveAim();
                else
                    transform.LookAt(target.transform);
                directionVector = (pickupLocation - transform.position).normalized;
            }
            else
            {
                transform.LookAt(pickupLocation);
                directionVector = transform.forward;
            }
        }

        /// <summary>
        /// Aims at the player taking into consideration distance, 
        /// player velocity and bot velocity.
        /// </summary>
        private void predictiveAim()
        {
            if (target == null)
                return;

            float bulletSpeed = 10.0f;
            float secondsToImpact = ((target.transform.position - transform.position).magnitude) / bulletSpeed;

            Vector3 targetPos = target.transform.position;
            Vector3 targetDir = target.GetComponent<Player.PlayerInputManager>().getDirectionVector();
            float targetSpeed = target.GetComponent<Player.PlayerController>().moveSpeed;
            targetPos += targetDir * targetSpeed * secondsToImpact;

            transform.LookAt(targetPos);

            bool debugging = false;
            if (debugging)
            {
                Debug.Log(targetPos - target.transform.position);
                Debug.DrawLine(transform.position, targetPos);
                Debug.DrawLine(target.transform.position, targetPos);
            }
        }

        /// <summary>
        /// Chases the target if there is one
        /// 
        /// TEMPORARY (ugly)
        /// </summary>
        protected void chaseP()
        {
            if (target == null)
                return;

            if (Vector3.Distance(target.transform.position, this.transform.position) > maxDistance)
            {
                predictiveAim();
                directionVector = transform.forward;
            }
        }
    }
}