using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraControls
{
    public class RoomScaleCam : CameraController
    {
        // For every 1 camera size, how much of the room does it show?
        public float camToRoomRatio = 3.5f;

        public float scaleTime = 1.0f;
        private float scaleCounter = 0.0f;

        private float oldSize;
        private float newSize;
        private Camera myCam;



        private void Awake()
        {
            myCam = GetComponent<Camera>();
            oldSize = myCam.orthographicSize;
            newSize = myCam.orthographicSize;
        }

        public override void setRoom(Room room)
        {
            base.setRoom(room);

            calculateCamDetails();
        }

        private void Update()
        {
            if (scaleCounter < scaleTime)
            {
                scaleCounter += Time.unscaledDeltaTime;
                
                myCam.orthographicSize = Mathf.Lerp(oldSize, newSize, scaleCounter / scaleTime);
            }
        }

        /// <summary>
        /// Calculates the new size and position of the camera
        /// </summary>
        private void calculateCamDetails()
        {
            float maximumRoomSize = room.dimensions.x > room.dimensions.z ? room.dimensions.x : room.dimensions.z;

            oldSize = myCam.orthographicSize;
            newSize = maximumRoomSize / camToRoomRatio;
            targetPos = room.transform.position;

            scaleCounter = 0.0f;
        }
    }
}