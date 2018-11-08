
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupSG : MonoBehaviour
{

    public SGbulletController SG;
    public GunController GC;



    public float bulletSpeed;
    public float time = 5.0f;


    void OnTriggerEnter(Collider other)

    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ShotGun pickup");
            GC = other.GetComponentInChildren<GunController>();
            //GC.ShootSG();
            //GC.resetTime();
            GC.currentGun = new Gun.Shotgun();



        }

    }
    void Start()
    {
        //SG = new SGbulletController();
    }
    private void Reset()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Debug.Log("getting inside the reset function");
            //GC.Shoot();
            GC.currentGun = new Gun.Handgun();
        }
        time = 5.0f;
    }
}

