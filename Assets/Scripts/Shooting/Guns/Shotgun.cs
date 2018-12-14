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

        public override GameObject shoot(Vector3 spawnPos, Vector3 ? target)
        {
            if (!canFire())
                return null;

            GameObject pelletBurst = new GameObject();

            for (int i = 0; i < pellets; i++)
            {
                Vector3 bulletTarget;

                if (target.HasValue)
                    bulletTarget = target.Value;
                else
                    bulletTarget = getForwardTarget();

                bulletTarget.x += Random.Range(-spreadRange, spreadRange);
                bulletTarget.y += Random.Range(-spreadRange, spreadRange);
                GameObject bullet = base.spawnBullet(spawnPos, bulletTarget, true);
                bullet.transform.parent = pelletBurst.transform.parent;
            }

            return pelletBurst;
        }
    }
}