﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed;

        public bool debugging = false;

        private CameraControls.CameraController myCamera;

        private Room currentRoom;

        public enum EMOTION { HAPPY, ANGRY };
        private GameObject smile;
        private GameObject angry;

        private GameObject UI;

        private List<GameObject> doorIndicators = new List<GameObject>();

        private void Awake()
        {
            GameObject hat = Instantiate(HatManager.getHat()) as GameObject;
            hat.transform.SetParent(transform.Find("Hat Anchor"), false);
            hat.name = "Hat";

            GameObject camera = Instantiate(Resources.Load("Main Camera"), transform.position, Quaternion.Euler(33, 0, 0)) as GameObject;
            myCamera = camera.GetComponent<CameraControls.CameraController>();

            UI = Instantiate(Resources.Load("PlayerUI")) as GameObject;

            GetComponent<HealthManager.PlayerHealthManager>().init(UI);

            smile = transform.Find("SmileFace").gameObject;
            angry = transform.Find("AngryFace").gameObject;
        }

        /// <summary>
        /// Sets the player's current room for reference.
        /// </summary>
        /// <param name="room">Player's current room.</param>
        public void setRoom(Room room)
        {
            currentRoom = room;

            myCamera.setRoom(room);

            for (int i = 0; i < doorIndicators.Count; i++)
                Destroy(doorIndicators[i]);
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

        /// <summary>
        /// Adds a chevron that points to an open door.
        /// </summary>
        /// <param name="targetPosition">Position of the open door.</param>
        /// <returns>The indicator chevron.</returns>
        public GameObject addIndicator(Vector3 targetPosition)
        {
            GameObject indicator = Instantiate(Resources.Load("DoorIndicator")) as GameObject;

            indicator.transform.parent = this.transform;
            indicator.GetComponent<DoorIndicator>().setTarget(targetPosition);

            doorIndicators.Add(indicator);

            return indicator;
        }
    }
}