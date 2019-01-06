﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Input;
using EnemyType;

public class RewindTimeManager : MonoBehaviour {

    bool isRewinding = false;

    private float recordTime = 3f;

    List<PointInTime> pointsInTime;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        pointsInTime = new List<PointInTime>();

        if (gameObject.GetComponent<Rigidbody>() != null)
            rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        // Toggle rewind to rewind for "recordTime" seconds
        if (Input.GetKeyDown(KeyCode.Space))
            StartRewind();
    }

    /// <summary>
    /// Uses FixedUpdate instead of update to keep physics consistant
    /// </summary>
    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    /// <summary>
    /// Rewinds untill end of list
    /// Destroys Bullets when they rewind past their time of creation
    /// </summary>
    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            if (gameObject.tag == "Bullet")
                Destroy(gameObject);

            StopRewind();
        }
    }

    /// <summary>
    /// Record position and rotation data into list for past "recordTime" secconds replacing end with new
    /// </summary>
    void Record()
    {
        if (pointsInTime.Count >  Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    /// <summary>
    /// Starts and Stops rewinding, makes rigid bodies kinematic while rewinding
    /// Prevents Player and Enemies from shooting while rewinding
    /// </summary>
    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;

        if (gameObject.tag == "Enemy")
            gameObject.GetComponent<GunEnemy>().canShoot = false;

        if (gameObject.tag == "Player")
            gameObject.GetComponent<PlayerInputManager>().canShoot = false;
        
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;

        if (gameObject.tag == "Enemy")
            gameObject.GetComponent<GunEnemy>().canShoot = true;

        if (gameObject.tag == "Player")
            gameObject.GetComponent<PlayerInputManager>().canShoot = true;
        
    }
}
