using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon.Gun
{
    public class Shotgun : AbstractGun
    {
        public int pellets = 5;
        public float spreadRange = 5.0f;

        public void Awake()
        {
            init(5.0f, 10.0f, 1.5f, pellets * 15);
        }

        public override GameObject shoot(Vector3 spawnPos)
        {
            if (!canFire())
                return null;

            GameObject pelletBurst = new GameObject();
            pelletBurst.name = "Pellet Burst";

            for (int i = 0; i < pellets; i++)
            {
                GameObject bullet = base.spawnBullet(spawnPos, true);
                bullet.transform.parent = pelletBurst.transform;

                bullet.transform.Rotate(Vector3.up, Random.Range(-spreadRange, spreadRange)); // spreads horizontally only 
            }

            return pelletBurst;
        }
    }
}