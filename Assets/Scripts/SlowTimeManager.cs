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

        private void Awake()
        {
            StopSlowMotion();
        }

        override protected void Start()
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name != level)
            {
                enabled = false;
                return;
            }

            if (GetComponent<Player.PlayerInputManager>())
                GetComponent<Player.PlayerInputManager>().setTimeMechanic(this);

            StopSlowMotion();

            playerInput = GetComponent<Player.PlayerInputManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerInput.isPaused())
                return;
            // checks player's speed
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0)
                StartSlowMotion();
            else
                StopSlowMotion();
        }
        // slow down time function
        void StartSlowMotion()
        {
            Time.timeScale = slowDownFactor; // adjust time scale to lower value
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // updates FixedUpdate
        }
        // resets tune function to run time
        void StopSlowMotion()
        {
            Time.timeScale = normTimeFactor; // adjust time scale to default
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // updates FixedUpdate
        }
    }
}