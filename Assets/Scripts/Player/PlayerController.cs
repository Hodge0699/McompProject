using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed;

        public bool debugging = false;
        public bool useController;

        private GameObject myCamera = null;
        private Vector3 cameraOffset = new Vector3(0f, 7f, -10f);

        private Room currentRoom;

        public enum EMOTION { HAPPY, ANGRY };
        private GameObject smile;
        private GameObject angry;

        private GameObject UI;

        private void Awake()
        {
            myCamera = Instantiate(Resources.Load("Main Camera"), transform.position + cameraOffset, Quaternion.Euler(33, 0, 0)) as GameObject;
            myCamera.GetComponent<Camera>().nearClipPlane = -2.5f; // Let objects penetrate camera by 2.5 units before culling (stops visible wall cull)

            UI = Instantiate(Resources.Load("PlayerUI")) as GameObject;

            GetComponent<PlayerHealthManager>().init(UI);

            smile = transform.Find("SmileFace").gameObject;
            angry = transform.Find("AngryFace").gameObject;
        }

        /// <summary>
        /// Sets the player's current room for reference.
        /// </summary>
        /// <param name="room">Player's current room.</param>
        public void setRoom(Room room)
        {
            this.currentRoom = room;
        }

        /// <summary>
        /// Gets the current room the player is in
        /// </summary>
        public Room getCurrentRoom()
        {
            return currentRoom;
        }

        /// <summary>
        /// Sets Darren's face to an emotion
        /// </summary>
        /// <param name="e">Happy or angry.</param>
        public void setFace(EMOTION e)
        {
            smile.SetActive(e == EMOTION.HAPPY);
            angry.SetActive(e == EMOTION.ANGRY);
        }

        /// <summary>
        /// Returns PlayerUI so scene doesn't have to be searched.
        /// </summary>
        /// <returns></returns>
        public GameObject getUI()
        {
            return UI;
        }
    }
}