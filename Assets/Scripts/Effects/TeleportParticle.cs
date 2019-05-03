using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace effects
{
    public class TeleportParticle : EffectController {
        [Header("Duration of Effect")]
        [SerializeField]
        private float time;

        private void Awake()
        {
            setDuration(time);
        }

        // Update is called once per frame
        void Update()
        {
            destroyObject();
        }
    }
}
