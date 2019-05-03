using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace TimeMechanic
{
    public class TimeStop : TimeMechanic
    {
        public bool isStopped { get; private set; }

        override protected void Start()
        {
            base.Start();

            isStopped = false;
        }

        public override void trigger()
        {
            if (!isStopped)
            {
                setTimeDilation(0.0f);
                isStopped = true;
            }
            else
            {
                setTimeDilation(1.0f);
                isStopped = false;
            }
        }

        /// <summary>
        /// Sets the time dilation for all enemies and bullets in scene
        /// </summary>
        /// <param name="timeDilation">0.0f is frozen, 1.0f is realtime</param>
        private void setTimeDilation(float timeDilation)
        {
            // Enemies
            Transform enemyContainer = GameObject.FindObjectOfType<Room>().transform.Find("Enemies").transform;

            for (int i = 0; i < enemyContainer.childCount; i++)
            {
                LocalTimeDilation enemyTime = enemyContainer.GetChild(i).GetComponent<LocalTimeDilation>();

                if (enemyTime != null)
                    enemyTime.setDilation(timeDilation);
            }


            // Bullets
            Transform bulletContainer = GameObject.Find("Active Bullets").transform;

            if (bulletContainer != null)
            {
                for (int i = 0; i < bulletContainer.childCount; i++)
                {
                    Transform bullet = bulletContainer.GetChild(i);

                    if (bullet.name == "Pellet Burst") // Shotgun pellet
                    {
                        for (int j = 0; j < bullet.childCount; j++)
                        {
                            LocalTimeDilation bulletTime = bullet.GetChild(j).GetComponent<LocalTimeDilation>();

                            if (bulletTime != null) // Some bullets aren't affected by time
                                bulletTime.setDilation(timeDilation);
                        }
                    }
                    else // Single bullet
                    {
                        LocalTimeDilation bulletTime = bullet.GetComponent<LocalTimeDilation>();

                        if (bulletTime != null) // Some bullets aren't affected by time
                            bulletTime.setDilation(timeDilation);
                    }
                }
            }

        }
    }
}
