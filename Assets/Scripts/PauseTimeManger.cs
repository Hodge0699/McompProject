using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTimeManger : MonoBehaviour {

    public float pauseTimeFactor = 0.0f;
    public float pauseTimeLength = 5f;
    GameObject[] PauseObjects;
	
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown("R"))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.0f;
                PauseObjects = GameObject.FindGameObjectsWithTag("Enemies");
            }

            else
            {
                Time.timeScale = 1.0f;

            }
            
        }
	}

    public void Pausemode()
    {
        Time.timeScale = pauseTimeFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.0f;
    }
}

