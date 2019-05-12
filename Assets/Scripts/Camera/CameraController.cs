using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraControls
{
    public abstract class CameraController : MonoBehaviour
    {
        private Vector3 offset = new Vector3(0f, 7f, -10f);

        public float speed = 5.0f;

        protected Vector3 targetPos;

        protected Room room;

        private Vector3 velocity = Vector3.zero;

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, speed * Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Sets the room that the camera is currently showing
        /// </summary>
        public virtual void setRoom(Room room)
        {
            this.room = room;
        }
    }
}