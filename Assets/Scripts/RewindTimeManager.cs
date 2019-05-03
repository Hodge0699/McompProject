using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using EnemyType;
using EnemyType.Turrets;

namespace TimeMechanic
{
    public class RewindTimeManager : TimeMechanic
    {
        public override void trigger()
        {
            StartRewind();
        }

        /// <summary>
        /// Starts and Stops rewinding, makes rigid bodies kinematic while rewinding
        /// Prevents Player and Enemies from shooting while rewinding
        /// </summary>
        public void StartRewind()
        {
            GetComponent<RewindTimeSlave>().StartRewind();

            // Enemies
            Transform enemyContainer = GameObject.Find("Room").transform.Find("Enemies").transform;

            for (int i = 0; i < enemyContainer.childCount; i++)
            {
                RewindTimeSlave enemyTime = enemyContainer.GetChild(i).GetComponent<RewindTimeSlave>();

                if (enemyTime != null)
                    enemyTime.StartRewind();
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
                            RewindTimeSlave bulletTime = bullet.GetChild(j).GetComponent<RewindTimeSlave>();

                            if (bulletTime != null) // Some bullets aren't affected by time
                                bulletTime.StartRewind();
                        }
                    }
                    else // Single bullet
                    {
                        RewindTimeSlave bulletTime = bullet.GetComponent<RewindTimeSlave>();

                        if (bulletTime != null) // Some bullets aren't affected by time
                            bulletTime.StartRewind();
                    }
                }
            }
        }

        public void StopRewind()
        {
            GetComponent<RewindTimeSlave>().StopRewind();

            // Enemies
            Transform enemyContainer = GameObject.Find("Room").transform.Find("Enemies").transform;

            for (int i = 0; i < enemyContainer.childCount; i++)
            {
                RewindTimeSlave enemyTime = enemyContainer.GetChild(i).GetComponent<RewindTimeSlave>();

                if (enemyTime != null)
                    enemyTime.StopRewind();
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
                            RewindTimeSlave bulletTime = bullet.GetChild(j).GetComponent<RewindTimeSlave>();

                            if (bulletTime != null) // Some bullets aren't affected by time
                                bulletTime.StopRewind();
                        }
                    }
                    else // Single bullet
                    {
                        RewindTimeSlave bulletTime = bullet.GetComponent<RewindTimeSlave>();

                        if (bulletTime != null) // Some bullets aren't affected by time
                            bulletTime.StopRewind();
                    }
                }
            }

        }
    }
}