using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Turrets
{
    public class bulletPillar : AbstractTurret
    {
        // Used for rewind system
        //[System.NonSerialized]
        public bool canShoot = true;

        public float serialFireRate = 10.0f;
        private float serialFireTimer;

        public float serialPhaseDuration = 5.0f;
        private float serialPhaseCounter = 0.0f;

        public float parallelFireRate = 1.5f;
        private float parallelFireTimer;

        public float parallelPhaseDuration = 15.0f;
        private float parallelPhaseCounter = 0.0f;

        public enum FireMode { SERIAL, PARALLEL };
        public FireMode currentPhase = FireMode.PARALLEL;

        private int currentGun = 0;

        public new void Start()
        {
            base.Start();
            serialFireTimer = 1 / serialFireRate;
            parallelFireTimer = 1 / parallelFireRate;

            serialPhaseCounter = serialPhaseDuration;
            parallelPhaseCounter = parallelPhaseDuration;
        }

        // Update is called once per frame
        void Update()
        {
            if (fov.hasVisibleTargets())
            {
                if (currentPhase == FireMode.SERIAL && serialFireTimer <= 0.0f)
                {
                    if (canShoot) // Used for rewind system
                        shoot(currentGun);
                    currentGun++;

                    if (currentGun >= gunCount)
                        currentGun = 0;

                    serialFireTimer = 1 / serialFireRate;
                }
                else if (currentPhase == FireMode.PARALLEL && parallelFireTimer <= 0.0f)
                {
                    if (canShoot) // Used for rewind system
                        shoot();
                    parallelFireTimer = 1 / parallelFireRate;
                }
            }

            managePhases();
        }

        void managePhases()
        {
            if (currentPhase == FireMode.SERIAL)
            {
                serialFireTimer -= Time.deltaTime;
                serialPhaseCounter -= Time.deltaTime;

                if (serialPhaseCounter <= 0.0f)
                {
                    currentPhase = FireMode.PARALLEL;
                    parallelPhaseCounter = parallelPhaseDuration;
                    setGuns(typeof(Weapon.Gun.Handgun));
                }

            }
            else if (currentPhase == FireMode.PARALLEL)
            {
                parallelFireTimer -= Time.deltaTime;
                parallelPhaseCounter -= Time.deltaTime;

                if (parallelPhaseCounter <= 0.0f)
                {
                    currentPhase = FireMode.SERIAL;
                    serialPhaseCounter = serialPhaseDuration;
                    setGuns(typeof(Weapon.Gun.Shotgun));
                }
            }
        }
    }
}