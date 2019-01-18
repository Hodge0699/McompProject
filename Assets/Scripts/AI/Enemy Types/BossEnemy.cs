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
            base.sT = SceneManager.GetComponent<SceneTransitions>();
            base.Awake();
        }

        private void Update()
        {
            if (target != null) // Can see player
            {
                if (getDistanceToTarget() >= 5.0f)
                    chase();
                else
                    transform.LookAt(target.transform);
                shoot();
            }
            else
                wander();
        }

        private void shoot()
        {
            gunController.shoot();
        }
    }
}
