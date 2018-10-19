using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUp : timeMechanic {

    public SpeedUpBulletController sUpB;
    public Transform secondaryFirePoint;
    public GunController gController;
    
    private void Update()
    {
        if (Input.GetButtonDown("Right Mouse"))
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        SpeedUpBulletController newBullet = Instantiate(sUpB, secondaryFirePoint.position, secondaryFirePoint.rotation);
        newBullet.speed = gController.bulletSpeed;
    }
}
