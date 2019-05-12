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

        AbstractEnemy bossScr = boss.GetComponent<AbstractEnemy>();

        // Make boss look at doorway on spawn
        boss.transform.LookAt(boss.transform.position - transform.right);

        base.addEnemy(bossScr);

    }

    public override void addDoor(DoorController door)
    {
        base.addDoor(door);

        // Remove it from doors array so it doesn't open when room beaten
        doors.Remove(door);
    }
}
