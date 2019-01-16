using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Gun;

using sceneTransitions;

namespace EnemyType
{
    public class BossEnemy : AbstractEnemy
    {
        protected System.Type gun;
        private GunController gunController;
        private int gunSwitchChanger = 0;
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

            if(base.currentHealth <= base.health / 2 && base.currentHealth >= base.health /4 && gunSwitchChanger == 0)
            {
                gun = typeof(Shotgun);
                gunController.setGun(gun, 300);
                gunSwitchChanger = 1;
            }
            else if (base.currentHealth <= base.health / 4 && base.currentHealth > 0 && gunSwitchChanger == 1)
            {
                gun = typeof(EXDHandgun);
                gunController.setGun(gun, 300);
                gunSwitchChanger = 2;
            }
        }

        private void shoot()
        {
            gunController.shoot();
        }
        private void handGunShooting()
        {
            gunController.shoot();
        }
    }
}
