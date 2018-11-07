﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script needs to be connect to an empty object
//Make sure gizmos are visible!

public class EnemiesSpawn : MonoBehaviour
{

    public Vector3 center;
    public Vector3 size;

    public GameObject Enemyprefab;

    // Use this for initialization
    void Start()
    {

        center = transform.position;

        //Spawn();

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.O))
        {
            Spawn();
        }

    }

    public void Spawn()
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

        // Vector2 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.z / 2, size.z / 2));

        Instantiate(Enemyprefab, pos, Quaternion.identity);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        // Gizmos.DrawCube(center, size);
        Gizmos.DrawCube(transform.localPosition, size);
    }
}