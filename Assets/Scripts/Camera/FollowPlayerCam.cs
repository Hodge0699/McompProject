using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraControls
{
    public class FollowPlayerCam : CameraController
    {
        public Vector2 size = new Vector2(8.0f, 4.5f);

        Transform player;

        protected void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            targetPos = getBoundedTargetPos(player.position);
        }


        /// <summary>
        /// Calculates closest centred position of camera over target with offset while remaining in room bounds.
        /// </summary>
        /// <param name="target">Target to try to centre over.</param>
        /// <returns>Closest centred position within room bounds.</returns>
        private Vector3 getBoundedTargetPos(Vector3 target)
        {
            Vector3 targetPos = target;

            if (room == null)
                return targetPos;

            Vector3 roomPos = room.transform.position;
            Vector3 roomSize = room.dimensions;

            if (targetPos.x > roomPos.x + (roomSize.x / 2) - size.x)
                targetPos.x = roomPos.x + (roomSize.x / 2) - size.x;
            else if (targetPos.x < roomPos.x - (roomSize.x / 2) + size.x)
                targetPos.x = roomPos.x - (roomSize.x / 2) + size.x;

            if (targetPos.z > roomPos.z + (roomSize.z / 2) - (size.y * 0.5f))
                targetPos.z = roomPos.z + (roomSize.z / 2) - (size.y * 0.5f);
            else if (targetPos.z < roomPos.z - (roomSize.z / 2) + (size.y * 1.5f))
                targetPos.z = roomPos.z - (roomSize.z / 2) + (size.y * 1.5f);

            return targetPos;
        }
    }
}