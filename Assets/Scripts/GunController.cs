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
    public enum whichTimeMechanic { NONE, STOPTIME, RESERVETIME, SPEEDUPTIME };
    public whichTimeMechanic tM = whichTimeMechanic.NONE;
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

        m_Scene = SceneManager.GetActiveScene();
        if(m_Scene.name == "SpeedUpScene")
        {
            tM = whichTimeMechanic.SPEEDUPTIME;
        }
        else if (m_Scene.name == "ReverseTimeScene")
        {
            tM = whichTimeMechanic.RESERVETIME;
        }
        else if (m_Scene.name == "StopTimeScene")
        {
            tM = whichTimeMechanic.STOPTIME;
        }
        else
        {
            Debug.Log("Error, cannot find the correct Scene Name to choose the correct Enum for the TimeMechanic");
        }
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

    public void timeMechanic()
    {
        if (tM == whichTimeMechanic.SPEEDUPTIME)
        {
            SpeedUpBulletController newBullet = Instantiate(sUpBullet, secondaryFirePoint.position, secondaryFirePoint.rotation);
            newBullet.speed = bulletSpeed;
        }
        else if (tM == whichTimeMechanic.RESERVETIME)
        {
            Debug.Log("Reverseing Time");
        }
    }
}
