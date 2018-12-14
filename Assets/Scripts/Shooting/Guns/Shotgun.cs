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

        public override GameObject shoot(Vector3 spawnPos)
        {
            if (!canFire())
                return null;

            GameObject pelletBurst = new GameObject();

            for (int i = 0; i < pellets; i++)
            {
                Vector3 bulletTarget = transform.forward * 10;

                bulletTarget.x += Random.Range(-spreadRange, spreadRange);
                bulletTarget.y += Random.Range(-spreadRange, spreadRange);

                GameObject bullet = base.spawnBullet(spawnPos, true);
                bullet.transform.LookAt(bulletTarget);
                bullet.transform.parent = pelletBurst.transform.parent;
            }

            return pelletBurst;
        }
    }
}