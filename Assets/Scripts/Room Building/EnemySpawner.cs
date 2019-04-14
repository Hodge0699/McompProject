using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script needs to be connect to an empty object
//Make sure gizmos are visible!

namespace RoomBuilding
{
	public class EnemySpawner : MonoBehaviour
	{
	    public Vector3 size;

        [Header("Debugging")]
        [SerializeField]
        private bool OGEnemies = false; // bad jake, no globaling variables.

        /// <summary>
        /// Spawns an enemy of a specific type
        /// </summary>
        /// <param name="type">EnemyType class.</param>
        /// <returns>New enemy</returns>
        public GameObject spawn(System.Type type = null)
        {
            bool useHybridMelee = true;

            GameObject enemy;

            if (type == null)
                type = randomEnemyType();


            if (OGEnemies) // Original capsule enemies
                enemy = Instantiate(Resources.Load("Enemy")) as GameObject;
            else // New animated enemies
            {
                if (type == typeof(EnemyType.MeleeEnemy)) // Melee enemy
                    enemy = Instantiate(Resources.Load("HybridEnemy")) as GameObject;
                else // Rifle enemy
                    enemy = Instantiate(Resources.Load("RifleEnemy")) as GameObject;
            }

            enemy.AddComponent(type);

            enemy.transform.position = generateNewPosition();
            enemy.transform.Rotate(Vector3.up, Random.Range(0.0f, 359.0f));

            EnemyType.AbstractEnemy enemyScr = enemy.GetComponent<EnemyType.AbstractEnemy>();
            enemyScr.enabled = false;

            if (useHybridMelee && type == typeof(EnemyType.MeleeEnemy))
                enemy.GetComponent<EnemyType.MeleeEnemy>().usePickups = true;

            return enemy;
        }

        /// <summary>
        /// Gets a random enemy type
        /// </summary>
        private System.Type randomEnemyType()
        {
            const int NUM_ENEMY_TYPES = 2;

            int rand = Random.Range(0, NUM_ENEMY_TYPES);

            switch (rand)
            {
                case (0):
                    return typeof(EnemyType.MeleeEnemy);
                case (1):
                    return typeof(EnemyType.GunEnemy);
                default:
                    return typeof(EnemyType.AbstractEnemy);
            }
        }

        /// <summary>
        /// Generates a new position within bounds
        /// </summary>
        /// <returns>Vector3 world position.</returns>
        public Vector3 generateNewPosition()
        {
            return transform.position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0.00f, Random.Range(-size.z / 2, size.z / 2));
        }

        void OnDrawGizmosSelected()
	    {
	        Gizmos.color = new Color(1, 0, 0, 0.5f);
	        Gizmos.DrawCube(transform.position, size);
	    }
	}
}
