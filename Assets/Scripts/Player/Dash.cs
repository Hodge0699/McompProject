using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        private GameObject dashObj;

        [SerializeField]
        private float dashDistance = 30f;
        private float dashDuration = 0f;
        private float dashCooldown = 0;

        [SerializeField]
        private float dashTrailDuration = 0.5f;

        private TimeMechanic.LocalTimeDilation myTime;

        private float playerSpeed;

        // Use this for initialization
        void Start()
        {
            myTime = GetComponent<TimeMechanic.LocalTimeDilation>();
            dashObj = transform.Find("DashTrail").gameObject;

            playerSpeed = GetComponent<PlayerController>().moveSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            if (dashDuration > 0)
                dashDuration -= Time.unscaledDeltaTime;
            else
                dashObj.SetActive(false);

            if (dashCooldown > 0)
                dashCooldown -= myTime.getDelta();
        }

        /// <summary>
        /// Activates the dash
        /// </summary>
        /// <param name="directionVector">Direction to travel in</param>
        public void activate(Vector3 directionVector)
        {
            if (dashCooldown > 0)
                return;

            dashObj.SetActive(true);
            CanMove(directionVector * playerSpeed * myTime.getDelta(), dashDistance);
            dashCooldown = 1.0f;
            dashDuration = dashTrailDuration;
        }

        /// <summary>
        /// Checks to see if the player can dash forward without hitting an object
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private void CanMove(Vector3 dir, float distance)
        {
            RaycastHit hitInfo = new RaycastHit();
            GameObject Gobject;
            if (Physics.Raycast(transform.position, dir, out hitInfo, 100f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                Gobject = hitInfo.collider.gameObject;
            else
                Gobject = null;

            if (Gobject == null)
                return;
            if (Gobject.tag == "Geometry" && hitInfo.distance <= 3)
            {
                return;
            }
            else if (Gobject.tag == "Geometry" && hitInfo.distance <= 8.0f)
            {

                transform.position += dir * (distance - hitInfo.distance) / 2;
            }
            else
            {

                transform.position += dir * distance;
            }
        }
    }
}