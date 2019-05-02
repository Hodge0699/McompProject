using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using EnemyType;
using EnemyType.Turrets;

namespace TimeMechanic
{
    public class RewindTimeManager : TimeMechanic
    {
        bool isRewinding = false;

        private float recordTime = 3f;

        List<PointInTime> pointsInTime;

        Rigidbody rb;

        HealthManager.HealthManager health;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            pointsInTime = new List<PointInTime>();

            health = gameObject.GetComponent<HealthManager.HealthManager>();

            if (gameObject.GetComponent<Rigidbody>() != null)
                rb = GetComponent<Rigidbody>();
        }

        public override void trigger()
        {
            StartRewind();
        }

        /// <summary>
        /// Uses FixedUpdate instead of update to keep physics consistant
        /// </summary>
        void FixedUpdate()
        {
            if (isRewinding)
                Rewind();
            else
                Record();
        }

        /// <summary>
        /// Rewinds untill end of list
        /// Destroys Bullets when they rewind past their time of creation
        /// </summary>
        void Rewind()
        {
            if (pointsInTime.Count > 0)
            {
                PointInTime pointInTime = pointsInTime[0];
                transform.position = pointInTime.position;
                transform.rotation = pointInTime.rotation;

                if (health != null)
                    health.setHealth(pointInTime.health);

                pointsInTime.RemoveAt(0);
            }
            else
            {
                if (gameObject.tag == "Bullet")
                    Destroy(gameObject);

                StopRewind();
            }
        }

        /// <summary>
        /// Record position and rotation data into list for past "recordTime" secconds replacing end with new
        /// </summary>
        void Record()
        {
            if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
                pointsInTime.RemoveAt(pointsInTime.Count - 1);

            if (health != null)
                pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, health.getHealth()));
            else
                pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
        }

        /// <summary>
        /// Starts and Stops rewinding, makes rigid bodies kinematic while rewinding
        /// Prevents Player and Enemies from shooting while rewinding
        /// </summary>
        public void StartRewind()
        {
            isRewinding = true;
            if (rb != null)
                rb.isKinematic = true;

            if (gameObject.tag == "Enemy")
                if (gameObject.GetComponent<GunEnemy>() != null)
                    gameObject.GetComponent<GunEnemy>().canShoot = false;

            if (gameObject.tag == "Boss")
                if (gameObject.GetComponent<EnemyType.Bosses.BossEnemy>() != null)
                    gameObject.GetComponent<EnemyType.Bosses.BossEnemy>().canShoot = false;

            if (gameObject.tag == "Turret")
                if (gameObject.GetComponent<bulletPillar>() != null)
                    gameObject.GetComponent<bulletPillar>().canShoot = false;

            if (gameObject.tag == "Player")
                gameObject.GetComponent<Player.PlayerInputManager>().canShoot = false;

        }

        public void StopRewind()
        {
            isRewinding = false;
            if (rb != null)
                rb.isKinematic = false;

            if (gameObject.tag == "Enemy")
                if (gameObject.GetComponent<GunEnemy>() != null)
                    gameObject.GetComponent<GunEnemy>().canShoot = true;

            if (gameObject.tag == "Boss")
                if (gameObject.GetComponent<EnemyType.Bosses.BossEnemy>() != null)
                    gameObject.GetComponent<EnemyType.Bosses.BossEnemy>().canShoot = true;

            if (gameObject.tag == "Turret")
                if (gameObject.GetComponent<bulletPillar>() != null)
                    gameObject.GetComponent<bulletPillar>().canShoot = true;

            if (gameObject.tag == "Player")
                gameObject.GetComponent<Player.PlayerInputManager>().canShoot = true;

        }
    }
}