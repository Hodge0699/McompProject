using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

using RoomBuilding;

public class BossTeleportM : MonoBehaviour {

<<<<<<< HEAD
    [SerializeField]
    List<GameObject> teleportLocations;
    private BossEnemy bEnemy;
    [SerializeField]
    private Room myRoom;
    private bool stopChecking = false;
=======
    private List<GameObject> teleportLocations = new List<GameObject>();
    private BossEnemy bEnemy;
>>>>>>> master

    private void Start()
    {
<<<<<<< HEAD
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
=======
        bEnemy = this.GetComponent<BossEnemy>();
>>>>>>> master

        // Code I told Nicky to type while making nuggets that ended up not working because he had that ^ line
        // below it and it was called in Awake instead of Start. So I ended up doing it on my branch and 
        // fixing it but he wanted a mention anyway - Jake.
        Transform teleContainer = bEnemy.getRoom().transform.Find("BossTeleporter").transform;
        for (int i = 0; i < teleContainer.childCount; i++)
            teleportLocations.Add(teleContainer.GetChild(i).gameObject);
    }
}
