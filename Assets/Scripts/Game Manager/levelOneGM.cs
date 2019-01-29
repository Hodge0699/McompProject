using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelOneGM : MonoBehaviour {
    [SerializeField]
    GameObject player;
    [SerializeField]
    Player.PlayerController pC;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
        if (player != GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.AddComponent<SlowTimeManager>();
        }
	}
}
