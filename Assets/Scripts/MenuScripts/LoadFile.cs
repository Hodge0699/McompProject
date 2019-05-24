using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gameController;

public class LoadFile : MonoBehaviour {

    GameController gController;
	// Use this for initialization
	void Start () {
        gController = GameObject.Find("GameController").GetComponent<GameController>();
	}

    public void loadFile()
    {
        gController.Load();
    }
	
}
