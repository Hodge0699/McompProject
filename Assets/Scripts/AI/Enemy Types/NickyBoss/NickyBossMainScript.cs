using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;
using sceneTransitions;
using System;
using Player;

namespace EnemyType.Bosses
{
    public class NickyBossMainScript : AbstractEnemy
    {
        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;
        [SerializeField]
        private GameObject SceneManager;

        [Header("Use Predictive Aiming")]
        [SerializeField]
        private bool predictiveAiming = false;


        // === variables you need ===
        //how fast our shots move
        static float shotSpeed = 10.0f;
        //objects
        GameObject shooter;
        static float speed;

        // === derived variables ===
        //positions
        static Vector3 targetPosition;
        //velocities
        static Vector3 targetVelocity;
        private Vector3 predictionPos;

        protected override void Awake()
        {
            SceneManager = GameObject.Find("SceneManager");
            speed = base.movementSpeed;
            //target = base.target;
            base.Awake();
        }

        private void Update()
        {
            if (target != null)
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
                    if (predictiveAiming)
                    {
                        if (anim != null)
                            setAnimTrigger("Shooting");

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
        /// <summary>
        /// shoots at player
        /// </summary>
        private void shoot()
        {
            transform.LookAt(PredictiveShooting(shotSpeed));
            gunController.shoot();

        }

        /// <summary>
        /// Load next scene on death
        /// </summary>
        public override void onDeath()
        {
            SceneManager.GetComponent<SceneTransitions>().LoadNextScene();
        }


        /// <summary>
        /// gets the gameobject myRoom from abstract enemy 
        /// </summary>
        /// <returns></returns>
        public Room getMyRoom()
        {
            return this.myRoom;
        }


        /// <summary>
        /// calculates the relative velocity of the object to adjust for predictive shooting
        /// </summary>
        /// <param name="shotSpeed"></param>
        /// <returns></returns>
        private Vector3 PredictiveShooting(float shotSpeed)
        {
            targetPosition = target.transform.position;
            float travelTime = ((targetPosition - transform.position).sqrMagnitude) / shotSpeed;
            Vector3 targetDirection = target.GetComponent<PlayerInputManager>().getDirectionVector();
            return predictionPos = targetPosition + targetDirection * travelTime;
        }

        /// <summary>
        /// Accessor for BossGunSwitchM
        /// </summary>
        public GunController getGunController()
        {
            return gunController;
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