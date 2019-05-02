using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeMechanic
{
    public class speedUp : TimeMechanic
    {
        public Transform secondaryFirePoint;
        public bool canShoot = true;
        public override void trigger()
        {
            if (canShoot == true)
            {
                Instantiate(Resources.Load("SpeedUpBullet"), secondaryFirePoint.position, secondaryFirePoint.rotation, GameObject.Find("Active Bullets").transform);
                canShoot = false;
            }
        }
    }
}