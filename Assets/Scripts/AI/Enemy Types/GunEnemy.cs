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

        private bool usePredictiveAiming = true; // Lightweight predictive shooting
                                                  //
                                                  // To do:
                                                  // - Fix issue where bullets fall behind player when moving in straight line far away.
                                                  // - Artificial stupidity.

        private void Update()
        {
            if (anim != null)
                anim.speed = myTime.getDilation();

            // Switch back to MeleeEnemy if out of ammo
            if (!gunController.hasAmmo())
            {
                switchToBehaviour(typeof(MeleeEnemy), false, false);
                GetComponent<MeleeEnemy>().usePickups = true;
                Destroy(this);
            }
            
            gunController.switchToBest();

            if (pickUpVisionCone.hasVisibleTargets())
            {
                setAnimTrigger("Chasing");
                moveToPickup();
            }
            else if (target != null)
            {
                // checks to see if distance is greater than a specified number
                if (getDistanceToTarget() >= 10.0f)
                {
                    if (anim != null)
                        setAnimTrigger("Chasing");
                    chase();
                }
                else
                {
                    // if enemy is close enough start using predictive aiming to fire at enemy
                    if (usePredictiveAiming)
                    {
                        if (anim != null)
                            setAnimTrigger("Shooting");

                        predictiveAim();
                        if (target != null && canShoot)
                            shoot();
                    }
                    else
                    {
                        turnTo(target);
                    }
                }
            }
            // if unable to find an enemy keep to default wandering state
            else
            {
                if (anim != null)
                    setAnimTrigger("PlayerDead");
                wander();
            }
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

            /* New enemies don't have strafe animation :( 
            if (target != null)
            {
                if (usePredictiveAiming)
                    predictiveAim();
                else
                    turnTo(target);
                directionVector = (pickupLocation - transform.position).normalized;
            }
            else */
                goToPosition(pickupLocation);
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

        /// <summary>
        /// Resets other triggers and sets new trigger
        /// </summary>
        /// <param name="trigger">New trigger to set </param>
        override protected void setAnimTrigger(string trigger)
        {
            anim.ResetTrigger("Chasing");
            anim.ResetTrigger("PlayerDead");
            anim.ResetTrigger("Shooting");

            anim.SetTrigger(trigger);
        }
    }
}