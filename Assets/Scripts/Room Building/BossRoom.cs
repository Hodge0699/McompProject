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
        boss = Instantiate(bossPrefab);
        boss.transform.position = spawnSpot.position;
        boss.GetComponent<EnemyType.AbstractEnemy>().enabled = false;
        base.addEnemy(boss.GetComponent<EnemyType.AbstractEnemy>());

        // Make boss look at doorway on spawn
        boss.transform.LookAt(transform.position + -transform.right * 10);
    }

    public override void addDoor(DoorController door)
    {
        base.addDoor(door);

        // Remove it from doors array so it doesn't open when room beaten
        doors.Remove(door);
    }
}
