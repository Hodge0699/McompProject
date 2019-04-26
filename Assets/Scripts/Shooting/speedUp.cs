using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUp : timeMechanic {

    public Transform secondaryFirePoint;

    private void Update()
    {
        if (Input.GetButtonDown("Right Mouse"))
            Shoot();
    }

    public override void Shoot()
    {
        Instantiate(Resources.Load("SpeedUpBullet"), secondaryFirePoint.position, secondaryFirePoint.rotation, GameObject.Find("Active Bullets").transform);
    }
}
