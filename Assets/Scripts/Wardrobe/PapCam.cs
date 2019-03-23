using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wardrobe
{
    public class PapCam : MonoBehaviour
    {
        public float exposureTime = 0.1f;
        private float counter = 0.0f;

        private float shootDelay = 0.0f;

        private Light flash;
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            flash = GetComponent<Light>();
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (shootDelay > 0.0f)
            {
                shootDelay -= Time.deltaTime;

                if (shootDelay <= 0.0f)
                    shoot();
            }

            if (counter > 0.0f)
            {
                counter -= Time.deltaTime;

                if (counter <= 0.0f)
                    endShoot();
            }
        }

        public void shoot(float delay = 0.0f)
        {
            if (delay != 0.0f)
            {
                shootDelay = delay;
                return;
            }

            flash.enabled = true;

            audioSource.Play();

            counter = exposureTime;
        }

        private void endShoot()
        {
            flash.enabled = false;
        }

        public void setShutterSound(AudioClip sound)
        {
            audioSource.clip = sound;
        }
    }
}