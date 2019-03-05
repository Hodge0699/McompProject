using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBonus : MonoBehaviour {

    private Player.PlayerController player;
    // Use this for initialization
    void Start () {


        GameObject playerObj = Instantiate(Resources.Load("Player")) as GameObject;
        playerObj.transform.position = new Vector3(0, 0.5f, 0);

        player = playerObj.GetComponent<Player.PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
