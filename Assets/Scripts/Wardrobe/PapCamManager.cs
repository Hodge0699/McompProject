using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wardrobe
{
    public class PapCamManager : MonoBehaviour
    {
        public List<AudioClip> shutterSounds;
        private PapCam[] cams;
        
        // Use this for initialization
        void Awake()
        {
            cams = GetComponentsInChildren<PapCam>();
        }

        public void shoot()
        {
            shuffleCamArray();

            foreach (PapCam c in cams)
            {
                c.setShutterSound(shutterSounds[Random.Range(0, shutterSounds.Count)]);
                c.shoot(Random.Range(0.0f, 0.7f));
            }
        }

        private void shuffleCamArray()
        {
            for (int i = 0; i < cams.Length; i++)
            {
                int swapIndex = Random.Range(0, cams.Length);
                PapCam temp = cams[i];
                cams[i] = cams[swapIndex];
                cams[swapIndex] = temp;
            }
        }
    }
}