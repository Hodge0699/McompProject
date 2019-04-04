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
        [SerializeField]
        private bool testEnemies = false;
        [SerializeField]
        private bool MeleeEnemies = false;

        /// <summary>
        /// Spawns an enemy of a specific type
        /// </summary>
        /// <param name="type">EnemyType class.</param>
        /// <returns>New enemy</returns>
        public GameObject spawn(System.Type type = null)
        {
            bool useHybridMelee = true;

            GameObject enemy;
            if (testEnemies)
                enemy = Instantiate(Resources.Load("rifleEnemy")) as GameObject;
            else
            {
                if (OGEnemies)
                    enemy = Instantiate(Resources.Load("Enemy")) as GameObject;
                else
                    enemy = Instantiate(Resources.Load("Enemy2")) as GameObject;
            }
                

            enemy.transform.position = generateNewPosition();
            enemy.transform.Rotate(Vector3.up, Random.Range(0.0f, 359.0f));

            if (type == null)
                enemy.AddComponent(randomEnemyType());
            else
                enemy.AddComponent(type);

            EnemyType.AbstractEnemy enemyScr = enemy.GetComponent<EnemyType.AbstractEnemy>();
            enemyScr.enabled = false;

            if (useHybridMelee && enemyScr.GetType() == typeof(EnemyType.MeleeEnemy))
                enemy.GetComponent<EnemyType.MeleeEnemy>().usePickups = true;

            return enemy;
        }

        /// <summary>
        /// Gets a random enemy type
        /// </summary>
        private System.Type randomEnemyType()
        {
            int rand = Random.Range(0, 2);
            if (MeleeEnemies)
                return typeof(EnemyType.GunEnemy);
            else
            {
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
