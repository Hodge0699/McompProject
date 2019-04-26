using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedUp : timeMechanic {

    public GameObject sUpB;

    public Transform secondaryFirePoint;

    public float bulletSpeed = 10.0f;
    
    private void Update()
    {
        if (Input.GetButtonDown("Right Mouse"))
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        GameObject newBullet = Instantiate(sUpB, secondaryFirePoint.position, secondaryFirePoint.rotation);
        //newBullet.speed = bulletSpeed;
    }
}
