using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;

using sceneTransitions;
using System;

namespace EnemyType.Bosses
{
    public class BossEnemy : AbstractEnemy
    {
        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;

        private GameObject SceneManager;



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
            if (target != null) // Can see player
            {
                if (getDistanceToTarget() > 5.0f)
                    chase();
                else
                    transform.LookAt(target.transform);

                if (canShoot) // Used for rewind system
                    shoot();
            }
            else
                wander();
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
            //gunController.shoot();
            
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
        /// gets the sqrMagnitude of an object
        /// </summary>
        /// <param name="v3"></param>
        /// <returns></returns>
        static float sqrMagnitude(Vector3 v3)
        {
            float temp = (float)(Math.Pow(v3.x, 2) + Math.Pow(v3.y,2) + Math.Pow(v3.z,2));
            return temp;
        }
        /// <summary>
        /// gets the Magnitude of an object
        /// </summary>
        /// <param name="v3"></param>
        /// <returns></returns>
        static float magnitude(Vector3 v3)
        {
            float temp = (float)Math.Sqrt(Math.Pow(v3.x, 2) + Math.Pow(v3.y, 2) + Math.Pow(v3.z, 2));
            return temp;
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
        public static Vector3 FirstOrderIntercept
        (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
        ){
            shooterPosition = shooterPosition * speed * Time.deltaTime;
            Vector3 targetRelativePosition = targetPosition - shooterPosition;
            Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
            const float predictionTime = 10; // One second prediction, you need to experiment.

            return targetPosition + predictionTime * (targetRelativeVelocity);
            //return turnToTarget + t * (targetRelativeVelocity); // applying bullet drop to the calculations
        }

        /// <summary>
        /// Accessor for BossGunSwitchM
        /// </summary>
        public GunController getGunController()
        {
            return gunController;
        }

        
    }
}
