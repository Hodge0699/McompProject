using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType
{
    public class JakeBoss : AbstractEnemy
    {
        private Transform mainPivot;
        private Transform rightPivot;
        private Transform leftPivot;

        private GunController gunRight;
        private GunController gunLeft;

        // Use this for initialization
        void Start()
        {
            mainPivot = transform.Find("GunPivot");
            rightPivot = mainPivot.Find("GunRightPivot");
            leftPivot = mainPivot.Find("GunLeftPivot");

            gunRight = rightPivot.Find("GunRight").GetComponent<GunController>();
            gunLeft = leftPivot.Find("GunLeft").GetComponent<GunController>();

            gunRight.setGun(typeof(Weapon.Gun.MachineGun), true);
            gunLeft.setGun(typeof(Weapon.Gun.MachineGun), true);
        }

        // Update is called once per frame
        void Update()
        {
            shoot();
        }

        protected void shoot()
        {
            gunRight.shoot();
            gunLeft.shoot();
        }
    }
}