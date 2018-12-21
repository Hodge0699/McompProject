using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossOneSpawner : BossSpawner 
{

    public GameObject boss;
    public Transform spawnSpot;
    // Use this for initialization
    void Start()
    {
        spawn();
    }
    
    private void spawn()
    {
        boss = Instantiate(Resources.Load("boss")) as GameObject;
        boss.transform.position = spawnSpot.position;
        //base.spawn(boss);
    }
}
