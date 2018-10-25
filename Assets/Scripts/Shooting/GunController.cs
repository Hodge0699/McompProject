using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GunController : MonoBehaviour {

    public BulletController bullet;
    public SpeedUpBulletController sUpBullet;
    public float bulletSpeed;
    public float timeBetweenShots = 0.5f; //jack
    private float shotCooldown = 0.0f;
    public Transform primaryFirePoint;
    public Transform secondaryFirePoint;

    private PlayerController player;

    private enum EMOTION { HAPPY, ANGRY };
    private GameObject smile;
    private GameObject angry;


    public bool debugging = false;

    private void Awake ()
    {
        // Only search through parent's children, quicker than searching through whole scene - Jake
        smile = transform.parent.Find("SmileFace").gameObject;
        angry = transform.parent.Find("AngryFace").gameObject;

        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            setFace(EMOTION.ANGRY);

            shoot();
        }
        else
            setFace(EMOTION.HAPPY);

        if (shotCooldown >= 0.0f)
            shotCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// Attempts to shoot a bullet
    /// </summary>
    void shoot()
    {
        // Return early if cooldown not reached
        if (shotCooldown >= 0.0f)
            return;

        BulletController newBullet = Instantiate(bullet, primaryFirePoint.position, primaryFirePoint.rotation);
        newBullet.speed = bulletSpeed;

        if (player.getMousePos() != null) // If mouse in valid position, point bullet at target
        {
            Vector3 target = player.getMousePos().Value;

            newBullet.transform.LookAt(target);

            if (debugging)
                Debug.DrawLine(primaryFirePoint.position, target, Color.red, 2.0f);
        }

        shotCooldown = timeBetweenShots;
    }

    /// <summary>
    /// Sets Darren's face to an emotion
    /// </summary>
    /// <param name="e">Happy or angry.</param>
    private void setFace(EMOTION e)
    {
        smile.SetActive(e == EMOTION.HAPPY);
        angry.SetActive(e == EMOTION.ANGRY);
    }
}
