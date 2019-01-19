using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;

using sceneTransitions;

namespace EnemyType
{
    public class BossEnemy : AbstractEnemy
    {
        public GunController gunController;
        private GameObject SceneManager;

        protected override void Awake()
        {
            gunController = GetComponentInChildren<GunController>();
            SceneManager = GameObject.Find("SceneManager");
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
            gunController.shoot();
        }

        /// <summary>
        /// Load next scene on death
        /// </summary>
        protected override void onDeath()
        {
            SceneManager.GetComponent<SceneTransitions>().LoadNextScene();
        }
    }
}
