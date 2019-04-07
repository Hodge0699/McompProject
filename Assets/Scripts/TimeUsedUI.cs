using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TimeUsedUI : MonoBehaviour {

    Scene scene;
    public Text txt;

    private void OnGUI()
    {
        if (scene.name == "LevelTwo")
        {
            txt.text = "Time Mechanic: Rewind\nUse: Right Click";
        }
    }

    // Use this for initialization
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
