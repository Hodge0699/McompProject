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
            init(5.0f, 10.0f, 1.0f);
        }

        public override void shoot(Vector3 spawnPos, Vector3 target)
        {
            if (!canFire())
                return;

            for (int i = 0; i < pellets; i++)
            {
                Vector3 bulletTarget = target;
                bulletTarget.x += Random.Range(-spreadRange, spreadRange);
                bulletTarget.y += Random.Range(-spreadRange, spreadRange);
                GameObject bullet = base.spawnBullet(spawnPos, bulletTarget, true);
            }
        }
    }
}