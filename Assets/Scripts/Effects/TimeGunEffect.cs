using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace effects
{
    public class TimeGunEffect : EffectController
    {
        // Update is called once per frame
        void Update()
        {
            destroyObject();
        }

        public void updateGameObject(GameObject gObject)
        {
            attachToParent(gObject);
        }
    }
}
