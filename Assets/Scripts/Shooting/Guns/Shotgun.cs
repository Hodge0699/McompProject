using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
{
    public class Shotgun : AbstractGun
    {
        public int pellets = 5;
        public float spreadRange = 0.25f;

        public void Awake()
        {
            init(5.0f, 10.0f, 3.0f);
        }

        //public override void shoot(Vector3 spawnPos, Vector3 target)
        //{
        //    if (!canFire())
        //        return;

        //    for (int i = 0; i < pellets; i++)
        //    {
        //        Vector3 bulletTarget = target;
        //        bulletTarget.x += Random.Range(-spreadRange, spreadRange);
        //        bulletTarget.y += Random.Range(-spreadRange, spreadRange);
        //        GameObject bullet = base.spawnBullet(spawnPos, bulletTarget, true);
        //    }
        //}


        public override void shoot(Vector3 spawnPos, Transform target)
        {
            if (!canFire())
                return;

            for (int i = 0; i < pellets; i++)
            {
                Transform bulletTarget = target;
                bulletTarget.position.Set((Random.Range(-spreadRange, spreadRange)), (Random.Range(-spreadRange, spreadRange)), bulletTarget.position.z);
                GameObject bullet = base.spawnBullet(spawnPos, bulletTarget, true);
            }
        }


    }
}