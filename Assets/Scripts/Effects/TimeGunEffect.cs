using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace effects
{
    public class TimeGunEffect : EffectController
    {
        GameObject timeBullet;
        private void Start()
        {
            timeBullet = GameObject.Find("IgnoreTimeBullet(Clone)");
            attachToParent(timeBullet);
        }
        // Update is called once per frame
        void Update()
        {
            destroyObject();
        }
    }
}
