﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour {

    private bool paused = false;

    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
          
          if(paused)
            {
               Time.timeScale = 1; 
            }
            else
                Time.timeScale = 0;
            paused = !paused;
        }
    }
}
