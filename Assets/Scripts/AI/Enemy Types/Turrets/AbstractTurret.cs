using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyType.Turrets
{

    public class AbstractTurret : MonoBehaviour
    {
        public int gunCount = 8;
        public float radius = 0.6f;

        private List<GunController> guns = new List<GunController>();
        protected VisionCone fov;

        // Use this for initialization
        public void Start()
        {
            generateFirePoints();
            fov = GetComponent<VisionCone>();
        }

        /// <summary>
        /// Generates fire points around the turret
        /// </summary>
        private void generateFirePoints()
        {
            for (int i = 0; i < gunCount; i++)
            {
                GameObject firePoint = new GameObject();
                firePoint.name = "Fire Point " + (i + 1);
                firePoint.transform.parent = this.transform;

                float angle = (360 / gunCount) * i;

                float x = radius * Mathf.Cos(angle * 3.14f / 180.0f);
                float z = radius * Mathf.Sin(angle * 3.14f / 180.0f);

                firePoint.transform.localPosition = new Vector3(x, -0.75f, z);
                guns.Add(firePoint.AddComponent<GunController>());
                Vector3 dir = (firePoint.transform.position - transform.position).normalized;
                dir.y = 0.0f;
                firePoint.transform.LookAt(firePoint.transform.position + (dir * 10));

                guns[i].ignoreTags.Add("Boss");
                guns[i].firePoint = firePoint.transform;
            }
        }

        /// <summary>
        /// Shoots all guns 
        /// </summary>
        protected void shoot()
        {
            for (int i = 0; i < gunCount; i++)
                guns[i].shoot();
        }

        /// <summary>
        /// Shoots a certain gun in the guns array
        /// </summary>
        /// <param name="gun">Gun index</param>
        protected void shoot(int gun)
        {
            guns[gun].shoot();
        }

        protected void setGuns(System.Type gun)
        {
            for (int i = 0; i < gunCount; i++)
                guns[i].setGun(gun, true);
        }
    }
}