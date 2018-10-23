﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //   public GameObject player;

    //   public Vector3 offset;

    //// Use this for initialization
    //void Start () {


    //       offset = transform.position - player.transform.position;

    //}

    //// Update is called once per frame
    //void Update () {

    //       transform.position = player.transform.position + offset;

    //}

    private Transform target;

    public float smoothing = 5f;

    private Vector3 offset;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        offset = transform.position - target.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
