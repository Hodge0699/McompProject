using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;
using sceneTransitions;
using System;

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
        static Vector3 shooterPosition;
        static Vector3 targetPosition;
        //velocities
        static Vector3 shooterVelocity;
        static Vector3 targetVelocity;

        protected override void Awake()
        {
            SceneManager = GameObject.Find("SceneManager");
            shooterPosition = transform.position;
            shooterVelocity = this.GetComponent<Rigidbody>().velocity;
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
            targetPosition = target.transform.position;
            targetVelocity = target.GetComponent<Rigidbody>().velocity;
            transform.LookAt(FirstOrderIntercept(shooterPosition,
                shooterVelocity,
                shotSpeed,
                targetPosition,
                targetVelocity));
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
        /// <param name="shooterPosition"></param>
        /// <param name="shooterVelocity"></param>
        /// <param name="shotSpeed"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        /// <returns></returns>
        private static Vector3 FirstOrderIntercept
        (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
        )
        {
            shooterPosition = shooterPosition * speed * Time.deltaTime;
            Vector3 targetRelativePosition = targetPosition - shooterPosition;
            Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
            const float predictionTime = 20; // prediction time, can be experimented with for artificial stupidity 

            return targetPosition + predictionTime * (targetRelativeVelocity);
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