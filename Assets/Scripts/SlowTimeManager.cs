using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace TimeMechanic
{
    public class SlowTimeManager : TimeMechanic
    {
        public float slowDownFactor = 0.05f;
        private float normTimeFactor = 1.0f;

        private Player.PlayerInputManager playerInput;

        override protected void Start()
        {
            base.Start();

            playerInput = GetComponent<Player.PlayerInputManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerInput.isPaused())
                return;

            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0)
                StartSlowMotion();
            else
                StopSlowMotion();
        }

        void StartSlowMotion()
        {
            Time.timeScale = slowDownFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        void StopSlowMotion()
        {
            Time.timeScale = normTimeFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
}