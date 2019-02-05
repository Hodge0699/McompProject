using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;

using sceneTransitions;
using System;

namespace EnemyType
{
    public class BossEnemy : AbstractEnemy
    {
        // Used for rewind system
        [System.NonSerialized]
        public bool canShoot = true;

        public GunController gunController;
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
            gunController = GetComponentInChildren<GunController>();
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
            //float t = FirstOrderInterceptTime
            //(
            //    shotSpeed,
            //    targetRelativePosition,           // not needed as there is no bullet drop in the game
            //    targetRelativeVelocity
            //);
            const float predictionTime = 10; // One second prediction, you need to experiment.

            return targetPosition + predictionTime * (targetRelativeVelocity);
            //return turnToTarget + t * (targetRelativeVelocity); // applying bullet drop to the calculations
        }


        /// <summary>
        /// Calculates the trajectory to the target if the bullet had a drop applied. 
        /// </summary>
        /// <param name="shotSpeed"></param>
        /// <param name="targetRelativePosition"></param>
        /// <param name="targetRelativeVelocity"></param>
        /// <returns></returns>
        public static float FirstOrderInterceptTime
        (
            float shotSpeed,
            Vector3 targetRelativePosition,
            Vector3 targetRelativeVelocity
        )
        {
            float velocitySquared = targetRelativeVelocity.sqrMagnitude;
            if (velocitySquared < 0.001f)
                return 0f;

            float a = velocitySquared - shotSpeed * shotSpeed;

            //handle similar velocities
            if (Mathf.Abs(a) < 0.001f)
            {
                float t = -targetRelativePosition.sqrMagnitude /
                (
                    2f * Vector3.Dot
                    (
                        targetRelativeVelocity,
                        targetRelativePosition
                    )
                );
                return Mathf.Max(t, 0f); //don't shoot back in time
            }

            float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
            float c = targetRelativePosition.sqrMagnitude;
            float determinant = b * b - 4f * a * c;

            if (determinant > 0f)
            { //determinant > 0; two intercept paths (most common)
                float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                        t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
                if (t1 > 0f)
                {
                    if (t2 > 0f)
                        return Mathf.Min(t1, t2); //both are positive
                    else
                        return t1; //only t1 is positive
                }
                else
                    return Mathf.Max(t2, 0f); //don't shoot back in time
            }
            else if (determinant < 0f) //determinant < 0; no intercept path
                return 0f;
            else //determinant = 0; one intercept path, pretty much never happens
                return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
        }
    }
}
