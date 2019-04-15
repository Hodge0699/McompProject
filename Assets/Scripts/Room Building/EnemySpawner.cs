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

        // Debugging tools
        public bool debugging = false;
        public bool OGEnemies = false;
        public enum DebugBehaviour { RANDOM, MELEE, GUN, HYBRID };
        public DebugBehaviour debugBehaviour;

        /// <summary>
        /// Spawns an enemy of a specific type
        /// </summary>
        /// <param name="type">EnemyType class.</param>
        /// <returns>New enemy</returns>
        public GameObject spawn(System.Type type = null)
        {
            GameObject enemy;

            // Should melee enemies be able to pick up and use guns?
            bool useHybridMelee = true;

            if (debugging)
            {
                // Find type
                switch (debugBehaviour)
                {
                    case (DebugBehaviour.RANDOM):
                        type = randomEnemyType();
                        break;
                    case (DebugBehaviour.MELEE):
                        type = typeof(EnemyType.MeleeEnemy);
                        useHybridMelee = false;
                        break;
                    case (DebugBehaviour.GUN):
                        type = typeof(EnemyType.GunEnemy);
                        break;
                    case (DebugBehaviour.HYBRID):
                        type = typeof(EnemyType.MeleeEnemy);
                        useHybridMelee = true;
                        break;
                }
            }
            else
            {
                // Find type
                if (type == null)
                    type = randomEnemyType();
            }

            // Spawn
            if (debugging && OGEnemies)
                enemy = Instantiate(Resources.Load("Enemy")) as GameObject;
            else
            {

                if (type == typeof(EnemyType.MeleeEnemy))
                    enemy = Instantiate(Resources.Load("HybridEnemy")) as GameObject; // Melee/Hybrid enemy
                else
                    enemy = Instantiate(Resources.Load("RifleEnemy")) as GameObject; // Gun enemy
            }

            // Add behaviour
            enemy.AddComponent(type);

            // Enable hybrid (if applicable)
            if (useHybridMelee && type == typeof(EnemyType.MeleeEnemy))
                enemy.GetComponent<EnemyType.MeleeEnemy>().usePickups = true;
            
            // Positioning
            enemy.transform.position = generateNewPosition();
            enemy.transform.Rotate(Vector3.up, Random.Range(0.0f, 359.0f));

            // Turn the script behaviour script off (so they are inactive until room starts them)
            EnemyType.AbstractEnemy enemyScr = enemy.GetComponent<EnemyType.AbstractEnemy>();
            enemyScr.enabled = false;

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
