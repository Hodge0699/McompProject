using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeMechanic
{
    public class speedUp : TimeMechanic
    {
        public Transform secondaryFirePoint;

        public override void trigger()
        {
            Instantiate(Resources.Load("SpeedUpBullet"), secondaryFirePoint.position, secondaryFirePoint.rotation, GameObject.Find("Active Bullets").transform);
        }
    }
}