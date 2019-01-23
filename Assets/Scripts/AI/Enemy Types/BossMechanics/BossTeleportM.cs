using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

using RoomBuilding;

public class BossTeleportM : MonoBehaviour {

    [SerializeField]
    private Room myRoom;
    private bool stopChecking = false;
    [SerializeField]
    private List<GameObject> teleportLocations = new List<GameObject>();
    private BossEnemy bEnemy;
    [SerializeField]
    private bool randTeleport = false;
    private bool teleport = true;

    private void Start()
    {
        // This will find every teleport location in the scene (including other rooms since multiple boss
        // initially spawn before the player decideds which one to walk into), fix this - Jake - suck me - Nicky 
        bEnemy = this.GetComponent<BossEnemy>();
        myRoom = bEnemy.getMyRoom();

        Transform teleContainer = bEnemy.getRoom().transform.Find("BossTeleporter").transform;
        for (int i = 0; i < teleContainer.childCount; i++)
            teleportLocations.Add(teleContainer.GetChild(i).gameObject);
        
    }
	
	// Update is called once per frame
	void Update () {
        if(randTeleport == true)
            randomTeleport();
        

	}
    /// <summary>
    /// randomly teleports the boss to any of the teleport locations specified.
    /// </summary>
    private void randomTeleport()
    {
        if (bEnemy.currentHealth < 700)
        {
            if (teleport == true)
            {
                int i = Random.Range(0, teleportLocations.Capacity);
                this.transform.position = teleportLocations[i].transform.position;
            }
        }
        teleport = false;
    }
}
