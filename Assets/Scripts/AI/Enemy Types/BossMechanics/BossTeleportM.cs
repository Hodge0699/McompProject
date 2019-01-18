using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyType;

public class BossTeleportM : MonoBehaviour {

    [SerializeField]
    List<GameObject> teleportLocations;
    BossEnemy bEnemy;
    bool stopChecking = false;

    private void Awake()
    {
        foreach (GameObject teleportlocations in GameObject.FindGameObjectsWithTag("BossRandomTeleporter"))
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
        //removes null locations
        for (int i = 0; i < teleportLocations.Capacity; i++)
        {
            if (!teleportLocations[i])
            {
                teleportLocations.RemoveAt(i);
                Debug.Log("count me" + i);
            }
        }
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
