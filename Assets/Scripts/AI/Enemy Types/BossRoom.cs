using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    public GameObject boss;
    public Transform spawnSpot;
    void Start()
    {
        spawn();
    }
    public void spawn()
    {
        boss = Instantiate(Resources.Load("boss")) as GameObject;
        boss.transform.position = spawnSpot.position;
        base.addEnemy(boss.GetComponent<EnemyType.AbstractEnemy>());
    }
}
