using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;
public class BossRoom : Room
{
    public GameObject bossPrefab;
    private GameObject boss;
    public Transform spawnSpot;

    void Start()
    {
        spawn();

        createGeometryShaders();
    }

    public void spawn()
    {
        boss = (GameObject)Instantiate(bossPrefab);
        boss.transform.position = spawnSpot.position;
        base.addEnemy(boss.GetComponent<EnemyType.AbstractEnemy>());
    }
}
