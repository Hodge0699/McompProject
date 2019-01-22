using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

using RoomBuilding;

public class BossTeleportM : MonoBehaviour {

    [SerializeField]
    List<GameObject> teleportLocations;
    private BossEnemy bEnemy;
    [SerializeField]
    private Room myRoom;
    private bool stopChecking = false;

    private void Awake()
    {
        // This will find every teleport location in the scene (including other rooms since multiple boss
        // initially spawn before the player decideds which one to walk into), fix this - Jake - suck me - Nicky 
        myRoom = bEnemy.getMyRoom();
        foreach (GameObject teleportlocations in myRoom.transform.Find("BossTeleporter"))
        {
            teleportLocations.Add(teleportlocations);
        }

        bEnemy = this.GetComponent<BossEnemy>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Teleport Location Capacity " + teleportLocations.Capacity);
        if (bEnemy.currentHealth < 800)
        {
            int i = Random.Range(0, 3);
            this.transform.position = teleportLocations[i].transform.position;
        }
        

	}

    void checkForErrors()
    {

        
    }
}
