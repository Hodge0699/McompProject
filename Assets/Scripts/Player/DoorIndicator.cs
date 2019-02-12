using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class DoorIndicator : MonoBehaviour
    {
        private Vector3 target;

        // Update is called once per frame
        void Update()
        {
            if (target == null)
                return;

            transform.localPosition = Vector3.zero; // Reset back to centre
            transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z); // Float 0.25f above ground
            transform.LookAt(target);
            transform.Translate(transform.forward * 2, Space.World);
        }

        /// <summary>
        /// Sets the door to point to.
        /// </summary>
        /// <param name="target">Open door.</param>
        public void setTarget(Vector3 target)
        {
            this.target = target;
            this.target.y = 0.25f;
        }
    }
}