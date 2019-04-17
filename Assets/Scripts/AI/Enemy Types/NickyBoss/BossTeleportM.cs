using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

public class BossTeleportM : MonoBehaviour {

    [SerializeField]
    private List<GameObject> teleportLocations = new List<GameObject>(); // list of teleport locations 
    private EnemyType.Bosses.NickyBossMainScript bEnemy;
    [SerializeField]
    private HealthManager bHealth;
    [Header("Boss Teleporting Mechanic method")]
    [SerializeField]
    private bool furthestTeleport = false;
    [SerializeField]
    private bool randTeleport = false; // bool for user to decide if they want the boss to randomly teleport among the list of teleport locations
    [Header("Teleport health value")]
    [SerializeField]
    private int teleportHealth; // health you want the boss to teleport after
    private bool teleport = true;
    [Header("Teleport Particle Effect")]
    [SerializeField]
    private GameObject teleportEffect;
    [SerializeField]
    Transform teleContainer;

    private void Start()
    {
        // This will find every teleport location in the scene (including other rooms since multiple boss
        bEnemy = this.GetComponent<EnemyType.Bosses.NickyBossMainScript>();
        bHealth = GetComponent<HealthManager>();


        teleContainer = bEnemy.getRoom().transform.Find("BossTeleporter").transform;
        for (int i = 0; i < teleContainer.childCount; i++)
            teleportLocations.Add(teleContainer.GetChild(i).gameObject);
        
    }
	
	// Update is called once per frame
	void Update () {
        if(randTeleport == true)
            randomTeleport();
        if (furthestTeleport == true)
            furthestAway();
	}
    /// <summary>
    /// randomly teleports the boss to any of the teleport locations specified.
    /// </summary>
    private void randomTeleport()
    {
        if (bHealth.getHealth() < teleportHealth)
        {
            if (teleport == true)
            {
                StartCoroutine(wait());
                teleport = false;
            }
        }
    }
    IEnumerator wait()
    {
        Instantiate(teleportEffect, this.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1);
        int i = Random.Range(0, teleportLocations.Capacity);
        this.transform.position = teleportLocations[i].transform.position;

    }
    /// <summary>
    /// Teleportes the boss to the furthest away teleport location
    /// </summary>
    private void furthestAway()
    {
        Vector3 position = transform.position;
        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject item in teleportLocations)
        {
            Vector3 diff = item.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = item;
                distance = curDistance;
            }
        }

        if(bHealth.getHealth() <= teleportHealth && teleport == true)
        {
            this.transform.position = closest.transform.position;
            teleport = false;
        }

    }
}
