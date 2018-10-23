using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isFiring;
    public BulletController bullet;
    public SpeedUpBulletController sUpBullet;
    public float bulletSpeed;
    public float timeBetweenShots;
    private float timeBetweenShootCounter;
    public Transform primaryFirePoint;
    public Transform secondaryFirePoint;
    Scene m_Scene;


    private GameObject smile;
    private GameObject angry;

    private void Awake ()
    {
        smile = GameObject.Find("SmileFace");
        angry = GameObject.Find("AngryFace");
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isFiring)
        {
            smile.active = false;
            angry.active = true;

            timeBetweenShootCounter -= Time.deltaTime;
            if (timeBetweenShootCounter <= 0)
            {
                shooting();
            }
        }
        else
        {
            smile.active = true;
            angry.active = false;
            timeBetweenShootCounter = 0;
        }
    }

    void shooting()
    {
        timeBetweenShootCounter = timeBetweenShots;
        BulletController newBullet = Instantiate(bullet, primaryFirePoint.position, primaryFirePoint.rotation);
        newBullet.speed = bulletSpeed;
    }
}
